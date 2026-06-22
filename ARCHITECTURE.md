# Kiến trúc dự án Project_CNPM

`Project_CNPM` là ứng dụng ASP.NET Core 8 theo mô hình MVC kết hợp API controller. Backend C# dùng Entity Framework Core làm việc với SQL Server, đồng thời gọi một service Python Flask để tạo embedding và gợi ý thuốc bằng vector search.

## 1. Tổng quan hệ thống

Hệ thống hiện có ba nhóm chức năng chính:

- Quản trị hệ thống: đăng nhập/đăng ký, dashboard, quản lý người dùng, thuốc, bệnh, triệu chứng, cảnh báo an toàn và liên kết thuốc-bệnh.
- Dự đoán cho người dùng: người dùng gửi bệnh và mô tả triệu chứng, hệ thống gọi AI service để gợi ý thuốc, lưu lịch sử, trả cảnh báo và nhận đánh giá kết quả.
- AI service: Flask API xử lý embedding thuốc, xóa vector thuốc, tìm thuốc phù hợp và kiểm tra trạng thái service.

Luồng backend tổng quát:

1. Client gọi API ASP.NET Core.
2. Controller nhận DTO, route parameter hoặc query parameter.
3. Controller kiểm tra dữ liệu cơ bản và gọi service tương ứng.
4. Service xử lý nghiệp vụ, đọc/ghi dữ liệu qua `ApplicationDbContext`.
5. `ApplicationDbContext` ánh xạ dữ liệu SQL Server sang entity trong `Models`.
6. Controller trả JSON cho API hoặc Razor view cho MVC.

Luồng liên kết thuốc-bệnh:

1. Admin gọi API trong `MedicineDiseaseMappingController`.
2. `MedicineDiseaseMappingService` tạo hoặc xóa record trong bảng `BenhThuoc`.
3. Khi tạo liên kết, service gọi Flask `POST /embed-drug` để đồng bộ vector thuốc.
4. Khi xóa liên kết, service gọi Flask `POST /remove-drug`, sau đó đồng bộ lại các mapping còn lại của thuốc nếu cần.

Luồng dự đoán thuốc:

1. User đăng nhập qua `AuthController` và nhận JWT.
2. User gọi `POST /api/UserPrediction/predict`.
3. `UserPredictionService` kiểm tra bệnh đang hoạt động và nội dung triệu chứng.
4. Service gọi Flask `POST /predict` với bệnh, triệu chứng và `top_k = 5`.
5. Service lấy thêm cảnh báo dị ứng, bệnh nền và tương tác thuốc từ SQL Server.
6. Kết quả được lưu vào `LichSuDuDoan` và `KetQuaDuDoan`.
7. API trả danh sách thuốc gợi ý kèm `ResultId` để frontend gửi feedback.
8. User có thể gửi đánh giá qua `POST /api/UserPrediction/feedback`.
9. User có thể xem hoặc xóa lịch sử qua các API `history`.

## 2. Cấu trúc thư mục

```text
Project_CNPM/
+-- Areas/
|   +-- Admin/
|       +-- Controllers/
|       +-- DTOs/
|       +-- Services/
+-- Controllers/
|   +-- HomeController.cs
|   +-- UserPredictionController.cs
+-- Data/
|   +-- ApplicationDbContext.cs
+-- DTOs/
|   +-- UserDto.cs
+-- Models/
+-- Services/
|   +-- IUserPredictionService.cs
|   +-- UserPredictionService.cs
+-- Views/
+-- wwwroot/
+-- docs/
+-- src/
|   +-- app.py
|   +-- predict.py
|   +-- seed_from_csv.py
|   +-- vector_store.py
+-- appsettings.json
+-- appsettings.Development.json
+-- docker-compose.yml
+-- Program.cs
+-- Project_CNPM.csproj
+-- SQL.sql
+-- drug_metadata.json
+-- drugs_vector.npy
+-- TutorialViews.md
+-- ARCHITECTURE.md
```

## 3. Công nghệ sử dụng

Backend C#:

- ASP.NET Core MVC/API trên `.NET 8`.
- Entity Framework Core 8.
- SQL Server.
- JWT Bearer Authentication.
- `BCrypt.Net-Next` để hash và kiểm tra mật khẩu.
- `HttpClient` để gọi Flask service.

AI service Python:

- Flask.
- `sentence-transformers`.
- Model `intfloat/multilingual-e5-small`.
- NumPy.
- `requests`.
- `pyodbc` cho script seed.

Hạ tầng local:

- `docker-compose.yml` chạy SQL Server 2019 trên port `1433`.
- `SQL.sql` chứa schema và dữ liệu khởi tạo.
- Flask service chạy mặc định tại `http://localhost:5000`.

## 4. Cấu hình ứng dụng

`Program.cs` đăng ký các thành phần chính:

- `ApplicationDbContext` dùng connection string `DefaultConnection`.
- `IAuthService -> AuthService`
- `IMedicineAdminService -> MedicineAdminService`
- `IDiseaseAdminService -> DiseaseAdminService`
- `ISymptomAdminService -> SymptomAdminService`
- `IUserAdminService -> UserAdminService`
- `ISafetyWarningAdminService -> SafetyWarningAdminService`
- `IDashboardAdminService -> DashboardAdminService`
- `IMedicineDiseaseMappingService -> MedicineDiseaseMappingService` qua `AddHttpClient`
- `IUserPredictionService -> UserPredictionService` qua `AddHttpClient`
- JWT Bearer Authentication.

Middleware pipeline:

- `UseStaticFiles`
- `UseRouting`
- `UseAuthentication`
- `UseAuthorization`
- MVC route mặc định `{controller=Home}/{action=Index}/{id?}`

`UseHttpsRedirection` đang được comment.

## 5. Xác thực và phân quyền

`AuthService` tạo JWT khi đăng nhập thành công. Token có các claim:

- `ClaimTypes.NameIdentifier`: mã người dùng.
- `ClaimTypes.Email`: email.
- `ClaimTypes.Name`: họ tên.
- `ClaimTypes.Role`: vai trò người dùng.

Phân quyền API:

- `AuthController` không yêu cầu token.
- Các API quản trị phần lớn yêu cầu role `Admin`.
- `MedicineController` yêu cầu đăng nhập để xem thuốc; tạo, cập nhật và bật/tắt thuốc yêu cầu role `Admin`.
- `UserPredictionController` yêu cầu user đã đăng nhập.

## 6. API controller

### AuthController

Route base: `api/Auth`

- `POST /api/Auth/login`
- `POST /api/Auth/register`
- `POST /api/Auth/logout?userId={id}`

Đăng nhập trả JWT, `userId` và `username`.

### DashboardController

Route base: `api/admin/Dashboard`

- `GET /api/admin/Dashboard/statistics`

Trả thống kê tổng số user, thuốc, bệnh đang hoạt động và số lượt dự đoán.

### DiseaseController

Route base: `api/Disease`

- `GET /api/Disease`
- `GET /api/Disease/{id}`
- `POST /api/Disease/create`
- `PATCH /api/Disease/Update`
- `PATCH /api/Disease/toggle?id={id}`

Quản lý danh mục bệnh, gồm xem danh sách, xem chi tiết, tạo, cập nhật và bật/tắt trạng thái.

### SymptomController

Route base: `api/Symptom`

- `GET /api/Symptom`
- `GET /api/Symptom/{id}`
- `POST /api/Symptom/create`
- `PATCH /api/Symptom/update`
- `PATCH /api/Symptom/toggle?id={id}`

Quản lý danh mục triệu chứng.

### MedicineController

Route base: `api/Medicine`

- `GET /api/Medicine`
- `GET /api/Medicine/{id}`
- `POST /api/Medicine/create`
- `PATCH /api/Medicine/update`
- `PATCH /api/Medicine/toggle?id={id}&isActive={true|false}`

Quản lý danh mục thuốc và trạng thái hoạt động của thuốc.

### MedicineDiseaseMappingController

Route base: `api/admin/MedicineDiseaseMapping`

- `GET /api/admin/MedicineDiseaseMapping/medicine/{medicineId}/diseases`
- `GET /api/admin/MedicineDiseaseMapping/disease/{diseaseId}/medicines`
- `POST /api/admin/MedicineDiseaseMapping/link`
- `DELETE /api/admin/MedicineDiseaseMapping/unlink/medicine/{medicineId}/disease/{diseaseId}`

Quản lý quan hệ thuốc-bệnh và đồng bộ dữ liệu sang vector store của AI service.

### SafetyWarningController

Route base: `api/SafetyWarning`

Dị ứng:

- `GET /api/SafetyWarning/allergies`
- `POST /api/SafetyWarning/allergies/create`
- `PATCH /api/SafetyWarning/allergies/update`
- `PATCH /api/SafetyWarning/allergies/toggle?id={id}&isActive={true|false}`

Bệnh nền:

- `GET /api/SafetyWarning/background-diseases`
- `POST /api/SafetyWarning/background-diseases/create`
- `PATCH /api/SafetyWarning/background-diseases/update`
- `PATCH /api/SafetyWarning/background-diseases/toggle?id={id}&isActive={true|false}`

Tương tác thuốc:

- `GET /api/SafetyWarning/drug-interactions`
- `POST /api/SafetyWarning/drug-interactions/create`
- `DELETE /api/SafetyWarning/drug-interactions/{medicine1Id}/{medicine2Id}`

Quản lý dữ liệu cảnh báo an toàn cho thuốc.

### UserController

File hiện nằm tại `Areas/Admin/Controllers/UserAdminController.cs`, nhưng class bên trong là `UserController`, nên route runtime là `api/User`.

- `GET /api/User`
- `PATCH /api/User/{userId}/lock?isLocked={true|false}`
- `DELETE /api/User/{userId}`

Quản lý người dùng, khóa/mở khóa tài khoản và xóa mềm.

### UserPredictionController

Route base: `api/UserPrediction`

- `POST /api/UserPrediction/predict`
- `POST /api/UserPrediction/feedback`
- `GET /api/UserPrediction/history`
- `DELETE /api/UserPrediction/history/{historyId}`

Xử lý dự đoán thuốc cho user, nhận đánh giá kết quả, xem lịch sử và xóa lịch sử.

### HomeController

Controller MVC mặc định:

- `GET /Home/Index`
- `GET /Home/Privacy`
- `GET /Home/Error`

## 7. Service C#

### AuthService

- Đăng nhập.
- Đăng ký.
- Logout.
- Hash mật khẩu bằng `BCrypt`.
- Tạo JWT có hạn 7 ngày.
- Kiểm tra tài khoản bị khóa hoặc đã xóa mềm.

### DashboardAdminService

Tổng hợp thống kê:

- User đang hoạt động.
- Thuốc đang hoạt động.
- Bệnh đang hoạt động.
- Lượt dự đoán.

### DiseaseAdminService

- Lấy danh sách bệnh.
- Lấy bệnh theo id.
- Tạo bệnh.
- Cập nhật bệnh.
- Bật/tắt trạng thái bệnh.

### SymptomAdminService

- Lấy danh sách triệu chứng.
- Lấy triệu chứng theo id.
- Tạo triệu chứng.
- Cập nhật triệu chứng.
- Bật/tắt trạng thái triệu chứng.

### MedicineAdminService

- Lấy danh sách thuốc.
- Lấy thuốc theo id.
- Tạo thuốc.
- Cập nhật thuốc.
- Bật/tắt trạng thái thuốc.

### MedicineDiseaseMappingService

- Liên kết thuốc với bệnh.
- Hủy liên kết thuốc với bệnh.
- Lấy danh sách bệnh của một thuốc.
- Lấy danh sách thuốc của một bệnh.
- Gọi Flask service để đồng bộ vector thuốc.

### SafetyWarningAdminService

- Quản lý dị ứng.
- Quản lý bệnh nền.
- Quản lý tương tác thuốc.
- Chuẩn hóa cặp thuốc theo thứ tự `min/max` để tránh trùng cặp đảo chiều.

### UserAdminService

- Lấy danh sách người dùng.
- Khóa/mở khóa người dùng.
- Xóa mềm người dùng.

### UserPredictionService

- Gọi Flask `POST /predict`.
- Lấy cảnh báo dị ứng, bệnh nền và tương tác thuốc liên quan.
- Lưu lịch sử dự đoán vào `LichSuDuDoan`.
- Lưu snapshot kết quả vào `KetQuaDuDoan`.
- Trả kết quả dự đoán cho frontend.
- Lưu feedback của user vào `DanhGiaDuDoan`.
- Lấy lịch sử dự đoán của user.
- Xóa lịch sử dự đoán của user.

## 8. DTO

DTO admin:

- `AuthDto.cs`: `loginDto`, `registerDto`.
- `DashboardDto.cs`: `SystemStatisticsDto`.
- `DiseaseDto.cs`: `DiseaseCreateDto`, `DiseaseUpdateDto`, `DiseaseDetailDto`.
- `MedicineDto.cs`: `MedicineCreateDto`, `MedicineUpdateDto`, `MedicineDetailDto`.
- `SafetyWarningDto.cs`: `AllergyCreateUpdateDto`, `BackgroundDiseaseCreateUpdateDto`, `DrugInteractionCreateDto`, `DrugInteractionDetailDto`.
- `SymptomDto.cs`: `SymptomCreateUpdateDto`, `SymptomDetailDto`.
- `UserAdminDto.cs`: `UserAdminViewDto`.
- `medicineDiseaseDto.cs`: `DiseaseLinkedWithMedicineDto`, `MedicineLinkedWithDiseaseDto`, `LinkMedicineDiseaseDto`.

DTO user:

- `PredictRequestDto`: dữ liệu yêu cầu dự đoán.
- `PredictResultDto`: dữ liệu trả kết quả dự đoán, gồm cảnh báo và `ResultId`.
- `FeedbackRequestDto`: dữ liệu gửi đánh giá.
- `PythonPredictResponse`: dữ liệu nhận từ Flask.
- `PythonDrugResult`: từng thuốc trong kết quả Flask.

## 9. Data và Models

`Data/ApplicationDbContext.cs` là DbContext chính của hệ thống.

Nhóm entity chính:

- Người dùng: `NguoiDung`.
- Bệnh và triệu chứng: `Benh`, `TrieuChung`, `BenhTrieuChung`.
- Thuốc và thành phần: `Thuoc`, `ThanhPhan`, `BenhThuoc`, `QuyTacGoiYthuoc`.
- Cảnh báo an toàn: `DiUng`, `BenhNen`, `CanhBaoDiUngThuoc`, `CanhBaoBenhNenThuoc`, `TuongTacThuoc`.
- Dự đoán: `LichSuDuDoan`, `ChiTietTrieuChung`, `KetQuaDuDoan`, `DanhGiaDuDoan`.

## 10. AI service Python

### `src/app.py`

Flask API cung cấp các endpoint:

- `POST /embed-drug`: tạo hoặc cập nhật vector thuốc.
- `POST /remove-drug`: xóa vector thuốc.
- `POST /predict`: gợi ý thuốc theo bệnh và triệu chứng.
- `GET /health`: kiểm tra trạng thái service.

### `src/predict.py`

`DrugRecommender` dùng model `intfloat/multilingual-e5-small`.

Trọng số tính điểm:

- Similarity: `0.6`
- Độ ưu tiên điều trị: `0.3`
- Bonus cùng bệnh: `0.1`

Thuốc cần kê đơn đang bị loại khỏi kết quả gợi ý.

### `src/vector_store.py`

`VectorStore` quản lý:

- `drugs_vector.npy`: vector thuốc.
- `drug_metadata.json`: metadata thuốc.

Các thao tác chính gồm load, save, upsert, delete và search.

### `src/seed_from_csv.py`

Script dùng để seed vector thuốc từ dữ liệu CSV và SQL Server, sau đó gọi Flask `POST /embed-drug`.

## 11. Tài liệu nghiệp vụ

Thư mục `docs/` gồm:

- `business_analysis.md`
- `project_scope.md`
- `functional_requirements.md`
- `non_functional_requirements.md`
- `user_stories.md`
- `acceptance_criteria.md`
- `user_survey.md`

## 12. Trạng thái kiểm chứng

Lệnh kiểm tra:

```text
dotnet build
```

## 13. Ghi chú kỹ thuật

- Một số chuỗi tiếng Việt trong source code đang bị mojibake.
- `appsettings.json` đang chứa connection string và JWT key trực tiếp.
- `ApplicationDbContext.OnConfiguring` vẫn còn connection string scaffold.
- `MedicineAdminService` inject `HttpClient` nhưng chưa dùng trong create/update/toggle.
- `ISymptomServeice.cs` và một số tên hàm/biến còn sai chính tả.
- File `UserAdminController.cs` chứa class `UserController`.
- Route API chưa thống nhất giữa `api/[Controller]` và `api/admin/[controller]`.
- Chưa có test tự động trong repo.

## 14. Hướng đọc mã

1. Đọc `Program.cs` để hiểu DI, JWT, middleware và route nền.
2. Đọc `Data/ApplicationDbContext.cs` để hiểu database schema.
3. Đọc các entity chính trong `Models`.
4. Đọc DTO trong `Areas/Admin/DTOs` và `DTOs/UserDto.cs`.
5. Đọc service admin trong `Areas/Admin/Services`.
6. Đọc `Services/UserPredictionService.cs`.
7. Đọc controller trong `Areas/Admin/Controllers` và `Controllers/UserPredictionController.cs`.
8. Đọc `src/app.py`, `src/predict.py`, `src/vector_store.py` khi làm phần AI.
