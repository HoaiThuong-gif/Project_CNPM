# Tutorial Views

## 1. Phân hệ Authentication

Phân hệ xác thực dùng chung cho cả User và Admin.

### V-01: Login View

Mục tiêu:

- Hiển thị form nhập email và mật khẩu.
- Gọi `POST /api/Auth/login`.
- Lưu JWT token vào `localStorage` hoặc cơ chế lưu token tương đương.
- Điều hướng theo role nhận được trong JWT.

Điều hướng:

- Role `Admin`: chuyển vào phân hệ Admin.
- Role `User`: chuyển vào phân hệ User.

### V-02: Register View

Mục tiêu:

- Hiển thị form nhập họ tên, email, mật khẩu và xác nhận mật khẩu.
- Gọi `POST /api/Auth/register`.
- Hiển thị thông báo thành công hoặc lỗi từ backend.

Workflow xác thực:

```text
Khách truy cập
  -> V-01 Login
  -> Đăng nhập thành công
  -> Điều hướng theo role
```

## 2. Phân hệ User

Phân hệ User dùng layout chung có header, nút đăng xuất và lối vào trang lịch sử.

### V-03: Prediction Input View

Mục tiêu:

- Là màn hình bắt đầu cho người dùng.
- Có dropdown chọn bệnh từ API `GET /api/Disease`.
- Có textarea lớn để nhập mô tả triệu chứng tự do.
- Gọi `POST /api/UserPrediction/predict`.

Dữ liệu gửi:

- `DiseaseId`: mã bệnh được chọn.
- `Symptoms`: mô tả triệu chứng người dùng nhập.

### V-04: Prediction Result View

Mục tiêu:

- Nhận JSON trả về từ V-03.
- Hiển thị danh sách thuốc gợi ý.
- Hiển thị tên thuốc, liều dùng, điểm số và lý do gợi ý.
- Làm nổi bật cảnh báo nếu có dữ liệu trong `AllergyWarnings`, `DiseaseWarnings` hoặc `DrugInteractions`.
- Có nút feedback cho từng kết quả.

API feedback:

- `POST /api/UserPrediction/feedback`

Dữ liệu feedback:

- `ResultId`: lấy từ kết quả dự đoán.
- `IsHelpful`: hữu ích hoặc không hữu ích.
- `Note`: ghi chú tùy chọn.

### V-05: User History View

Mục tiêu:

- Hiển thị lịch sử tra cứu dạng bảng hoặc danh sách card.
- Gọi `GET /api/UserPrediction/history`.
- Mỗi dòng có nút xóa lịch sử.
- Gọi `DELETE /api/UserPrediction/history/{historyId}` khi người dùng xóa.

Workflow User:

```text
V-03 Nhập bệnh và triệu chứng
  -> Gửi AI xử lý
  -> V-04 Xem kết quả và cảnh báo
  -> Gửi feedback nếu muốn

Header
  -> V-05 Xem lịch sử
  -> Xóa lịch sử nếu muốn
```

## 3. Phân hệ Admin

Phân hệ Admin dùng layout riêng, có sidebar cố định bên trái để điều hướng giữa các màn hình quản trị.

### V-06: Dashboard View

Mục tiêu:

- Hiển thị 4 thẻ thống kê tổng quan: users, thuốc, bệnh và lượt dự đoán.
- Gọi `GET /api/admin/Dashboard/statistics`.

### V-07: Medicine Management View

Mục tiêu:

- Hiển thị bảng danh sách thuốc.
- Có chức năng thêm mới, sửa và bật/tắt trạng thái.
- Dùng modal hoặc form panel để nhập chi tiết thuốc.

API chính:

- `GET /api/Medicine`
- `GET /api/Medicine/{id}`
- `POST /api/Medicine/create`
- `PATCH /api/Medicine/update`
- `PATCH /api/Medicine/toggle?id={id}&isActive={true|false}`

### V-08: Disease Management View

Mục tiêu:

- Hiển thị bảng danh sách bệnh.
- Có chức năng thêm mới, sửa và bật/tắt trạng thái.
- Dùng modal hoặc form panel để nhập thông tin bệnh.

API chính:

- `GET /api/Disease`
- `GET /api/Disease/{id}`
- `POST /api/Disease/create`
- `PATCH /api/Disease/Update`
- `PATCH /api/Disease/toggle?id={id}`

### V-09: Symptom Management View

Mục tiêu:

- Hiển thị bảng danh sách triệu chứng.
- Có chức năng thêm mới, sửa và bật/tắt trạng thái.
- Dùng modal hoặc form panel tương tự bệnh và thuốc.

API chính:

- `GET /api/Symptom`
- `GET /api/Symptom/{id}`
- `POST /api/Symptom/create`
- `PATCH /api/Symptom/update`
- `PATCH /api/Symptom/toggle?id={id}`

### V-10: Medicine-Disease Mapping View

Mục tiêu:

- Cho admin chọn một bệnh từ dropdown.
- Hiển thị danh sách thuốc đang được liên kết với bệnh đó.
- Cho phép link thêm thuốc mới vào bệnh.
- Cho phép thiết lập `Priority` và `TreatmentType`.
- Cho phép unlink thuốc khỏi bệnh.

API chính:

- `GET /api/admin/MedicineDiseaseMapping/disease/{diseaseId}/medicines`
- `GET /api/admin/MedicineDiseaseMapping/medicine/{medicineId}/diseases`
- `POST /api/admin/MedicineDiseaseMapping/link`
- `DELETE /api/admin/MedicineDiseaseMapping/unlink/medicine/{medicineId}/disease/{diseaseId}`

Ghi chú:

- Màn hình này ảnh hưởng trực tiếp tới vector store của AI service vì backend sẽ đồng bộ dữ liệu sang Flask khi link hoặc unlink.

### V-11: Safety Warnings View

Mục tiêu:

- Quản lý dữ liệu cảnh báo an toàn cho thuốc.
- Chia thành 3 tab: dị ứng, bệnh nền và tương tác thuốc.

Tab dị ứng:

- `GET /api/SafetyWarning/allergies`
- `POST /api/SafetyWarning/allergies/create`
- `PATCH /api/SafetyWarning/allergies/update`
- `PATCH /api/SafetyWarning/allergies/toggle?id={id}&isActive={true|false}`

Tab bệnh nền:

- `GET /api/SafetyWarning/background-diseases`
- `POST /api/SafetyWarning/background-diseases/create`
- `PATCH /api/SafetyWarning/background-diseases/update`
- `PATCH /api/SafetyWarning/background-diseases/toggle?id={id}&isActive={true|false}`

Tab tương tác thuốc:

- `GET /api/SafetyWarning/drug-interactions`
- `POST /api/SafetyWarning/drug-interactions/create`
- `DELETE /api/SafetyWarning/drug-interactions/{medicine1Id}/{medicine2Id}`

Gợi ý UI:

- Tab tương tác thuốc cần 2 dropdown chọn thuốc A và thuốc B.
- Khi hiển thị tương tác, nên hiển thị cả hai tên thuốc và mô tả mức độ/cảnh báo.

### V-12: User Admin View

Mục tiêu:

- Hiển thị bảng người dùng.
- Có cột trạng thái khóa tài khoản.
- Có nút khóa/mở khóa.
- Có nút xóa tài khoản.

API chính:

- `GET /api/User`
- `PATCH /api/User/{userId}/lock?isLocked={true|false}`
- `DELETE /api/User/{userId}`

Workflow Admin:

```text
Sidebar
  -> Chọn màn hình V-06 đến V-12
  -> Xem danh sách dữ liệu
  -> Thêm, sửa, bật/tắt, khóa hoặc xóa
  -> Gửi API
  -> Đóng modal/form
  -> Reload lại bảng
```

## 4. Gợi ý UI/UX frontend

### Xử lý token

- Tạo một lớp HTTP client dùng chung, ví dụ `axios_interceptor.js`.
- Tự động gắn `Authorization: Bearer <token>` vào request cần xác thực.
- Nếu API trả `401 Unauthorized`, xóa token và chuyển người dùng về V-01 Login.

### Thông báo

- Dùng toast hoặc dialog nhẹ để hiển thị `message` từ backend.
- Thông báo nên xuất hiện sau các hành động: đăng nhập, đăng ký, thêm, sửa, bật/tắt, xóa, gửi feedback.

### Trạng thái tải và lỗi

- Mỗi bảng nên có trạng thái loading.
- Form nên disable nút submit trong lúc gửi API.
- Khi API lỗi, hiển thị thông báo rõ ràng và giữ lại dữ liệu người dùng đã nhập nếu có thể.

### Bố cục dữ liệu

- Các màn hình quản trị nên ưu tiên bảng dữ liệu dễ scan.
- Các form thêm/sửa nên tái sử dụng cùng một modal hoặc form panel.
- Các hành động nguy hiểm như xóa lịch sử, xóa người dùng hoặc unlink thuốc-bệnh nên có bước xác nhận.

## 5. Thứ tự triển khai gợi ý

1. V-01 Login và xử lý token.
2. V-02 Register.
3. Layout User và V-03 Prediction Input.
4. V-04 Prediction Result và feedback.
5. V-05 User History.
6. Layout Admin và V-06 Dashboard.
7. V-07, V-08, V-09 cho dữ liệu danh mục.
8. V-10 Mapping thuốc-bệnh.
9. V-11 Safety Warnings.
10. V-12 User Admin.
