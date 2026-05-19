# 1. Mục đích
Tài liệu này mô tả acceptance criteria cho các chức năng chính của website dự đoán, gợi ý thuốc cho một bệnh cụ thể. Acceptance criteria là các tiêu chí dùng để xác định một chức năng đã được hoàn thành đúng yêu cầu hay chưa.

Các tiêu chí này giúp nhóm phát triển, tester và BA/PO có cơ sở kiểm tra, nghiệm thu và đánh giá kết quả thực hiện của từng chức năng.

# 2. Acceptance criteria cho chức năng đăng ký tài khoản
Chức năng đăng ký tài khoản được chấp nhận khi:
- Người dùng có thể truy cập trang đăng ký.
- Người dùng có thể nhập họ tên, email, mật khẩu và xác nhận mật khẩu.
- Hệ thống kiểm tra các trường bắt buộc không được bỏ trống.
- Hệ thống kiểm tra email đúng định dạng.
- Hệ thống kiểm tra mật khẩu và xác nhận mật khẩu phải trùng nhau.
- Nếu thông tin hợp lệ, hệ thống tạo tài khoản mới cho người dùng.
- Nếu thông tin không hợp lệ, hệ thống hiển thị thông báo lỗi rõ ràng.

# 3. Acceptance criteria cho chức năng đăng nhập
Chức năng đăng nhập được chấp nhận khi:
- Người dùng có thể truy cập trang đăng nhập.
- Người dùng có thể nhập email và mật khẩu.
- Hệ thống kiểm tra email và mật khẩu không được bỏ trống.
- Nếu thông tin đăng nhập đúng, hệ thống cho phép người dùng truy cập vào tài khoản.
- Nếu thông tin đăng nhập sai, hệ thống hiển thị thông báo lỗi.
- Người dùng không có quyền admin không được truy cập trang quản trị.

# 4. Acceptance criteria cho chức năng đăng xuất
Chức năng đăng xuất được chấp nhận khi:
- Người dùng đã đăng nhập có thể bấm nút đăng xuất.
- Sau khi đăng xuất, hệ thống kết thúc phiên đăng nhập hiện tại.
- Người dùng được chuyển về trang đăng nhập hoặc trang chủ.
- Người dùng không thể truy cập các trang yêu cầu đăng nhập nếu chưa đăng nhập lại.

# 5. Acceptance criteria cho chức năng nhập bệnh và triệu chứng
Chức năng nhập bệnh và triệu chứng được chấp nhận khi:
- Người dùng có thể truy cập form nhập thông tin bệnh và triệu chứng.
- Form có các trường nhập bệnh, triệu chứng và mức độ triệu chứng.
- Các trường bắt buộc được hiển thị rõ ràng.
- Người dùng không thể gửi form nếu bỏ trống thông tin bắt buộc.
- Nếu dữ liệu nhập không hợp lệ, hệ thống hiển thị thông báo lỗi dễ hiểu.
- Nếu dữ liệu hợp lệ, hệ thống cho phép gửi thông tin để xử lý gợi ý thuốc.

# 6. Acceptance criteria cho chức năng nhập thông tin dị ứng và bệnh nền
Chức năng nhập thông tin dị ứng và bệnh nền được chấp nhận khi:
- Người dùng có thể nhập thông tin dị ứng thuốc nếu có.
- Người dùng có thể nhập thông tin bệnh nền nếu có.
- Người dùng có thể nhập thông tin thuốc đang sử dụng nếu hệ thống có hỗ trợ.
- Hệ thống lưu hoặc xử lý các thông tin này cùng với yêu cầu gợi ý thuốc.
- Các thông tin dị ứng và bệnh nền được dùng để kiểm tra cảnh báo an toàn.
- Nếu người dùng không có dị ứng hoặc bệnh nền, hệ thống vẫn cho phép tiếp tục thao tác.

# 7. Acceptance criteria cho chức năng dự đoán/gợi ý thuốc
Chức năng dự đoán/gợi ý thuốc được chấp nhận khi:
- Người dùng có thể gửi yêu cầu gợi ý thuốc sau khi nhập thông tin hợp lệ.
- Hệ thống tiếp nhận dữ liệu bệnh, triệu chứng và thông tin sức khỏe liên quan.
- Hệ thống xử lý dữ liệu và trả về danh sách thuốc gợi ý.
- Danh sách thuốc gợi ý phù hợp với bệnh hoặc triệu chứng trong phạm vi dữ liệu của hệ thống.
- Nếu không tìm được thuốc phù hợp, hệ thống hiển thị thông báo rõ ràng.
- Hệ thống không trình bày kết quả như một đơn thuốc chính thức.

# 8. Acceptance criteria cho chức năng hiển thị kết quả gợi ý thuốc
Chức năng hiển thị kết quả gợi ý thuốc được chấp nhận khi:
- Người dùng xem được danh sách thuốc được gợi ý.
- Mỗi thuốc hiển thị tên thuốc.
- Mỗi thuốc hiển thị công dụng chính.
- Mỗi thuốc hiển thị liều dùng tham khảo nếu có dữ liệu.
- Mỗi thuốc hiển thị lưu ý hoặc chống chỉ định nếu có.
- Kết quả được trình bày rõ ràng, dễ đọc.
- Hệ thống hiển thị thông báo rằng kết quả chỉ mang tính tham khảo.

# 9. Acceptance criteria cho chức năng xem chi tiết thuốc
Chức năng xem chi tiết thuốc được chấp nhận khi:
- Người dùng có thể chọn một thuốc trong danh sách gợi ý để xem chi tiết.
- Trang chi tiết thuốc hiển thị tên thuốc.
- Trang chi tiết thuốc hiển thị công dụng.
- Trang chi tiết thuốc hiển thị liều dùng tham khảo.
- Trang chi tiết thuốc hiển thị cách sử dụng nếu có.
- Trang chi tiết thuốc hiển thị chống chỉ định.
- Trang chi tiết thuốc hiển thị tác dụng phụ hoặc lưu ý khi sử dụng nếu có.
- Người dùng có thể quay lại trang kết quả gợi ý thuốc.

# 10. Acceptance criteria cho chức năng cảnh báo an toàn
Chức năng cảnh báo an toàn được chấp nhận khi:
- Hệ thống kiểm tra thông tin dị ứng, bệnh nền hoặc thuốc đang sử dụng của người dùng.
- Nếu thuốc có nguy cơ không phù hợp, hệ thống hiển thị cảnh báo rõ ràng.
- Cảnh báo được đặt ở vị trí dễ nhìn.
- Nội dung cảnh báo dễ hiểu đối với người dùng phổ thông.
- Hệ thống khuyến cáo người dùng hỏi ý kiến bác sĩ hoặc dược sĩ.
- Nếu không có cảnh báo, hệ thống vẫn hiển thị kết quả gợi ý bình thường.

# 11. Acceptance criteria cho chức năng lịch sử tra cứu
Chức năng lịch sử tra cứu được chấp nhận khi:
- Người dùng đã đăng nhập có thể xem lịch sử tra cứu.
- Mỗi lịch sử hiển thị thời gian tra cứu.
- Mỗi lịch sử hiển thị bệnh hoặc triệu chứng đã nhập.
- Mỗi lịch sử hiển thị kết quả thuốc đã được gợi ý.
- Người dùng có thể xem lại chi tiết một lần tra cứu.
- Nếu chưa có lịch sử, hệ thống hiển thị thông báo phù hợp.

# 12. Acceptance criteria cho chức năng xóa lịch sử tra cứu
Chức năng xóa lịch sử tra cứu được chấp nhận khi:
- Người dùng có thể chọn một lịch sử tra cứu để xóa.
- Hệ thống hiển thị xác nhận trước khi xóa.
- Nếu người dùng xác nhận, lịch sử được xóa khỏi danh sách.
- Nếu người dùng hủy thao tác, lịch sử vẫn được giữ nguyên.
- Người dùng chỉ được xóa lịch sử của chính mình.

# 13. Acceptance criteria cho chức năng quản lý thuốc
Chức năng quản lý thuốc được chấp nhận khi:
- Quản trị viên có thể xem danh sách thuốc.
- Quản trị viên có thể thêm thuốc mới.
- Quản trị viên có thể sửa thông tin thuốc.
- Quản trị viên có thể xóa thuốc.
- Các thông tin thuốc bắt buộc không được để trống.
- Sau khi thêm, sửa hoặc xóa, dữ liệu được cập nhật trong hệ thống.
- Người dùng thông thường không được truy cập chức năng quản lý thuốc.

# 14. Acceptance criteria cho chức năng quản lý bệnh
Chức năng quản lý bệnh được chấp nhận khi:
- Quản trị viên có thể xem danh sách bệnh.
- Quản trị viên có thể thêm bệnh mới.
- Quản trị viên có thể sửa thông tin bệnh.
- Quản trị viên có thể xóa bệnh.
- Mỗi bệnh có tên bệnh và mô tả cơ bản.
- Bệnh có thể được liên kết với triệu chứng hoặc thuốc nếu hệ thống hỗ trợ.
- Người dùng thông thường không được truy cập chức năng quản lý bệnh.

# 15. Acceptance criteria cho chức năng quản lý triệu chứng
Chức năng quản lý triệu chứng được chấp nhận khi:
- Quản trị viên có thể xem danh sách triệu chứng.
- Quản trị viên có thể thêm triệu chứng mới.
- Quản trị viên có thể sửa thông tin triệu chứng.
- Quản trị viên có thể xóa triệu chứng.
- Triệu chứng có thể được liên kết với bệnh cụ thể.
- Dữ liệu triệu chứng được sử dụng trong chức năng gợi ý thuốc.
- Người dùng thông thường không được truy cập chức năng quản lý triệu chứng.

# 16. Acceptance criteria cho chức năng quản lý dị ứng và chống chỉ định
Chức năng quản lý dị ứng và chống chỉ định được chấp nhận khi:
- Quản trị viên có thể thêm thông tin dị ứng thuốc.
- Quản trị viên có thể thêm thông tin chống chỉ định.
- Quản trị viên có thể sửa hoặc xóa thông tin dị ứng và chống chỉ định.
- Dữ liệu này được dùng để tạo cảnh báo an toàn cho người dùng.
- Cảnh báo hiển thị đúng khi thông tin người dùng trùng với dữ liệu rủi ro.
- Người dùng thông thường không được truy cập chức năng quản lý dữ liệu cảnh báo.

# 17. Acceptance criteria cho chức năng quản lý người dùng
Chức năng quản lý người dùng được chấp nhận khi:
- Quản trị viên có thể xem danh sách người dùng.
- Quản trị viên có thể xem thông tin cơ bản của người dùng.
- Quản trị viên có thể khóa hoặc mở khóa tài khoản nếu hệ thống có hỗ trợ.
- Người dùng bị khóa không thể đăng nhập hoặc sử dụng chức năng hệ thống.
- Người dùng thông thường không được truy cập danh sách tài khoản người dùng.

# 18. Acceptance criteria cho chức năng thống kê cơ bản
Chức năng thống kê cơ bản được chấp nhận khi:
- Quản trị viên có thể xem số lượng người dùng.
- Quản trị viên có thể xem số lượng thuốc.
- Quản trị viên có thể xem số lượng bệnh.
- Quản trị viên có thể xem số lượt tra cứu.
- Dữ liệu thống kê được hiển thị rõ ràng.
- Người dùng thông thường không được truy cập trang thống kê.

# 19. Acceptance criteria cho phân quyền hệ thống
Chức năng phân quyền được chấp nhận khi:
- Hệ thống phân biệt được người dùng và quản trị viên.
- Người dùng chỉ truy cập được các chức năng dành cho người dùng.
- Quản trị viên truy cập được các chức năng quản lý.
- Người chưa đăng nhập không được truy cập các trang yêu cầu đăng nhập.
- Khi truy cập trái quyền, hệ thống hiển thị thông báo hoặc chuyển hướng phù hợp.

# 20. Acceptance criteria cho thông báo lỗi và cảnh báo
Chức năng thông báo lỗi và cảnh báo được chấp nhận khi:
- Hệ thống hiển thị thông báo khi người dùng nhập thiếu dữ liệu.
- Hệ thống hiển thị thông báo khi đăng nhập sai.
- Hệ thống hiển thị thông báo khi thao tác thêm, sửa, xóa thất bại.
- Thông báo được viết ngắn gọn, dễ hiểu.
- Không hiển thị lỗi kỹ thuật phức tạp trực tiếp cho người dùng cuối.

