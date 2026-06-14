# Kiến trúc dự án Project_CNPM

Cập nhật: 14/06/2026

Tài liệu này mô tả trạng thái hiện tại của mã nguồn để các thành viên khác nhanh chóng nắm được ứng dụng đang tổ chức ra sao, phần admin đã làm đến đâu và nên đọc file nào khi tiếp tục phát triển.

## 1. Tổng quan

`Project_CNPM` là ứng dụng ASP.NET Core 8, dùng MVC kết hợp API controller. Dữ liệu được truy cập qua Entity Framework Core và SQL Server database `WebsiteDuDoanThuoc`.

Luồng xử lý chính:

1. Client gửi request tới controller.
2. Controller nhận DTO hoặc query parameter, kiểm tra cơ bản và gọi service.
3. Service xử lý nghiệp vụ, truy vấn/cập nhật dữ liệu qua `ApplicationDbContext`.
4. `ApplicationDbContext` ánh xạ bảng SQL Server sang entity trong `Models`.
5. Controller trả JSON cho API hoặc Razor view cho MVC.

Hiện các nghiệp vụ admin chính đã có controller và service: xác thực, dashboard, bệnh, triệu chứng, thuốc, cảnh báo an toàn và người dùng.

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
|       +-- Services/
|           +-- AuthService.cs
|           +-- DashboardService.cs
|           +-- DiseaseAdminService.cs
|           +-- IAuthService.cs
|           +-- IDashboardService.cs
|           +-- IDiseaseAdminService.cs
|           +-- IMedicineAdminService.cs
|           +-- ISafetyAdminService.cs
|           +-- ISymptomServeice.cs
|           +-- IUserAdminService.cs
|           +-- MedicineAdminService.cs
|           +-- SafetyAdminService.cs
|           +-- SymptomService.cs
|           +-- UserAdminService.cs
+-- Data/
|   +-- ApplicationDbContext.cs
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
+-- Program.cs
+-- Project_CNPM.csproj
+-- SQL.sql
```

Ghi chú: Git status hiện vẫn còn một số file cũ ở root `Controllers/`, `DTOs/`, `Services/` bị đánh dấu xóa. Hướng phát triển hiện tại là dùng cấu trúc mới trong `Areas/Admin`.

## 3. Program.cs và Dependency Injection

`Program.cs` đang đăng ký:

- `ApplicationDbContext` với SQL Server connection string `DefaultConnection`.
- `IAuthService -> AuthService`
- `IMedicineAdminService -> MedicineAdminService`
- `IDiseaseAdminService -> DiseaseAdminService`
- `ISymptomAdminService -> SymptomAdminService`
- `IUserAdminService -> UserAdminService`
- `ISafetyWarningAdminService -> SafetyWarningAdminService`
- `IDashboardAdminService -> DashboardAdminService`

Ứng dụng dùng `AddControllersWithViews`, `UseHttpsRedirection`, `UseStaticFiles`, `UseRouting` và `UseAuthorization`.

## 4. Controller hiện có

Tất cả controller admin nằm trong namespace `Project_CNPM.Area.Admin.Controllers`.

### AuthController

Route base: `api/Auth`

- `POST /api/Auth/login`
- `POST /api/Auth/register`
- `POST /api/Auth/logout`

Gọi `IAuthService` để đăng nhập, đăng ký và logout. Message tiếng Việt trong file hiện đang bị lỗi encoding, cần chuẩn hóa UTF-8 sau.

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

Gọi `ISymptomAdminService`. Lưu ý method trong interface đang tên `SolfDeleteAsync`, nên sau này nên đổi thành `SoftDeleteAsync` cho đúng chính tả.

### MedicineController

Route base: `api/Medicine`

- `GET /api/Medicine`
- `GET /api/Medicine/id?id={id}`
- `POST /api/Medicine/create`
- `PATCH /api/Medicine/update`
- `PATCH /api/Medicine/status?id={id}&isActive={true|false}`

Gọi `IMedicineAdminService`. Service hiện đã được chỉnh để trả `(IsSuccess, Message)` giống mẫu Disease.

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

## 5. Service hiện có

### AuthService

Xử lý đăng nhập, đăng ký, hash mật khẩu bằng `BCrypt.Net`. Khi login, service tìm user theo email và verify password hash.

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

Ghi chú kỹ thuật: `SolfDeleteAsync` hiện thiếu `SaveChangesAsync`, cần bổ sung ở lượt dọn lỗi tiếp theo.

### MedicineAdminService

Quản trị thuốc:

- Lấy danh sách, có tùy chọn lọc thuốc đang hoạt động.
- Lấy chi tiết theo id.
- Tạo thuốc, kiểm tra trùng tên.
- Cập nhật thuốc và `NgayCapNhat`.
- Bật/tắt trạng thái thuốc.

Các field nullable từ entity `Thuoc` được map về chuỗi rỗng trong DTO để giảm warning/null khi trả API.

### SafetyWarningAdminService

Quản trị dữ liệu an toàn:

- Dị ứng: lấy danh sách, tạo, cập nhật, bật/tắt trạng thái.
- Bệnh nền: lấy danh sách, tạo, cập nhật, bật/tắt trạng thái.
- Tương tác thuốc: lấy danh sách, tạo, xóa.

Phần tương tác thuốc đã đổi sang dùng navigation property `MaThuoc1Navigation` và `MaThuoc2Navigation` để lấy tên thuốc, tránh truy vấn `FirstOrDefault(...).TenThuoc` có nguy cơ null.

### UserAdminService

Quản trị người dùng:

- Lấy danh sách user, có tùy chọn bao gồm user đã xóa mềm.
- Khóa/mở khóa user bằng `BiKhoa`.
- Xóa mềm user bằng `DeleteAt` và khóa luôn tài khoản.

Ghi chú: `UserAdminDto.cs` hiện đang ở thư mục `Areas/Admin/DTOs` nhưng namespace là `Project_CNPM.DTOs.Medicine`. Service/interface đang import namespace này để build được; nên đổi namespace về `Project_CNPM.Area.Admin.DTOs` khi có thời gian dọn đồng bộ.

## 6. DTO chính

- `AuthDto.cs`: `loginDto`, `registerDto`.
- `DashboardDto.cs`: `SystemStatisticsDto`.
- `DiseaseDto.cs`: `DiseaseCreateDto`, `DiseaseUpdateDto`, `DiseaseDetailDto`.
- `MedicineDto.cs`: `MedicineCreateDto`, `MedicineUpdateDto`, `MedicineDetailDto`.
- `SafetyWarningDto.cs`: `AllergyCreateUpdateDto`, `BackgroundDiseaseCreateUpdateDto`, `DrugInteractionCreateDto`, `DrugInteractionDetailDto`.
- `SymptomDto.cs`: `SymptomCreateUpdateDto`, `SymptomDetailDto`.
- `UserAdminDto.cs`: `UserAdminViewDto`.

## 7. Data và Models

`Data/ApplicationDbContext.cs` là DbContext chính đang được đăng ký trong `Program.cs`.

Các nhóm entity quan trọng:

- Người dùng: `NguoiDung`.
- Bệnh và triệu chứng: `Benh`, `TrieuChung`, `BenhTrieuChung`.
- Thuốc và thành phần: `Thuoc`, `ThanhPhan`, `BenhThuoc`, `QuyTacGoiYthuoc`.
- Cảnh báo an toàn: `DiUng`, `BenhNen`, `CanhBaoDiUngThuoc`, `CanhBaoBenhNenThuoc`, `TuongTacThuoc`.
- Lịch sử dự đoán: `LichSuDuDoan`, `ChiTietTrieuChung`, `KetQuaDuDoan`, `DanhGiaDuDoan`.

## 8. Trạng thái build

Lệnh đã kiểm tra:

```text
dotnet build
```

Kết quả hiện tại: build thành công, 0 error.

Các warning còn tồn tại:

- Connection string vẫn xuất hiện trong `ApplicationDbContext.cs`.
- Một số nullable warning ở `AuthController`, `UserAdminService`, `SymptomService`, `DiseaseAdminService`.
- `AuthService.LogoutAsync` là async method nhưng chưa có `await`.
- Một số message/comment tiếng Việt trong code bị lỗi encoding.

## 9. Việc nên làm tiếp

- Chuẩn hóa route style: hiện đa số controller dùng `api/[Controller]`, riêng `DashboardController` dùng `api/admin/[controller]`.
- Sửa lỗi chính tả `ISymptomServeice.cs` và `SolfDeleteAsync`.
- Bổ sung `SaveChangesAsync` trong `SymptomAdminService.SolfDeleteAsync`.
- Đổi namespace `UserAdminDto.cs` về `Project_CNPM.Area.Admin.DTOs` và cập nhật import tương ứng.
- Chuẩn hóa UTF-8 cho các file đang bị mojibake tiếng Việt.
- Cân nhắc chuyển các route lấy id từ `GET /id?id=...` sang dạng rõ hơn như `GET /{id}`.
- Di chuyển connection string nhạy cảm khỏi DbContext scaffold, dùng `appsettings`, environment variable hoặc secret store.

## 10. Hướng đọc mã cho dev mới

1. Đọc `Program.cs` để hiểu DI, middleware và route nền.
2. Đọc `Data/ApplicationDbContext.cs` để hiểu schema và quan hệ database.
3. Đọc entity trong `Models`, tập trung vào `NguoiDung`, `Benh`, `TrieuChung`, `Thuoc`, `DiUng`, `BenhNen`, `TuongTacThuoc`.
4. Đọc DTO trong `Areas/Admin/DTOs` để hiểu dữ liệu vào/ra.
5. Đọc service trong `Areas/Admin/Services` để nắm logic nghiệp vụ.
6. Đọc controller trong `Areas/Admin/Controllers` để biết endpoint client đang gọi.
