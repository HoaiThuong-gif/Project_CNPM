# 1. Mục đích
Tài liệu này mô tả các yêu cầu chức năng của hệ thống website dự đoán, gợi ý thuốc cho một bệnh cụ thể. Các yêu cầu chức năng được xây dựng dựa trên phạm vi đề tài và kết quả khảo sát nhu cầu người dùng.

Mục tiêu của phần này là xác định rõ hệ thống cần có những chức năng nào, chức năng đó dành cho ai sử dụng và hệ thống cần xử lý những gì để đáp ứng nhu cầu của người dùng.

# 2. Nhóm người dùng của hệ thống
Hệ thống gồm hai nhóm người dùng chính:
- Người dùng: là người có nhu cầu nhập bệnh và triệu chứng cơ bản để nhận gợi ý thuốc tham khảo.
- Quản trị viên: là người có quyền quản lý dữ liệu thuốc, bệnh, triệu chứng, người dùng và theo dõi hoạt động cơ bản của hệ thống.

# 3. Yêu cầu chức năng dành cho người dùng
## 3.1. Đăng ký tài khoản
Người dùng có thể đăng ký tài khoản để sử dụng các chức năng của hệ thống.

Hệ thống cần cho phép người dùng nhập các thông tin cơ bản như họ tên, email, mật khẩu và xác nhận mật khẩu. Sau khi đăng ký thành công, tài khoản người dùng được lưu vào hệ thống.

## 3.2. Đăng nhập
Người dùng có thể đăng nhập vào hệ thống bằng tài khoản đã đăng ký.

Hệ thống cần kiểm tra thông tin đăng nhập. Nếu thông tin hợp lệ, người dùng được truy cập vào các chức năng dành cho người dùng. Nếu thông tin không hợp lệ, hệ thống hiển thị thông báo lỗi phù hợp.

## 3.3. Đăng xuất
Người dùng có thể đăng xuất khỏi hệ thống sau khi sử dụng.

Khi đăng xuất, hệ thống kết thúc phiên đăng nhập hiện tại và đưa người dùng về trang đăng nhập hoặc trang chủ.

## 3.4. Nhập thông tin bệnh và triệu chứng
Người dùng có thể nhập thông tin liên quan đến bệnh và triệu chứng đang gặp phải.

Hệ thống cần cung cấp form nhập liệu gồm các thông tin như tên bệnh kèm triệu chứng, mức độ nặng/nhẹ của triệu chứng và các mô tả bổ sung nếu có. Dữ liệu nhập vào cần được kiểm tra để tránh bỏ trống các trường quan trọng.

## 3.5. Nhập thông tin sức khỏe liên quan
Người dùng có thể nhập thêm các thông tin sức khỏe có ảnh hưởng đến việc gợi ý thuốc.

Các thông tin này có thể bao gồm độ tuổi, giới tính, tiền sử dị ứng thuốc, bệnh nền hoặc thuốc đang sử dụng. Những thông tin này giúp hệ thống đưa ra cảnh báo khi thuốc có nguy cơ không phù hợp với người dùng.

## 3.6. Gửi yêu cầu dự đoán/gợi ý thuốc
Sau khi nhập đầy đủ thông tin cần thiết, người dùng có thể gửi yêu cầu để hệ thống xử lý và gợi ý thuốc.

Hệ thống tiếp nhận dữ liệu đầu vào, kiểm tra tính hợp lệ của dữ liệu và chuyển thông tin sang chức năng xử lý dự đoán, gợi ý thuốc.

## 3.7. Xem kết quả gợi ý thuốc
Người dùng có thể xem danh sách thuốc được hệ thống gợi ý dựa trên thông tin đã nhập.

Kết quả cần hiển thị rõ ràng các thông tin cơ bản như tên thuốc, công dụng chính, liều dùng tham khảo, lưu ý khi sử dụng, chống chỉ định và mức độ phù hợp nếu có.

## 3.8. Xem chi tiết thuốc
Người dùng có thể chọn một thuốc trong danh sách gợi ý để xem thông tin chi tiết.

Trang chi tiết thuốc cần hiển thị các nội dung như tên thuốc, công dụng, thành phần nếu có, liều dùng tham khảo, cách sử dụng, tác dụng phụ có thể gặp, chống chỉ định và lưu ý khi dùng thuốc.

## 3.9. Nhận cảnh báo an toàn
Hệ thống cần hiển thị cảnh báo nếu phát hiện thuốc có thể không phù hợp với thông tin người dùng đã nhập.

Ví dụ, nếu người dùng có tiền sử dị ứng với một thành phần thuốc hoặc có bệnh nền liên quan đến chống chỉ định, hệ thống cần hiển thị cảnh báo để người dùng cân nhắc và hỏi ý kiến bác sĩ/dược sĩ.

## 3.10. Xem lịch sử tra cứu
Người dùng đã đăng nhập có thể xem lại lịch sử các lần tra cứu trước đó.

Mỗi lịch sử tra cứu cần lưu các thông tin cơ bản như thời gian tra cứu, bệnh/triệu chứng đã nhập và kết quả thuốc đã được gợi ý.

## 3.11. Xóa lịch sử tra cứu
Người dùng có thể xóa một hoặc nhiều lịch sử tra cứu nếu không muốn lưu lại trong hệ thống.

Hệ thống cần xác nhận trước khi xóa để tránh người dùng xóa nhầm dữ liệu.

# 4. Yêu cầu chức năng dành cho quản trị viên
## 4.1. Đăng nhập quản trị
Quản trị viên có thể đăng nhập vào hệ thống bằng tài khoản có quyền admin.

Hệ thống cần phân quyền để chỉ tài khoản quản trị viên mới được truy cập vào các chức năng quản lý.

## 4.2. Quản lý thuốc
Quản trị viên có thể thêm, sửa, xóa và xem danh sách thuốc trong hệ thống.

Thông tin thuốc cần bao gồm tên thuốc, công dụng, liều dùng tham khảo, cách sử dụng, chống chỉ định, tác dụng phụ, lưu ý khi sử dụng và các thông tin liên quan khác.

## 4.3. Quản lý bệnh
Quản trị viên có thể thêm, sửa, xóa và xem danh sách bệnh trong phạm vi hệ thống.

Mỗi bệnh cần có thông tin cơ bản như tên bệnh, mô tả bệnh và các triệu chứng liên quan.

## 4.4. Quản lý triệu chứng
Quản trị viên có thể thêm, sửa, xóa và xem danh sách triệu chứng.

Triệu chứng có thể được liên kết với bệnh cụ thể để hỗ trợ chức năng dự đoán/gợi ý thuốc.

## 4.5. Quản lý thông tin dị ứng và chống chỉ định
Quản trị viên có thể quản lý các thông tin liên quan đến dị ứng thuốc, chống chỉ định và các trường hợp cần cảnh báo.

Dữ liệu này được sử dụng để hệ thống hiển thị cảnh báo an toàn cho người dùng khi thuốc có nguy cơ không phù hợp.

## 4.6. Quản lý tài khoản người dùng
Quản trị viên có thể xem danh sách tài khoản người dùng trong hệ thống.

Quản trị viên có thể khóa, mở khóa hoặc cập nhật trạng thái tài khoản nếu cần thiết.

## 4.7. Xem thống kê cơ bản
Quản trị viên có thể xem một số thống kê cơ bản của hệ thống như số lượng người dùng, số lượng thuốc, số lượng bệnh, số lượt tra cứu và các thuốc được tra cứu nhiều.

Chức năng này giúp quản trị viên theo dõi tình hình hoạt động của website.

# 5. Yêu cầu chức năng xử lý của hệ thống
## 5.1. Kiểm tra dữ liệu đầu vào
Hệ thống cần kiểm tra dữ liệu người dùng nhập vào trước khi xử lý.

Các trường bắt buộc như bệnh và triệu chứng không được để trống. Nếu dữ liệu không hợp lệ, hệ thống cần hiển thị thông báo lỗi rõ ràng để người dùng nhập lại.

## 5.2. Xử lý dự đoán/gợi ý thuốc
Hệ thống cần xử lý thông tin đầu vào của người dùng để đưa ra danh sách thuốc gợi ý phù hợp.

Việc gợi ý thuốc có thể dựa trên bệnh, triệu chứng, mức độ triệu chứng, dữ liệu thuốc và các thông tin sức khỏe liên quan mà người dùng cung cấp.

## 5.3. So sánh thông tin người dùng với dữ liệu cảnh báo
Hệ thống cần so sánh thông tin dị ứng, bệnh nền hoặc thuốc đang sử dụng của người dùng với dữ liệu chống chỉ định và cảnh báo trong hệ thống.

Nếu phát hiện rủi ro, hệ thống cần hiển thị cảnh báo để người dùng biết thuốc có thể không phù hợp.

## 5.4. Lưu kết quả tra cứu
Nếu người dùng đã đăng nhập, hệ thống có thể lưu lại kết quả tra cứu để người dùng xem lại sau.

Dữ liệu lịch sử tra cứu cần bao gồm thông tin đầu vào, kết quả gợi ý và thời gian thực hiện tra cứu.

## 5.5. Phân quyền người dùng
Hệ thống cần phân quyền giữa người dùng thông thường và quản trị viên.

Người dùng thông thường chỉ được sử dụng các chức năng tra cứu, gợi ý thuốc và xem lịch sử. Quản trị viên được phép truy cập các chức năng quản lý dữ liệu.

# 6. Danh sách yêu cầu chức năng chính
## 6.1. Nhóm chức năng tài khoản
- Đăng ký tài khoản.
- Đăng nhập.
- Đăng xuất.
- Phân quyền người dùng và quản trị viên.
- Quản lý trạng thái tài khoản người dùng.

## 6.2. Nhóm chức năng tra cứu và gợi ý thuốc
- Nhập bệnh và triệu chứng.
- Nhập thông tin dị ứng, bệnh nền và thông tin sức khỏe liên quan.
- Gửi yêu cầu dự đoán/gợi ý thuốc.
- Xem danh sách thuốc được gợi ý.
- Xem thông tin chi tiết thuốc.
- Nhận cảnh báo an toàn.
- Xem lịch sử tra cứu.
- Xóa lịch sử tra cứu.

## 6.3. Nhóm chức năng quản trị dữ liệu
- Quản lý thuốc.
- Quản lý bệnh.
- Quản lý triệu chứng.
- Quản lý thông tin dị ứng.
- Quản lý chống chỉ định.
- Quản lý lưu ý khi sử dụng thuốc.
- Quản lý người dùng.

## 6.4. Nhóm chức năng thống kê
- Xem số lượng người dùng.
- Xem số lượng thuốc.
- Xem số lượng bệnh.
- Xem số lượt tra cứu.
- Xem thuốc được tra cứu hoặc gợi ý nhiều.

# 7. Ràng buộc chức năng
Hệ thống cần có các ràng buộc sau:
- Kết quả gợi ý thuốc chỉ mang tính tham khảo, không được trình bày như đơn thuốc chính thức.
- Hệ thống cần hiển thị cảnh báo an toàn y tế rõ ràng cho người dùng.
- Người dùng cần nhập đủ thông tin bắt buộc trước khi gửi yêu cầu dự đoán/gợi ý thuốc.
- Chỉ quản trị viên mới được thêm, sửa, xóa dữ liệu thuốc, bệnh và triệu chứng.
- Người dùng thông thường không được truy cập vào trang quản trị.
- Dữ liệu thuốc cần được quản lý rõ ràng để tránh hiển thị thông tin thiếu hoặc sai định dạng.
