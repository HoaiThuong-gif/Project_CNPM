
# 1. Mục đích
Tài liệu này mô tả các user stories của hệ thống website dự đoán, gợi ý thuốc cho một bệnh cụ thể. User stories được viết dưới góc nhìn của người sử dụng hệ thống nhằm làm rõ người dùng muốn thực hiện chức năng gì và mục đích của chức năng đó.

Các user stories trong tài liệu này được xây dựng dựa trên phạm vi đề tài, kết quả khảo sát nhu cầu người dùng, yêu cầu chức năng và yêu cầu phi chức năng đã xác định.

# 2. Cấu trúc user story
Mỗi user story được viết theo cấu trúc: Là một [vai trò], tôi muốn [chức năng], để [mục đích].

Trong đó:
- Vai trò là người sử dụng chức năng, ví dụ: người dùng hoặc quản trị viên.
- Chức năng là hành động mà người dùng muốn thực hiện trên hệ thống.
- Mục đích là lý do người dùng cần chức năng đó.

# 3. Nhóm user stories dành cho người dùng
## US-01: Đăng ký tài khoản
Là một người dùng, tôi muốn đăng ký tài khoản, để có thể sử dụng các chức năng của hệ thống.

## US-02: Đăng nhập hệ thống
Là một người dùng, tôi muốn đăng nhập vào hệ thống, để có thể sử dụng các chức năng dành cho tài khoản của mình.

## US-03: Đăng xuất hệ thống
Là một người dùng, tôi muốn đăng xuất khỏi hệ thống, để bảo vệ tài khoản sau khi sử dụng xong.

## US-04: Nhập thông tin bệnh
Là một người dùng, tôi muốn nhập thông tin bệnh cụ thể, để hệ thống có cơ sở đưa ra gợi ý thuốc phù hợp.

## US-05: Nhập triệu chứng
Là một người dùng, tôi muốn nhập các triệu chứng đang gặp phải khi đang bị bệnh đó, để hệ thống hiểu rõ hơn tình trạng của tôi trước khi gợi ý thuốc.

## US-06: Nhập mức độ triệu chứng
Là một người dùng, tôi muốn nhập mức độ nặng nhẹ của triệu chứng, để hệ thống đưa ra kết quả gợi ý phù hợp hơn.

## US-07: Nhập thông tin dị ứng thuốc
Là một người dùng, tôi muốn nhập thông tin dị ứng thuốc, để hệ thống cảnh báo nếu thuốc được gợi ý có nguy cơ không phù hợp với tôi.

## US-08: Nhập thông tin bệnh nền
Là một người dùng, tôi muốn nhập thông tin bệnh nền, để hệ thống có thể cảnh báo khi thuốc có chống chỉ định hoặc cần thận trọng.

## US-09: Nhập thuốc đang sử dụng
Là một người dùng, tôi muốn nhập thông tin thuốc đang sử dụng, để hệ thống có thêm dữ liệu tham khảo khi đưa ra cảnh báo an toàn.

## US-10: Gửi yêu cầu dự đoán, gợi ý thuốc
Là một người dùng, tôi muốn gửi thông tin đã nhập cho hệ thống, để nhận danh sách thuốc gợi ý tham khảo.

## US-11: Xem danh sách thuốc được gợi ý
Là một người dùng, tôi muốn xem danh sách thuốc được hệ thống gợi ý, để biết những thuốc nào có thể phù hợp với thông tin đã nhập.

## US-12: Xem thông tin chi tiết thuốc
Là một người dùng, tôi muốn xem thông tin chi tiết của từng thuốc, để hiểu rõ công dụng, liều dùng tham khảo, lưu ý và chống chỉ định.

## US-13: Xem cảnh báo an toàn
Là một người dùng, tôi muốn xem cảnh báo an toàn khi thuốc có thể không phù hợp, để cân nhắc trước khi sử dụng và hỏi ý kiến bác sĩ hoặc dược sĩ.

## US-14: Xem thông báo kết quả chỉ mang tính tham khảo
Là một người dùng, tôi muốn hệ thống hiển thị thông báo rằng kết quả chỉ mang tính tham khảo, để tránh hiểu nhầm đây là đơn thuốc chính thức.

## US-15: Xem lịch sử tra cứu
Là một người dùng, tôi muốn xem lại lịch sử tra cứu thuốc, để có thể theo dõi các lần tìm kiếm trước đó.

## US-16: Xóa lịch sử tra cứu
Là một người dùng, tôi muốn xóa lịch sử tra cứu, để chủ động quản lý dữ liệu cá nhân của mình.

# 4. Nhóm user stories dành cho quản trị viên
## US-17: Đăng nhập trang quản trị
Là một quản trị viên, tôi muốn đăng nhập vào trang quản trị, để có thể quản lý dữ liệu của hệ thống.

## US-18: Quản lý danh sách thuốc
Là một quản trị viên, tôi muốn thêm, sửa, xóa và xem danh sách thuốc, để dữ liệu thuốc trong hệ thống luôn được cập nhật.

## US-19: Quản lý thông tin chi tiết thuốc
Là một quản trị viên, tôi muốn cập nhật công dụng, liều dùng tham khảo, cách sử dụng, chống chỉ định và tác dụng phụ của thuốc, để người dùng xem được thông tin đầy đủ hơn.

## US-20: Quản lý danh sách bệnh
Là một quản trị viên, tôi muốn thêm, sửa, xóa và xem danh sách bệnh, để hệ thống có dữ liệu phục vụ chức năng gợi ý thuốc.

## US-21: Quản lý danh sách triệu chứng
Là một quản trị viên, tôi muốn thêm, sửa, xóa và xem danh sách triệu chứng, để liên kết triệu chứng với bệnh và thuốc phù hợp.

## US-22: Quản lý thông tin dị ứng
Là một quản trị viên, tôi muốn quản lý thông tin dị ứng thuốc, để hệ thống có thể cảnh báo người dùng khi cần thiết.

## US-23: Quản lý thông tin chống chỉ định
Là một quản trị viên, tôi muốn quản lý thông tin chống chỉ định của thuốc, để hạn chế việc gợi ý thuốc không phù hợp với tình trạng người dùng.

## US-24: Quản lý tài khoản người dùng
Là một quản trị viên, tôi muốn xem và quản lý tài khoản người dùng, để theo dõi và kiểm soát hoạt động trong hệ thống.

## US-25: Khóa hoặc mở khóa tài khoản người dùng
Là một quản trị viên, tôi muốn khóa hoặc mở khóa tài khoản người dùng, để xử lý các tài khoản không phù hợp hoặc cần hạn chế truy cập.

## US-26: Xem thống kê hệ thống
Là một quản trị viên, tôi muốn xem thống kê số lượng người dùng, thuốc, bệnh và lượt tra cứu, để theo dõi tình hình hoạt động của website.

# 5. Nhóm user stories dành cho hệ thống
## US-27: Kiểm tra dữ liệu đầu vào
Là một hệ thống, tôi cần kiểm tra dữ liệu người dùng nhập vào, để tránh xử lý thông tin bị bỏ trống hoặc sai định dạng.

## US-28: Xử lý gợi ý thuốc
Là một hệ thống, tôi cần xử lý thông tin bệnh, triệu chứng và dữ liệu thuốc, để đưa ra danh sách thuốc gợi ý phù hợp.

## US-29: So sánh dữ liệu với thông tin cảnh báo
Là một hệ thống, tôi cần so sánh thông tin dị ứng, bệnh nền và thuốc đang sử dụng với dữ liệu cảnh báo, để phát hiện nguy cơ không phù hợp.

## US-30: Lưu lịch sử tra cứu
Là một hệ thống, tôi cần lưu lại lịch sử tra cứu của người dùng đã đăng nhập, để người dùng có thể xem lại khi cần.

## US-31: Phân quyền truy cập
Là một hệ thống, tôi cần phân quyền giữa người dùng và quản trị viên, để đảm bảo mỗi vai trò chỉ truy cập được các chức năng phù hợp.

## US-32: Hiển thị thông báo lỗi
Là một hệ thống, tôi cần hiển thị thông báo lỗi dễ hiểu khi có thao tác không hợp lệ, để người dùng biết cách sửa và thực hiện lại.
