# 1. Tên đề tài: Website dự đoán thuốc cho một bệnh cụ thể

# 2. Mô tả tổng quan đề tài
Đề tài xây dựng một website hỗ trợ người dùng tra cứu và nhận gợi ý thuốc phù hợp cho một bệnh cụ thể dựa trên thông tin đầu vào như bệnh cụ thể, triệu chứng, mức độ bệnh, tiền sử dị ứng, bệnh nền hoặc một số lưu ý sức khỏe cơ bản. Hệ thống hướng đến việc hỗ trợ người dùng tham khảo thông tin thuốc nhanh hơn, dễ hiểu hơn và có cảnh báo cơ bản về các trường hợp không nên sử dụng thuốc.

Website không có mục đích thay thế bác sĩ hoặc đưa ra đơn thuốc chính thức. Kết quả dự đoán,gợi ý thuốc chỉ mang tính chất tham khảo, giúp người dùng có thêm thông tin.

# 3. Lý do chọn đề tài
Hiện nay, nhiều người có thói quen tự tra cứu thông tin thuốc trên Internet. Tuy nhiên, thông tin trên mạng có thể không đầy đủ, khó hiểu hoặc thiếu cảnh báo về dị ứng, bệnh nền và chống chỉ định. Điều này có thể khiến người dùng lựa chọn thuốc không phù hợp.

Vì vậy, nhóm lựa chọn đề tài xây dựng website dự đoán thuốc cho một bệnh cụ thể nhằm hỗ trợ người dùng nhập thông tin bệnh và nhận được gợi ý thuốc có cấu trúc rõ ràng hơn. Đồng thời, hệ thống giúp nhóm vận dụng kiến thức về phân tích yêu cầu, thiết kế giao diện, xây dựng backend/API, thiết kế cơ sở dữ liệu và kiểm thử phần mềm.

# 4. Mục tiêu của hệ thống
Hệ thống cần đạt được các mục tiêu chính sau:
- Hỗ trợ người dùng nhập thông tin liên quan đến bệnh và triệu chứng nặng nhẹ đi kèm.
- Đưa ra danh sách thuốc gợi ý phù hợp với thông tin đầu vào.
- Hiển thị thông tin cơ bản của thuốc như tên thuốc, công dụng, liều dùng tham khảo, lưu ý và chống chỉ định.
- Cảnh báo người dùng khi thuốc có thể không phù hợp với dị ứng, bệnh nền hoặc thông tin sức khỏe đã nhập.
- Cho phép quản trị viên quản lý dữ liệu thuốc, bệnh, triệu chứng và người dùng.
- Cung cấp giao diện dễ sử dụng, rõ ràng và phù hợp với người dùng phổ thông.
- Đảm bảo kết quả dự đoán chỉ mang tính tham khảo, không thay thế tư vấn y tế chuyên môn.

# 5. Đối tượng sử dụng
## 5.1. Người dùng
Người dùng là người có nhu cầu tra cứu thông tin thuốc hoặc muốn nhận gợi ý thuốc cho một bệnh cụ thể. Người dùng có thể nhập thêm triệu chứng, thông tin dị ứng, bệnh nền và xem kết quả gợi ý từ hệ thống.

## 5.2. Quản trị viên
Quản trị viên là người quản lý dữ liệu trong hệ thống. Quản trị viên có thể thêm, sửa, xóa thông tin thuốc, bệnh, triệu chứng, tài khoản người dùng và theo dõi dữ liệu hoạt động của website.

## 5.3. Nhóm phát triển hệ thống
Nhóm phát triển gồm các vai trò BA/PO, UI/UX - Frontend, Backend/API, Database/DevOps và Tester/QA. Mỗi vai trò chịu trách nhiệm một phần trong quá trình phân tích, thiết kế, xây dựng và kiểm thử hệ thống.

# 6. Phạm vi chức năng
## 6.1. Chức năng dành cho người dùng
Hệ thống cho phép người dùng thực hiện các chức năng sau:
- Đăng ký tài khoản.
- Đăng nhập và đăng xuất.
- Nhập thông tin bệnh và triệu chứng đi kèm.
- Nhập thông tin dị ứng, bệnh nền hoặc các lưu ý sức khỏe nếu có.
- Gửi thông tin để hệ thống dự đoán/gợi ý thuốc.
- Xem danh sách thuốc được gợi ý.
- Xem thông tin chi tiết của từng thuốc.
- Xem cảnh báo nếu thuốc có nguy cơ không phù hợp.
- Xem lại lịch sử tra cứu nếu hệ thống có lưu dữ liệu.

## 6.2. Chức năng dành cho quản trị viên
Hệ thống cho phép quản trị viên thực hiện các chức năng sau:
- Quản lý danh sách thuốc.
- Quản lý danh sách bệnh.
- Quản lý danh sách triệu chứng.
- Quản lý thông tin chống chỉ định, dị ứng và lưu ý khi dùng thuốc.
- Quản lý tài khoản người dùng.
- Xem thống kê cơ bản về số lượt tra cứu hoặc số lượng thuốc trong hệ thống.
- Cập nhật dữ liệu để cải thiện kết quả gợi ý.

## 6.3. Chức năng xử lý của hệ thống
Hệ thống cần thực hiện các xử lý chính sau:
- Tiếp nhận dữ liệu đầu vào từ người dùng.
- Kiểm tra dữ liệu nhập vào có hợp lệ hay không.
- So sánh thông tin người dùng nhập với dữ liệu bệnh, triệu chứng và thuốc.
- Đưa ra danh sách thuốc gợi ý phù hợp.
- Hiển thị cảnh báo nếu phát hiện yếu tố không an toàn.
- Lưu lịch sử tra cứu nếu người dùng đã đăng nhập.
- Phân quyền giữa người dùng thông thường và quản trị viên.

# 7. Phạm vi dữ liệu
Hệ thống cần quản lý các nhóm dữ liệu chính sau:
- Dữ liệu người dùng.
- Dữ liệu bệnh cụ thể.
- Dữ liệu triệu chứng.
- Dữ liệu thuốc.
- Dữ liệu công dụng thuốc.
- Dữ liệu liều dùng tham khảo.
- Dữ liệu chống chỉ định.
- Dữ liệu dị ứng.
- Dữ liệu bệnh nền có thể ảnh hưởng đến việc dùng thuốc.
- Dữ liệu lịch sử tra cứu của người dùng.

# 8. Phạm vi không thực hiện
Để đảm bảo đề tài phù hợp với thời gian và năng lực thực hiện của nhóm, hệ thống không bao gồm các nội dung sau:
- Không thay thế bác sĩ hoặc nhân viên y tế.
- Không đưa ra đơn thuốc chính thức.
- Không xử lý tất cả các loại bệnh.
- Không tư vấn điều trị cho tình huống cấp cứu.
- Không bán thuốc trực tiếp trên hệ thống.
- Không tích hợp thanh toán online.
- Không giao thuốc hoặc quản lý đơn hàng thuốc.
- Không kết nối trực tiếp với bệnh viện, nhà thuốc hoặc hồ sơ y tế quốc gia.
- Không đảm bảo kết quả gợi ý đúng tuyệt đối trong mọi trường hợp.
- Không tự động chẩn đoán bệnh nếu dữ liệu đầu vào không đủ rõ ràng.

# 9. Giới hạn của đề tài
Đề tài chỉ tập trung vào một bệnh cụ thể do nhóm lựa chọn. Dữ liệu thuốc, triệu chứng và cảnh báo được xây dựng trong phạm vi học tập, có thể chưa đầy đủ như hệ thống y tế thực tế.

Kết quả dự đoán/gợi ý phụ thuộc vào dữ liệu mà nhóm thu thập và thiết kế. Vì vậy, hệ thống cần hiển thị thông báo rõ ràng rằng thông tin chỉ mang tính tham khảo. Người dùng nên hỏi ý kiến bác sĩ hoặc dược sĩ trước khi sử dụng thuốc.

# 10. Yêu cầu đầu ra của hệ thống
Sau khi người dùng nhập thông tin, hệ thống cần trả về kết quả gồm:
- Tên thuốc được gợi ý.
- Công dụng chính của thuốc.
- Liều dùng tham khảo nếu có.
- Lưu ý khi sử dụng.
- Chống chỉ định nếu có.
- Cảnh báo liên quan đến dị ứng hoặc bệnh nền.
- Mức độ phù hợp hoặc lý do thuốc được gợi ý.

# 11. Kết quả mong đợi
Sau khi hoàn thành, website có thể hỗ trợ người dùng nhập thông tin bệnh và triệu chứng đi kèm và nhận danh sách thuốc gợi ý phù hợp trong phạm vi bệnh đã chọn. Hệ thống có giao diện dễ dùng, dữ liệu được tổ chức rõ ràng, có phân quyền cơ bản và có khả năng quản lý dữ liệu thuốc từ phía quản trị viên.

Đối với nhóm phát triển, đề tài giúp rèn luyện quy trình phát triển phần mềm theo hướng Agile/Scrum, từ khảo sát nhu cầu, phân tích yêu cầu, thiết kế backlog, xây dựng giao diện, phát triển API, thiết kế cơ sở dữ liệu đến kiểm thử hệ thống.

# 12. Ghi chú an toàn y tế
Website cần hiển thị cảnh báo rõ ràng cho người dùng: "Kết quả gợi ý thuốc chỉ mang tính chất tham khảo, không thay thế tư vấn, chẩn đoán hoặc chỉ định điều trị từ bác sĩ, dược sĩ hoặc nhân viên y tế có chuyên môn."