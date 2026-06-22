# Kiến trúc dự án Project_CNPM

`Project_CNPM` là ứng dụng ASP.NET Core 8 theo mô hình MVC kết hợp API controller. Hệ thống dùng Entity Framework Core làm việc với SQL Server và gọi thêm service Flask để gợi ý thuốc bằng vector search.

## 1. Tổng quan hệ thống

Hệ thống hiện có 3 lớp chính:

- MVC views cho giao diện người dùng và quản trị.
- API controllers cho dữ liệu nghiệp vụ.
- Service layer xử lý nghiệp vụ, truy cập database và gọi Flask khi cần.

Luồng tổng quát:

1. Người dùng thao tác ở Razor view.
2. JavaScript phía client gọi API ASP.NET Core.
3. Controller nhận request, kiểm tra quyền và gọi service.
4. Service xử lý nghiệp vụ, dùng `ApplicationDbContext` để đọc/ghi SQL Server.
5. Riêng phần dự đoán thuốc và mapping thuốc-bệnh có thể gọi thêm Flask service.

## 2. Cấu trúc thư mục chính

```text
Project_CNPM/
├── Areas/
│   └── Admin/
│       ├── Controllers/
│       ├── DTOs/
│       ├── Services/
│       └── Views/
├── Controllers/
├── Data/
├── DTOs/
├── Models/
├── Services/
├── Views/
├── wwwroot/
├── docs/
├── src/
├── Program.cs
├── Project_CNPM.csproj
├── TutorialViews.md
└── ARCHITECTURE.md
```

## 3. MVC controllers hiện có

Controllers phía user/public:

- `HomeController`
- `AccountController`
- `DuDoanController`
- `ThuocController`
- `ErrorController`

Controllers API/user:

- `UserPredictionController`

Controllers admin area:

- `PortalController`
- `AuthController`
- `DashboardController`
- `MedicineController`
- `DiseaseController`
- `SymptomController`
- `SafetyWarningController`
- `MedicineDiseaseMappingController`
- `UserAdminController.cs` chứa class runtime là `UserController`

## 4. MVC routes/view hiện có

Public:

- `/` hoặc `/Home/Index` -> `Views/Home/TrangChu.cshtml`
- `/Account/DangNhap` -> `Views/Account/DangNhap.cshtml`
- `/Account/DangKy` -> `Views/Account/DangKy.cshtml`

User portal:

- `/DuDoan/NhapThongTinBenh` -> `Views/DuDoan/NhapThongTinBenh.cshtml`
- `/DuDoan/KetQuaDuDoan` -> `Views/DuDoan/KetQuaDuDoan.cshtml`
- `/DuDoan/LichSuTraCuu` -> `Views/DuDoan/LichSuTraCuu.cshtml`
- `/DuDoan/TongQuan` -> `Views/DuDoan/TongQuan.cshtml`
- `/Thuoc/ChiTietThuoc/{id?}` -> `Views/Thuoc/ChiTietThuoc.cshtml`
- `/Thuoc/DanhSachThuoc` -> `Views/Thuoc/DanhSachThuoc.cshtml`

Admin portal:

- `/Admin`
- `/Admin/Dashboard`
- `/Admin/QuanLyThuoc`
- `/Admin/QuanLyBenh`
- `/Admin/QuanLyTrieuChung`
- `/Admin/QuanLyNguoiDung`
- `/Admin/QuanLyDiUng`
- `/Admin/ThongKe`

Error pages:

- `/Error/Forbidden`
- `/Error/NotFoundPage`
- `/Error/ServerError`

## 5. Layouts đang dùng

User portal:

- `Views/Shared/_UserPortalLayout.cshtml`

Admin portal:

- `Areas/Admin/Views/Shared/_Layout.cshtml`

Điểm cần lưu ý:

- Public page và auth page dùng CSS riêng trong `wwwroot/css/shared` và `wwwroot/css/account`.
- User portal và admin portal đều dùng sidebar riêng, không còn phụ thuộc layout mặc định cũ của template ASP.NET.

## 6. Authentication và phân quyền

JWT được tạo ở `AuthService` khi đăng nhập thành công. Token chứa các claim chính:

- `ClaimTypes.NameIdentifier`
- `ClaimTypes.Email`
- `ClaimTypes.Name`
- `ClaimTypes.Role`

Phân quyền đang áp dụng:

- `AuthController`: không yêu cầu token cho login/register.
- `UserPredictionController`: yêu cầu người dùng đã đăng nhập.
- `PortalController`: yêu cầu role `Admin`.
- Phần lớn API admin yêu cầu `Admin`.
- `DiseaseController` hiện cho phép người dùng đã đăng nhập đọc danh sách bệnh để phục vụ form dự đoán.

Frontend đang dùng JWT từ `localStorage` để gọi API.

## 7. API controllers hiện có

### `api/Auth`

- `POST /api/Auth/login`
- `POST /api/Auth/register`
- `POST /api/Auth/logout?userId={id}`

### `api/UserPrediction`

- `POST /api/UserPrediction/predict`
- `POST /api/UserPrediction/feedback`
- `GET /api/UserPrediction/history`
- `DELETE /api/UserPrediction/history/{historyId}`

### `api/admin/Dashboard`

- `GET /api/admin/Dashboard/statistics`

### `api/Medicine`

- `GET /api/Medicine`
- `GET /api/Medicine/{id}`
- `POST /api/Medicine/create`
- `PATCH /api/Medicine/update`
- `PATCH /api/Medicine/toggle?id={id}&isActive={true|false}`

### `api/Disease`

- `GET /api/Disease`
- `GET /api/Disease/{id}`
- `POST /api/Disease/create`
- `PATCH /api/Disease/Update`
- `PATCH /api/Disease/toggle?id={id}`

### `api/Symptom`

- `GET /api/Symptom`
- `GET /api/Symptom/{id}`
- `POST /api/Symptom/create`
- `PATCH /api/Symptom/update`
- `PATCH /api/Symptom/toggle?id={id}`

### `api/SafetyWarning`

- `GET /api/SafetyWarning/allergies`
- `POST /api/SafetyWarning/allergies/create`
- `PATCH /api/SafetyWarning/allergies/update`
- `PATCH /api/SafetyWarning/allergies/toggle?id={id}&isActive={true|false}`
- `GET /api/SafetyWarning/background-diseases`
- `POST /api/SafetyWarning/background-diseases/create`
- `PATCH /api/SafetyWarning/background-diseases/update`
- `PATCH /api/SafetyWarning/background-diseases/toggle?id={id}&isActive={true|false}`
- `GET /api/SafetyWarning/drug-interactions`
- `POST /api/SafetyWarning/drug-interactions/create`
- `DELETE /api/SafetyWarning/drug-interactions/{medicine1Id}/{medicine2Id}`

### `api/admin/MedicineDiseaseMapping`

- `GET /api/admin/MedicineDiseaseMapping/medicine/{medicineId}/diseases`
- `GET /api/admin/MedicineDiseaseMapping/disease/{diseaseId}/medicines`
- `POST /api/admin/MedicineDiseaseMapping/link`
- `DELETE /api/admin/MedicineDiseaseMapping/unlink/medicine/{medicineId}/disease/{diseaseId}`

### `api/User`

File controller nằm ở `Areas/Admin/Controllers/UserAdminController.cs`, nhưng class runtime là `UserController`, nên route thật là:

- `GET /api/User`
- `PATCH /api/User/{userId}/lock?isLocked={true|false}`
- `DELETE /api/User/{userId}`

## 8. Service layer

Admin services:

- `AuthService`
- `DashboardAdminService`
- `MedicineAdminService`
- `DiseaseAdminService`
- `SymptomService`
- `SafetyAdminService`
- `MedicineDiseaseMappingService`
- `UserAdminService`

User services:

- `UserPredictionService`

Trách nhiệm chính:

- Xử lý login/register/JWT.
- CRUD danh mục admin.
- Đồng bộ mapping thuốc-bệnh với Flask vector store.
- Gọi Flask để dự đoán thuốc cho user.
- Lưu lịch sử dự đoán và feedback.

## 9. Luồng dự đoán thuốc hiện tại

1. User đăng nhập.
2. Frontend mở `GET /DuDoan/NhapThongTinBenh`.
3. Trang gọi `GET /api/Disease` để nạp danh sách bệnh.
4. User nhập bệnh, triệu chứng và thông tin sức khỏe liên quan.
5. Frontend ghép thêm tuổi, giới tính, mức độ triệu chứng, dị ứng, bệnh nền, thuốc đang dùng vào chuỗi `symptoms`.
6. Frontend gọi `POST /api/UserPrediction/predict`.
7. Backend gọi `UserPredictionService`, từ đó gọi Flask `/predict`.
8. Backend trả danh sách thuốc gợi ý, cảnh báo và `resultId`.
9. Frontend lưu tạm dữ liệu ở `sessionStorage` rồi chuyển sang `GET /DuDoan/KetQuaDuDoan`.
10. User có thể gửi feedback qua `POST /api/UserPrediction/feedback`.
11. User có thể xem lịch sử qua `GET /api/UserPrediction/history` và xóa qua `DELETE /api/UserPrediction/history/{historyId}`.

## 10. Database và models

`Data/ApplicationDbContext.cs` là DbContext chính.

Nhóm entity quan trọng:

- Người dùng: `NguoiDung`
- Bệnh và triệu chứng: `Benh`, `TrieuChung`, `BenhTrieuChung`
- Thuốc và thành phần: `Thuoc`, `ThanhPhan`, `BenhThuoc`, `QuyTacGoiYthuoc`
- Cảnh báo an toàn: `DiUng`, `BenhNen`, `CanhBaoDiUngThuoc`, `CanhBaoBenhNenThuoc`, `TuongTacThuoc`
- Dự đoán: `LichSuDuDoan`, `ChiTietTrieuChung`, `KetQuaDuDoan`, `DanhGiaDuDoan`

## 11. Flask AI service

File chính:

- `src/app.py`
- `src/predict.py`
- `src/vector_store.py`

Endpoint Flask đang được backend dùng:

- `POST /embed-drug`
- `POST /remove-drug`
- `POST /predict`
- `GET /health`

## 12. Những điểm đã loại khỏi tài liệu

Các mục sau không còn được xem là luồng chuẩn hiện tại nên không mô tả dài ở đây:

- `Views/Home/Privacy.cshtml`
- layout mặc định cũ của template ASP.NET
- các file HTML/CSS/JS tách rời đời đầu ngoài `Views/` và `wwwroot/`

## 13. Kiểm chứng

Lệnh build gần nhất:

```text
dotnet msbuild /t:Compile /p:UseAppHost=false
```

Trạng thái:

- Build qua.
- Vẫn còn một số warning nullability và cảnh báo connection string scaffold trong `ApplicationDbContext.cs`.
