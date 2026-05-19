# 1. Mục đích
Tài liệu này mô tả các yêu cầu phi chức năng của hệ thống website dự đoán, gợi ý thuốc cho một bệnh cụ thể. Yêu cầu phi chức năng không mô tả trực tiếp hệ thống cần làm chức năng gì, mà mô tả hệ thống cần hoạt động như thế nào để đảm bảo chất lượng, tính ổn định, tính bảo mật và khả năng sử dụng.

Các yêu cầu phi chức năng trong tài liệu này được xây dựng dựa trên phạm vi đề tài, kết quả khảo sát nhu cầu người dùng và các yêu cầu chức năng đã xác định ở PD-8.

# 2. Yêu cầu về giao diện và trải nghiệm người dùng
Hệ thống cần có giao diện đơn giản, rõ ràng và dễ sử dụng đối với người dùng phổ thông. 

Các form nhập liệu cần được bố trí dễ hiểu, có nhãn rõ ràng và có hướng dẫn ngắn gọn để người dùng biết cần nhập thông tin gì. Các thông báo lỗi, cảnh báo hoặc kết quả gợi ý thuốc cần được hiển thị bằng ngôn ngữ dễ hiểu, tránh sử dụng thuật ngữ quá phức tạp.

Website cần có bố cục nhất quán giữa các trang. Các nút chức năng chính như gửi yêu cầu dự đoán, xem chi tiết thuốc, đăng nhập, đăng xuất hoặc quay lại cần được đặt ở vị trí dễ nhìn và dễ thao tác.

# 3. Yêu cầu về hiệu năng
Hệ thống cần phản hồi nhanh khi người dùng thực hiện các thao tác cơ bản. Các trang như trang chủ, trang đăng nhập, trang nhập thông tin bệnh và trang hiển thị kết quả gợi ý thuốc cần được tải trong thời gian hợp lý.

Khi người dùng gửi yêu cầu dự đoán, gợi ý thuốc, hệ thống cần xử lý và trả về kết quả trong thời gian chấp nhận được. Trong trường hợp quá trình xử lý mất nhiều thời gian, hệ thống nên hiển thị trạng thái đang xử lý để người dùng biết rằng yêu cầu đang được thực hiện.

Các thao tác quản trị như thêm, sửa, xóa thuốc, bệnh, triệu chứng hoặc thông tin cảnh báo cần được xử lý ổn định, tránh tình trạng treo trang hoặc mất dữ liệu khi đang cập nhật.

# 4. Yêu cầu về bảo mật
Hệ thống cần bảo vệ thông tin tài khoản của người dùng. Mật khẩu không được lưu dưới dạng văn bản thuần túy. Các chức năng đăng nhập, đăng xuất và phân quyền cần được xử lý đúng để tránh người dùng không có quyền truy cập vào trang quản trị.

Hệ thống cần phân quyền rõ ràng giữa người dùng và quản trị viên. Người dùng chỉ được sử dụng các chức năng như nhập thông tin, nhận gợi ý thuốc, xem chi tiết thuốc và xem lịch sử tra cứu. Chỉ quản trị viên mới được truy cập các chức năng quản lý thuốc, bệnh, triệu chứng, thông tin dị ứng, chống chỉ định và tài khoản người dùng.

Dữ liệu nhập từ người dùng cần được kiểm tra để hạn chế lỗi hoặc dữ liệu không hợp lệ. Hệ thống cần tránh các rủi ro cơ bản như nhập dữ liệu rỗng, nhập sai định dạng hoặc truy cập trái phép vào chức năng không thuộc quyền của người dùng.

# 5. Yêu cầu về an toàn thông tin y tế
Vì hệ thống có liên quan đến thông tin thuốc và sức khỏe, website cần hiển thị cảnh báo rõ ràng rằng kết quả gợi ý thuốc chỉ mang tính tham khảo, không thay thế tư vấn, chẩn đoán hoặc chỉ định điều trị từ bác sĩ, dược sĩ hoặc nhân viên y tế có chuyên môn.

Các thông tin như dị ứng thuốc, bệnh nền hoặc thuốc đang sử dụng cần được xử lý cẩn thận, tránh hiển thị sai hoặc gây hiểu nhầm cho người dùng. Nếu hệ thống không đủ dữ liệu để đưa ra gợi ý phù hợp, cần hiển thị thông báo rõ ràng thay vì cố gắng đưa ra kết quả thiếu cơ sở.

Các cảnh báo về chống chỉ định, dị ứng hoặc bệnh nền cần được trình bày nổi bật để người dùng dễ nhận biết trước khi xem hoặc tham khảo thông tin thuốc.

# 6. Yêu cầu về độ tin cậy
Hệ thống cần hoạt động ổn định trong các thao tác chính như đăng ký, đăng nhập, nhập thông tin bệnh/triệu chứng, gửi yêu cầu gợi ý thuốc, xem kết quả, xem chi tiết thuốc và quản lý dữ liệu từ phía admin.

Kết quả gợi ý thuốc cần được lấy từ dữ liệu đã được quản lý trong hệ thống. Dữ liệu thuốc, bệnh, triệu chứng, chống chỉ định và cảnh báo cần được tổ chức rõ ràng để hạn chế sai sót trong quá trình xử lý.

Trong trường hợp xảy ra lỗi, hệ thống cần hiển thị thông báo dễ hiểu để người dùng biết thao tác chưa được thực hiện thành công. Không nên hiển thị lỗi kỹ thuật quá phức tạp trực tiếp cho người dùng cuối.

# 7. Yêu cầu về khả năng sử dụng
Website cần phù hợp với người dùng phổ thông, đặc biệt là những người chỉ muốn tra cứu nhanh thông tin thuốc. Quy trình sử dụng nên ngắn gọn, dễ hiểu và không yêu cầu người dùng nhập quá nhiều thông tin không cần thiết.

Người dùng cần có thể dễ dàng nhận biết các bước chính gồm nhập thông tin bệnh, gửi yêu cầu, xem kết quả gợi ý thuốc và đọc cảnh báo an toàn. Các thông tin quan trọng như tên thuốc, công dụng, liều dùng tham khảo, lưu ý và chống chỉ định cần được trình bày rõ ràng.

Nếu có nhiều thuốc được gợi ý, hệ thống nên sắp xếp hoặc hiển thị theo cách dễ theo dõi, tránh làm người dùng bị rối.

# 8. Yêu cầu về khả năng tương thích
Website cần có khả năng hoạt động trên các trình duyệt phổ biến như Google Chrome, Microsoft Edge hoặc Firefox. Giao diện cần hiển thị ổn định trên màn hình máy tính và có thể sử dụng được trên thiết bị di động nếu nhóm có triển khai giao diện responsive.

Các thành phần giao diện như form nhập liệu, nút bấm, bảng danh sách thuốc và trang chi tiết thuốc cần hiển thị đúng trên các kích thước màn hình cơ bản.

# 9. Yêu cầu về khả năng bảo trì
Mã nguồn và tài liệu của hệ thống cần được tổ chức rõ ràng để các thành viên trong nhóm có thể dễ dàng tiếp tục phát triển, sửa lỗi hoặc bổ sung chức năng. Các phần như frontend, backend/API, cơ sở dữ liệu, kiểm thử và tài liệu phân tích cần được tách biệt hợp lý.

Dữ liệu thuốc, bệnh, triệu chứng, dị ứng và chống chỉ định cần được thiết kế để quản trị viên có thể cập nhật khi cần. Việc cập nhật dữ liệu không nên yêu cầu chỉnh sửa trực tiếp trong mã nguồn nếu hệ thống đã có chức năng quản trị dữ liệu.

Tên file, tên thư mục, tên chức năng và nội dung commit trên GitHub cần rõ ràng để thuận tiện cho việc theo dõi tiến độ làm việc của nhóm.

# 10. Yêu cầu về khả năng mở rộng
Hệ thống hiện tại chỉ tập trung vào một bệnh cụ thể trong phạm vi đề tài. Tuy nhiên, thiết kế hệ thống nên có khả năng mở rộng trong tương lai để bổ sung thêm bệnh, triệu chứng, thuốc hoặc các loại cảnh báo khác.

Cấu trúc dữ liệu nên được xây dựng theo hướng có thể thêm mới thuốc, thêm mới bệnh, thêm triệu chứng và liên kết thuốc với bệnh hoặc triệu chứng mà không phải thay đổi toàn bộ hệ thống.

Nếu nhóm phát triển tiếp, hệ thống có thể mở rộng thêm các chức năng như tìm kiếm thuốc nâng cao, thống kê chi tiết hơn, gợi ý theo nhiều bệnh hoặc tích hợp nguồn dữ liệu y tế đáng tin cậy.

# 11. Yêu cầu về sao lưu và phục hồi dữ liệu
Dữ liệu quan trọng như tài khoản người dùng, danh sách thuốc, danh sách bệnh, triệu chứng, thông tin dị ứng, chống chỉ định và lịch sử tra cứu cần được lưu trữ an toàn.

Trong phạm vi đề tài học tập, nhóm cần đảm bảo dữ liệu không bị mất trong quá trình thao tác thông thường. Nếu có chỉnh sửa hoặc cập nhật dữ liệu quan trọng, cần kiểm tra kỹ trước khi lưu vào hệ thống.

Nếu có điều kiện triển khai, hệ thống nên có phương án sao lưu cơ sở dữ liệu để có thể phục hồi khi xảy ra lỗi.

# 12. Yêu cầu về kiểm thử
Hệ thống cần được kiểm thử các chức năng chính trước khi hoàn thành. Các trường hợp kiểm thử cần tập trung vào đăng ký, đăng nhập, phân quyền, nhập bệnh, nhập thông tin dị ứng/bệnh nền, gợi ý thuốc, hiển thị cảnh báo an toàn, xem chi tiết thuốc và quản lý dữ liệu từ phía admin.

Ngoài kiểm thử chức năng, nhóm cần kiểm tra thêm các yếu tố phi chức năng như giao diện có dễ dùng không, thông báo lỗi có rõ ràng không, phân quyền có đúng không và hệ thống có hoạt động ổn định khi thao tác liên tục không.

# 13. Danh sách yêu cầu phi chức năng chính
Các yêu cầu phi chức năng chính của hệ thống gồm:
- Giao diện đơn giản, rõ ràng và dễ sử dụng.
- Thông tin thuốc và cảnh báo được trình bày dễ hiểu.
- Hệ thống phản hồi trong thời gian hợp lý.
- Tài khoản người dùng được bảo vệ.
- Phân quyền rõ ràng giữa người dùng và quản trị viên.
- Dữ liệu nhập vào được kiểm tra hợp lệ.
- Cảnh báo an toàn y tế được hiển thị rõ ràng.
- Kết quả gợi ý thuốc chỉ mang tính tham khảo.
- Hệ thống hoạt động ổn định trong các thao tác chính.
- Website tương thích với các trình duyệt phổ biến.
- Mã nguồn và tài liệu được tổ chức dễ bảo trì.
- Cấu trúc hệ thống có khả năng mở rộng trong tương lai.
