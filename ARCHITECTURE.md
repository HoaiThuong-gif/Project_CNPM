# Kiến trúc dự án Project_CNPM

Cập nhật: 20/06/2026

Tài liệu này mô tả trạng thái hiện tại của mã nguồn để các thành viên nhanh chóng nắm được ứng dụng đang tổ chức ra sao, phần admin đã làm đến đâu, lớp gợi ý thuốc bằng AI đang nằm ở đâu và nên đọc file nào khi tiếp tục phát triển.

## 1. Tổng quan

`Project_CNPM` là ứng dụng ASP.NET Core 8 theo mô hình MVC kết hợp API controller. Dữ liệu nghiệp vụ được truy cập bằng Entity Framework Core và SQL Server database `WebsiteDuDoAnThuoc`/`WebsiteDuDoanThuoc` tùy cấu hình môi trường.

Ngoài backend C#, dự án hiện có thêm một service Python Flask trong thư mục `src/` để tạo embedding thuốc và gợi ý thuốc bằng vector search. Service này dùng model `intfloat/multilingual-e5-small`, lưu vector vào `drugs_vector.npy` và metadata vào `drug_metadata.json`.

Luồng xử lý chính của phần ASP.NET Core:

1. Client gửi request tới controller.
2. Controller nhận DTO hoặc query parameter, kiểm tra cơ bản và gọi service.
3. Service xử lý nghiệp vụ, truy vấn/cập nhật dữ liệu qua `ApplicationDbContext`.
4. `ApplicationDbContext` ánh xạ bảng SQL Server sang entity trong `Models`.
5. Controller trả JSON cho API hoặc Razor view cho MVC.

Luồng xử lý AI hiện có:

1. API admin liên kết thuốc với bệnh qua `MedicineDiseaseMappingService`.
2. Service lưu mapping vào bảng `BenhThuoc`.
3. Service gọi Flask API `POST /embed-drug` để đồng bộ vector thuốc.
4. Flask service lưu/cập nhật vector trong local vector store.
5. Khi cần gợi ý, Flask API `POST /predict` mã hóa bệnh + triệu chứng và trả danh sách thuốc phù hợp theo điểm tổng hợp.

## 2. Cấu trúc thư mục hiện tại

```text
Project_CNPM/
+-- Areas/
|   +-- Admin/
|       +-- Controllers/
|       |   +-- AuthController.cs
|       |   +-- DashboardController.cs
|       |   +-- DiseaseController.cs
|       |   +-- MedicineController.cs
|       |   +-- SafetyWarningController.cs
|       |   +-- SymptomController.cs
|       |   +-- UserController.cs
|       +-- DTOs/
|       |   +-- AuthDto.cs
|       |   +-- DashboardDto.cs
|       |   +-- DiseaseDto.cs
|       |   +-- MedicineDto.cs
|       |   +-- SafetyWarningDto.cs
|       |   +-- SymptomDto.cs
|       |   +-- UserAdminDto.cs
|       |   +-- medicineDiseaseDto.cs
|       +-- Services/
|           +-- AuthService.cs
|           +-- DashboardService.cs
|           +-- DiseaseAdminService.cs
|           +-- MedicineAdminService.cs
|           +-- MedicineDiseaseMappingService.cs
|           +-- SafetyAdminService.cs
|           +-- SymptomService.cs
|           +-- UserAdminService.cs
|           +-- IAuthService.cs
|           +-- IDashboardService.cs
|           +-- IDiseaseAdminService.cs
|           +-- IMedicineAdminService.cs
|           +-- IMedicineDiseaseMappingService.cs
|           +-- ISafetyAdminService.cs
|           +-- ISymptomServeice.cs
|           +-- IUserAdminService.cs
+-- Controllers/
|   +-- HomeController.cs
+-- Data/
|   +-- ApplicationDbContext.cs
|   +-- benh_trieu_chung.csv
|   +-- thuoc.csv
+-- Models/
|   +-- Benh.cs
|   +-- BenhNen.cs
|   +-- BenhThuoc.cs
|   +-- BenhTrieuChung.cs
|   +-- CanhBaoBenhNenThuoc.cs
|   +-- CanhBaoDiUngThuoc.cs
|   +-- ChiTietTrieuChung.cs
|   +-- DanhGiaDuDoan.cs
|   +-- DiUng.cs
|   +-- KetQuaDuDoan.cs
|   +-- LichSuDuDoan.cs
|   +-- NguoiDung.cs
|   +-- QuyTacGoiYthuoc.cs
|   +-- ThanhPhan.cs
|   +-- Thuoc.cs
|   +-- TrieuChung.cs
|   +-- TuongTacThuoc.cs
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
+-- drugs_vector.npy
+-- drug_metadata.json
```

## 3. Công nghệ và thư viện

Phần C#:

- ASP.NET Core MVC/API trên `.NET 8`.
- Entity Framework Core 8 với SQL Server.
- `BCrypt.Net-Next` để hash và verify mật khẩu.
- `HttpClient` để gọi service AI nội bộ ở `http://localhost:5000`.

Phần Python:

- Flask cho API AI.
- `sentence-transformers` với model `intfloat/multilingual-e5-small`.
- NumPy để lưu và tính similarity vector.
- `requests` và `pyodbc` trong script seed dữ liệu.

Hạ tầng local:

- `docker-compose.yml` khởi tạo SQL Server 2019, expose port `1433`.
- `SQL.sql` chứa schema/dữ liệu khởi tạo.

## 4. Program.cs và Dependency Injection

`Program.cs` hiện đăng ký:

- `ApplicationDbContext` với SQL Server connection string `DefaultConnection`.
- `IAuthService -> AuthService`
- `IMedicineAdminService -> MedicineAdminService`
- `IDiseaseAdminService -> DiseaseAdminService`
- `ISymptomAdminService -> SymptomAdminService`
- `IUserAdminService -> UserAdminService`
- `ISafetyWarningAdminService -> SafetyWarningAdminService`
- `IDashboardAdminService -> DashboardAdminService`
- `IMedicineDiseaseMappingService -> MedicineDiseaseMappingService` qua `AddHttpClient`

Pipeline hiện dùng `AddControllersWithViews`, `UseHttpsRedirection`, `UseStaticFiles`, `UseRouting`, `UseAuthorization` và route MVC mặc định `{controller=Home}/{action=Index}/{id?}`.

## 5. Controller hiện có

Tất cả controller admin nằm trong namespace `Project_CNPM.Area.Admin.Controllers`.

### AuthController

Route base: `api/Auth`

- `POST /api/Auth/login`
- `POST /api/Auth/register`
- `POST /api/Auth/logout`

Gọi `IAuthService` để đăng nhập, đăng ký và logout. Login kiểm tra email, mật khẩu đã hash và trạng thái khóa tài khoản.

### DashboardController

Route base: `api/admin/Dashboard`

- `GET /api/admin/Dashboard/statistics`

Gọi `IDashboardAdminService.GetSystemStatisticsAsync`, trả tổng số user đang hoạt động, thuốc đang hoạt động, bệnh đang hoạt động và lượt dự đoán.

### DiseaseController

Route base: `api/Disease`

- `GET /api/Disease`
- `GET /api/Disease/id?id={id}`
- `POST /api/Disease/create`
- `PATCH /api/Disease/Update`
- `PATCH /api/Disease/delete?id={id}`

Gọi `IDiseaseAdminService`. Service trả `(IsSuccess, Message)` cho create/update/delete.

### SymptomController

Route base: `api/Symptom`

- `GET /api/Symptom`
- `GET /api/Symptom/id?id={id}`
- `POST /api/Symptom/create`
- `PATCH /api/Symptom/update`
- `PATCH /api/Symptom/delete?id={id}`

Gọi `ISymptomAdminService`. Interface và file hiện còn typo `ISymptomServeice.cs`, method soft delete hiện tên `SolfDeleteAsync`.

### MedicineController

Route base: `api/Medicine`

- `GET /api/Medicine`
- `GET /api/Medicine/id?id={id}`
- `POST /api/Medicine/create`
- `PATCH /api/Medicine/update`
- `PATCH /api/Medicine/status?id={id}&isActive={true|false}`

Gọi `IMedicineAdminService`. Controller hiện quản lý CRUD/trạng thái thuốc, chưa expose trực tiếp endpoint liên kết thuốc-bệnh.

### SafetyWarningController

Route base: `api/SafetyWarning`

Dị ứng:

- `GET /api/SafetyWarning/allergies`
- `POST /api/SafetyWarning/allergies/create`
- `PATCH /api/SafetyWarning/allergies/update`
- `PATCH /api/SafetyWarning/allergies/status?id={id}&isActive={true|false}`

Bệnh nền:

- `GET /api/SafetyWarning/background-diseases`
- `POST /api/SafetyWarning/background-diseases/create`
- `PATCH /api/SafetyWarning/background-diseases/update`
- `PATCH /api/SafetyWarning/background-diseases/status?id={id}&isActive={true|false}`

Tương tác thuốc:

- `GET /api/SafetyWarning/drug-interactions`
- `POST /api/SafetyWarning/drug-interactions/create`
- `DELETE /api/SafetyWarning/drug-interactions/delete?medicine1Id={id1}&medicine2Id={id2}`

Gọi `ISafetyWarningAdminService`. Service chuẩn hóa cặp thuốc theo thứ tự `min/max` trước khi tạo hoặc xóa tương tác để tránh trùng cặp đảo chiều.

### UserController

Route base: `api/User`

- `GET /api/User?includeDeleted={true|false}`
- `PATCH /api/User/lock?userId={id}&isLocked={true|false}`
- `PATCH /api/User/delete?userId={id}`

Gọi `IUserAdminService`. Hỗ trợ lấy danh sách user, khóa/mở khóa tài khoản và xóa mềm bằng `DeleteAt`.

### HomeController

Controller MVC mặc định cho Razor view:

- `GET /Home/Index`
- `GET /Home/Privacy`
- `GET /Home/Error`

## 6. Service hiện có

### AuthService

Xử lý đăng nhập, đăng ký, logout. Mật khẩu được hash bằng `BCrypt.Net`; khi login, service tìm user theo email và verify password hash.

### DashboardAdminService

Tổng hợp:

- `TotalActiveUsers`
- `TotalActiveMedicines`
- `TotalActiveDiseases`
- `TotalPredictionsMade`

### DiseaseAdminService

Quản trị bệnh:

- Lấy danh sách bệnh.
- Lấy chi tiết theo id.
- Tạo bệnh, kiểm tra trùng tên.
- Cập nhật bệnh.
- Xóa mềm bằng `DeleteAt` và `DangHoatDong = false`.

### SymptomAdminService

Quản trị triệu chứng:

- Lấy danh sách.
- Lấy chi tiết.
- Tạo mới, kiểm tra trùng tên.
- Cập nhật.
- Tắt trạng thái hoạt động.

Ghi chú kỹ thuật: `SolfDeleteAsync` hiện cần được đổi tên thành `SoftDeleteAsync` để đồng bộ chính tả.

### MedicineAdminService

Quản trị thuốc:

- Lấy danh sách thuốc, có tùy chọn lọc thuốc đang hoạt động.
- Lấy chi tiết theo id.
- Tạo thuốc, kiểm tra trùng tên.
- Cập nhật thuốc và `NgayCapNhat`.
- Bật/tắt trạng thái thuốc.

Service hiện có inject `HttpClient` và đặt `BaseAddress = http://localhost:5000`, nhưng các luồng create/update/status chưa gọi Flask trực tiếp. Phần đồng bộ AI đang nằm ở `MedicineDiseaseMappingService`.

### MedicineDiseaseMappingService

Quản lý mapping thuốc-bệnh trong bảng `BenhThuoc`:

- `LinkMedicineWithDiseaseAsync`: kiểm tra thuốc/bệnh tồn tại, tránh trùng mapping, thêm record `BenhThuoc`, sau đó gọi Flask `POST /embed-drug`.
- `UnlinkMedicineFromDiseaseAsync`: xóa mapping, gọi Flask `POST /remove-drug`, rồi sync lại các mapping còn lại của thuốc.
- `GetDiseasesOfMedicineAsync`: lấy danh sách bệnh đã gắn với một thuốc.
- `GetMedicinesOfDiseaseAsync`: lấy danh sách thuốc đã gắn với một bệnh.

Trạng thái hiện tại: service và DTO đã có, đã đăng ký DI qua `AddHttpClient`, nhưng chưa có controller endpoint tương ứng trong `Areas/Admin/Controllers`.

### SafetyWarningAdminService

Quản trị dữ liệu an toàn:

- Dị ứng: lấy danh sách, tạo, cập nhật, bật/tắt trạng thái.
- Bệnh nền: lấy danh sách, tạo, cập nhật, bật/tắt trạng thái.
- Tương tác thuốc: lấy danh sách, tạo, xóa.

Phần tương tác thuốc dùng navigation property `MaThuoc1Navigation` và `MaThuoc2Navigation` để lấy tên thuốc.

### UserAdminService

Quản trị người dùng:

- Lấy danh sách user, có tùy chọn bao gồm user đã xóa mềm.
- Khóa/mở khóa user bằng `BiKhoa`.
- Xóa mềm user bằng `DeleteAt` và khóa luôn tài khoản.

## 7. DTO chính

- `AuthDto.cs`: `loginDto`, `registerDto`.
- `DashboardDto.cs`: `SystemStatisticsDto`.
- `DiseaseDto.cs`: `DiseaseCreateDto`, `DiseaseUpdateDto`, `DiseaseDetailDto`.
- `MedicineDto.cs`: `MedicineCreateDto`, `MedicineUpdateDto`, `MedicineDetailDto`.
- `SafetyWarningDto.cs`: `AllergyCreateUpdateDto`, `BackgroundDiseaseCreateUpdateDto`, `DrugInteractionCreateDto`, `DrugInteractionDetailDto`.
- `SymptomDto.cs`: `SymptomCreateUpdateDto`, `SymptomDetailDto`.
- `UserAdminDto.cs`: `UserAdminViewDto`.
- `medicineDiseaseDto.cs`: `DiseaseLinkedWithMedicineDto`, `MedicineLinkedWithDiseaseDto`.

## 8. Data và Models

`Data/ApplicationDbContext.cs` là DbContext chính được đăng ký trong `Program.cs`. File này vẫn còn connection string scaffold trong `OnConfiguring`, dù ứng dụng cũng đã đọc `DefaultConnection` từ cấu hình.

Các nhóm entity quan trọng:

- Người dùng: `NguoiDung`.
- Bệnh và triệu chứng: `Benh`, `TrieuChung`, `BenhTrieuChung`.
- Thuốc và thành phần: `Thuoc`, `ThanhPhan`, `BenhThuoc`, `QuyTacGoiYthuoc`.
- Cảnh báo an toàn: `DiUng`, `BenhNen`, `CanhBaoDiUngThuoc`, `CanhBaoBenhNenThuoc`, `TuongTacThuoc`.
- Lịch sử dự đoán: `LichSuDuDoan`, `ChiTietTrieuChung`, `KetQuaDuDoan`, `DanhGiaDuDoan`.

## 9. Service AI Python

### Flask API trong `src/app.py`

- `POST /embed-drug`: nhận thông tin thuốc, tạo embedding và upsert vào vector store.
- `POST /remove-drug`: xóa vector thuốc theo `ma_thuoc`.
- `POST /predict`: nhận `ten_benh`, `trieu_chung`, `ma_benh`, `top_k`, trả danh sách thuốc gợi ý.
- `GET /health`: kiểm tra service còn sống và số thuốc trong vector store.

### Logic gợi ý trong `src/predict.py`

`DrugRecommender` dùng:

- `MODEL_NAME = intfloat/multilingual-e5-small`
- `W_SIMILARITY = 0.6`
- `W_DO_UU_TIEN = 0.3`
- `W_TRONG_SO_TRIEU_CHUNG = 0.1`

Điểm cuối cùng kết hợp similarity, độ ưu tiên điều trị và bonus cùng bệnh. Thuốc cần kê đơn (`can_ke_don != 0`) đang bị loại khỏi kết quả gợi ý.

### Vector store trong `src/vector_store.py`

`VectorStore` lưu:

- Vector: `drugs_vector.npy`
- Metadata: `drug_metadata.json`

Các thao tác chính: load, save, upsert, delete, search theo dot product similarity.

### Seed dữ liệu trong `src/seed_from_csv.py`

Script đọc `Data/thuoc.csv`, tra `MaBenh` và `MaThuoc` thật từ SQL Server bằng `pyodbc`, rồi gọi `POST /embed-drug` để tạo vector ban đầu.

## 10. Tài liệu nghiệp vụ

Thư mục `docs/` hiện có:

- `business_analysis.md`
- `project_scope.md`
- `functional_requirements.md`
- `non_functional_requirements.md`
- `user_stories.md`
- `acceptance_criteria.md`
- `user_survey.md`

Các file này nên được dùng làm nguồn tham chiếu nghiệp vụ khi thêm tính năng mới hoặc kiểm tra tiêu chí nghiệm thu.

## 11. Trạng thái build và kiểm chứng

Lệnh đã kiểm tra:

```text
dotnet build
```

Kết quả ngày 20/06/2026: build chưa thành công.

Warning đáng chú ý:

- Một số file có tiếng Việt bị mojibake do encoding cũ, đặc biệt trong message/comment C# và Python.
- `MedicineAdminService` inject `HttpClient` nhưng chưa dùng trong create/update/status.
- Một số nullable warning ở `AuthController`, `UserAdminService`, `DiseaseAdminService`, `SymptomService`.
- `AuthService.LogoutAsync` là async method nhưng chưa có `await`.
- Chưa thấy test tự động trong repo.

## 12. Việc nên làm tiếp
- Chuẩn hóa route style: hiện đa số controller dùng `api/[Controller]`, riêng `DashboardController` dùng `api/admin/[controller]`.
- Sửa chính tả `ISymptomServeice.cs` và `SolfDeleteAsync`.
- Chuẩn hóa UTF-8 cho các file đang bị mojibake tiếng Việt.
- Di chuyển connection string nhạy cảm khỏi `ApplicationDbContext.OnConfiguring`, chỉ dùng `appsettings`, environment variable hoặc secret store.
- Cân nhắc đổi route lấy id từ `GET /id?id=...` sang `GET /{id}`.
- Bổ sung test cho service quan trọng: auth, medicine, disease, symptom, safety warning và mapping thuốc-bệnh.
- Bổ sung health/check hoặc config rõ ràng cho Flask service để backend C# không phụ thuộc hard-code `http://localhost:5000`.

## 13. Hướng đọc mã cho dev mới

1. Đọc `Program.cs` để hiểu DI, middleware và route nền.
2. Đọc `Data/ApplicationDbContext.cs` để hiểu schema và quan hệ database.
3. Đọc entity trong `Models`, tập trung vào `NguoiDung`, `Benh`, `TrieuChung`, `Thuoc`, `BenhThuoc`, `DiUng`, `BenhNen`, `TuongTacThuoc`.
4. Đọc DTO trong `Areas/Admin/DTOs` để hiểu dữ liệu vào/ra.
5. Đọc service trong `Areas/Admin/Services` để nắm logic nghiệp vụ.
6. Đọc controller trong `Areas/Admin/Controllers` để biết endpoint client đang gọi.
7. Đọc `src/app.py`, `src/predict.py`, `src/vector_store.py` nếu làm phần AI/gợi ý thuốc.
