# Tutorial Views

Tài liệu này mô tả luồng MVC/view đang dùng thực tế trong repo hiện tại. Chỉ ghi các màn hình và API đã có trong code, tránh giả định thêm route hoặc endpoint mới.

## 1. Authentication

### V-01: Đăng nhập

View:

- `GET /Account/DangNhap`
- File view: `Views/Account/DangNhap.cshtml`

API gọi từ frontend:

- `POST /api/Auth/login`

Kết quả:

- Lưu JWT ở `localStorage`.
- Lưu `username`, `role`, `userId` để dùng lại ở layout/frontend.
- Nếu role là `Admin`, chuyển sang `GET /Admin/Dashboard`.
- Nếu role là `User`, chuyển sang `GET /DuDoan/NhapThongTinBenh`.
- Nếu có `returnUrl` hợp lệ thì ưu tiên quay lại đúng trang trước đó.

### V-02: Đăng ký

View:

- `GET /Account/DangKy`
- File view: `Views/Account/DangKy.cshtml`

API gọi từ frontend:

- `POST /api/Auth/register`

Kết quả:

- Hiển thị thông báo thành công hoặc lỗi từ backend.
- Không tự thay đổi logic xác thực phía server.

## 2. Phân hệ User

Layout chung:

- `Views/Shared/_UserPortalLayout.cshtml`

Sidebar user hiện có 3 lối vào:

- `GET /DuDoan/NhapThongTinBenh`
- `GET /DuDoan/LichSuTraCuu`
- `GET /DuDoan/TongQuan`

### V-03: Nhập thông tin dự đoán

View:

- `GET /DuDoan/NhapThongTinBenh`
- File view: `Views/DuDoan/NhapThongTinBenh.cshtml`

API dùng thật:

- `GET /api/Disease`
- `POST /api/UserPrediction/predict`

Thông tin người dùng có thể nhập ở frontend hiện tại:

- Bệnh cần dự đoán.
- Tuổi.
- Giới tính.
- Mức độ triệu chứng.
- Bệnh nền.
- Thuốc đang sử dụng.
- Dị ứng thuốc.
- Triệu chứng tự do.

Ghi chú triển khai:

- Backend hiện vẫn nhận DTO cũ gồm `diseaseId` và `symptoms`.
- Frontend sẽ ghép các thông tin bổ sung thành một chuỗi `symptoms` giàu ngữ cảnh trước khi gọi API.
- Không có thay đổi schema request ở backend.

### V-04: Kết quả dự đoán

View:

- `GET /DuDoan/KetQuaDuDoan`
- File view: `Views/DuDoan/KetQuaDuDoan.cshtml`

Nguồn dữ liệu:

- Đọc từ `sessionStorage` sau khi V-03 gọi dự đoán thành công.

Hiển thị:

- Tóm tắt thông tin đầu vào.
- Danh sách thuốc gợi ý.
- Điểm phù hợp.
- Lý do gợi ý.
- Liều dùng tham khảo.
- Cảnh báo dị ứng, bệnh nền, tương tác thuốc nếu backend trả về.
- Nút xem chi tiết thuốc đến `GET /Thuoc/ChiTietThuoc/{id}`.
- Nút feedback cho từng kết quả.

API feedback:

- `POST /api/UserPrediction/feedback`

### V-05: Lịch sử tra cứu

View:

- `GET /DuDoan/LichSuTraCuu`
- File view: `Views/DuDoan/LichSuTraCuu.cshtml`

API dùng thật:

- `GET /api/UserPrediction/history`
- `DELETE /api/UserPrediction/history/{historyId}`

Trạng thái frontend hiện tại:

- Tìm kiếm theo bệnh, thông tin triệu chứng hoặc thuốc.
- Xóa từng lịch sử.
- Xóa nhiều lịch sử đã chọn bằng cách gọi nhiều request `DELETE` tuần tự.
- Chưa có endpoint chi tiết riêng cho một bản ghi lịch sử.

### V-06: Tổng quan người dùng

View:

- `GET /DuDoan/TongQuan`
- File view: `Views/DuDoan/TongQuan.cshtml`

Nguồn dữ liệu:

- `GET /api/UserPrediction/history`
- Số bệnh đang hoạt động và số thuốc đang hoạt động được lấy từ MVC controller `DuDoanController`.

Hiển thị:

- Số bệnh đang hoạt động.
- Số thuốc đang hoạt động.
- Số lịch sử tra cứu của tài khoản hiện tại.
- Số thuốc ở lần dự đoán gần nhất.
- Bệnh tra cứu nhiều nhất.
- Hoạt động gần đây.

## 3. Phân hệ Admin

Layout chung:

- `Areas/Admin/Views/Shared/_Layout.cshtml`

Controller MVC điều hướng:

- `Areas/Admin/Controllers/PortalController.cs`

Các route view hiện có:

- `GET /Admin`
- `GET /Admin/Dashboard`
- `GET /Admin/QuanLyThuoc`
- `GET /Admin/QuanLyBenh`
- `GET /Admin/QuanLyTrieuChung`
- `GET /Admin/QuanLyNguoiDung`
- `GET /Admin/QuanLyDiUng`
- `GET /Admin/ThongKe`

### V-07: Dashboard admin

View:

- `Areas/Admin/Views/Dashboard/Dashboard.cshtml`

API:

- `GET /api/admin/Dashboard/statistics`

### V-08: Quản lý thuốc

View:

- `Areas/Admin/Views/Medicine/QuanLyThuoc.cshtml`

API:

- `GET /api/Medicine`
- `GET /api/Medicine/{id}`
- `POST /api/Medicine/create`
- `PATCH /api/Medicine/update`
- `PATCH /api/Medicine/toggle?id={id}&isActive={true|false}`

### V-09: Quản lý bệnh

View:

- `Areas/Admin/Views/Disease/QuanLyBenh.cshtml`

API:

- `GET /api/Disease`
- `GET /api/Disease/{id}`
- `POST /api/Disease/create`
- `PATCH /api/Disease/Update`
- `PATCH /api/Disease/toggle?id={id}`

### V-10: Quản lý triệu chứng

View:

- `Areas/Admin/Views/Symptom/QuanLyTrieuChung.cshtml`

API:

- `GET /api/Symptom`
- `GET /api/Symptom/{id}`
- `POST /api/Symptom/create`
- `PATCH /api/Symptom/update`
- `PATCH /api/Symptom/toggle?id={id}`

### V-11: Quản lý cảnh báo an toàn

View:

- `Areas/Admin/Views/SafetyWarning/QuanLyDiUng.cshtml`

API:

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

### V-12: Quản lý người dùng

View:

- `Areas/Admin/Views/UserAdmin/QuanLyNguoiDung.cshtml`

API:

- `GET /api/User`
- `PATCH /api/User/{userId}/lock?isLocked={true|false}`
- `DELETE /api/User/{userId}`

### V-13: Thống kê admin

View:

- `Areas/Admin/Views/SafetyWarning/ThongKe.cshtml`

API:

- Hiện đang dùng dữ liệu thống kê tổng hợp từ backend admin và dữ liệu hiện có ở frontend.

## 4. Quy ước frontend đang áp dụng

- Route nội bộ Razor ưu tiên `asp-controller`, `asp-action`, `Url.Action`.
- File JS ngoài không dùng Razor trực tiếp, chỉ nhận URL qua `data-*`.
- Các request cần xác thực dùng JWT từ `localStorage`.
- Khi gặp `401`, frontend xóa trạng thái cũ nếu cần và chuyển người dùng về trang đăng nhập.
- Form dự đoán và các thao tác xóa/feedback có trạng thái đang xử lý ở phía client.

## 5. Những gì cố ý chưa thêm

- Không thay DTO hoặc schema API backend.
- Không đổi service, model, DbContext hoặc logic AI/Flask.
- Không thêm endpoint chi tiết lịch sử vì backend hiện chưa có.
- Không tự suy diễn thêm màn hình ngoài các route đang tồn tại trong repo.
