CREATE DATABASE WebsiteDuDoanThuoc;
GO

USE WebsiteDuDoanThuoc;
GO

-- =========================
-- 1. BẢNG NGƯỜI DÙNG
-- Xóa mềm: có DeleteAt (phân biệt khóa tạm vs xóa hẳn)
-- =========================
CREATE TABLE NguoiDung (
    MaNguoiDung     INT IDENTITY(1,1) PRIMARY KEY,
    HoTen           NVARCHAR(100)   NOT NULL,
    Email           VARCHAR(100)    NOT NULL UNIQUE,
    MatKhauMaHoa    VARCHAR(255)    NOT NULL,
    SoDienThoai     VARCHAR(20),
    GioiTinh        NVARCHAR(10),
    NgaySinh        DATE,
    VaiTro          VARCHAR(20)     DEFAULT 'User',
    BiKhoa          BIT             DEFAULT 0,      
    DeleteAt        DATETIME        NULL,           
    NgayTao         DATETIME        DEFAULT GETDATE(),

    CONSTRAINT CK_NguoiDung_VaiTro CHECK (VaiTro IN ('User', 'Admin'))
);
GO

-- =========================
-- 2. BẢNG BỆNH
-- Xóa mềm: DangHoatDong + DeleteAt (có FK từ LichSuDuDoan)
-- =========================
CREATE TABLE Benh (
    MaBenh              INT IDENTITY(1,1) PRIMARY KEY,
    TenBenh             NVARCHAR(150)   NOT NULL,
    MoTa                NVARCHAR(MAX),
    NhomBenh            NVARCHAR(100),              
    MucDoNghiemTrong    INT             DEFAULT 1, 
    DangHoatDong        BIT             DEFAULT 1,
    DeleteAt            DATETIME        NULL,
    NgayTao             DATETIME        DEFAULT GETDATE(),
    NgayCapNhat         DATETIME        NULL,      

    CONSTRAINT CK_Benh_MucDoNghiemTrong CHECK (MucDoNghiemTrong BETWEEN 1 AND 3)
);
GO

INSERT INTO Benh (TenBenh, NhomBenh, MucDoNghiemTrong, DangHoatDong) VALUES (N'Cảm cúm', N'Hô hấp', 1, 1),
                                                                            (N'Đau đầu', N'Thần kinh', 1, 1),
                                                                            (N'Tiêu chảy', N'Tiêu hoá', 1, 1),
                                                                            (N'Dị ứng / Mề đay', N'Da liễu', 1, 1),
                                                                            (N'Đau dạ dày', N'Tiêu hoá', 2, 1);

-- =========================
-- 3. BẢNG TRIỆU CHỨNG
-- Xóa mềm: DangHoatDong (có FK từ BenhTrieuChung, QuyTacGoiYThuoc, ChiTietTrieuChung)
-- =========================
CREATE TABLE TrieuChung (
    MaTrieuChung    INT IDENTITY(1,1) PRIMARY KEY,
    TenTrieuChung   NVARCHAR(150)   NOT NULL,
    MoTa            NVARCHAR(MAX),
    DangHoatDong    BIT             DEFAULT 1,
    NgayTao         DATETIME        DEFAULT GETDATE()
);
GO

INSERT INTO TrieuChung (TenTrieuChung, MoTa) VALUES (N'Sốt', N'Nhiệt độ cơ thể tăng cao hơn mức bình thường'),
                                                    (N'Đau đầu', N'Cảm giác đau nhức ở vùng đầu'),
                                                    (N'Mệt mỏi', N'Cảm giác thiếu năng lượng, uể oải toàn thân'),
                                                    (N'Sổ mũi', N'Chảy dịch mũi, nghẹt mũi'),
                                                    (N'Hắt hơi', N'Phản xạ hắt hơi liên tục'),
                                                    (N'Ho khan', N'Ho không có đờm'),
                                                    (N'Đau họng', N'Cảm giác đau hoặc rát ở vùng họng'),
                                                    (N'Đau nhức cơ thể', N'Đau mỏi các cơ, khớp toàn thân'),
                                                    (N'Đau căng vùng cổ vai gáy', N'Đau do căng cơ ở vùng cổ, vai, gáy'),
                                                    (N'Chóng mặt', N'Cảm giác mất thăng bằng, choáng váng'),
                                                    (N'Buồn nôn', N'Cảm giác muốn nôn'),
                                                    (N'Tiêu chảy', N'Đi ngoài phân lỏng nhiều lần trong ngày'),
                                                    (N'Đau bụng', N'Đau ở vùng bụng'),
                                                    (N'Mất nước', N'Cơ thể thiếu nước do mất dịch nhiều'),
                                                    (N'Đầy hơi chướng bụng', N'Cảm giác bụng căng đầy do tích khí'),
                                                    (N'Ngứa da', N'Cảm giác ngứa trên da'),
                                                    (N'Mề đay', N'Nổi mẩn đỏ, sẩn phù trên da kèm ngứa'),
                                                    (N'Phát ban', N'Da xuất hiện các nốt hoặc mảng đỏ'),
                                                    (N'Chảy nước mắt', N'Mắt chảy nước nhiều bất thường'),
                                                    (N'Ợ chua', N'Cảm giác nóng rát từ dạ dày lên thực quản'),
                                                    (N'Khó tiêu', N'Cảm giác đầy bụng, khó chịu sau ăn'),
                                                    (N'Đau rát thượng vị', N'Đau hoặc nóng rát ở vùng trên bụng, dưới xương ức');

-- =========================
-- 4. BỆNH - TRIỆU CHỨNG (CÓ TRỌNG SỐ)
-- Xóa cứng: mapping table, không có business history
-- =========================
CREATE TABLE BenhTrieuChung (
    MaBenh          INT,
    MaTrieuChung    INT,
    TrongSo         INT     DEFAULT 1,  -- 1-5: mức độ liên quan của triệu chứng với bệnh

    PRIMARY KEY (MaBenh, MaTrieuChung),
    FOREIGN KEY (MaBenh)        REFERENCES Benh(MaBenh),
    FOREIGN KEY (MaTrieuChung)  REFERENCES TrieuChung(MaTrieuChung),

    CONSTRAINT CK_BenhTrieuChung_TrongSo CHECK (TrongSo BETWEEN 1 AND 5)
);
GO

INSERT INTO BenhTrieuChung (MaBenh, MaTrieuChung, TrongSo)
SELECT b.MaBenh, t.MaTrieuChung, src.TrongSo
FROM (
    VALUES
    (N'Cảm cúm', N'Sốt', 4),
    (N'Cảm cúm', N'Đau đầu', 3),
    (N'Cảm cúm', N'Mệt mỏi', 3),
    (N'Cảm cúm', N'Sổ mũi', 5),
    (N'Cảm cúm', N'Hắt hơi', 4),
    (N'Cảm cúm', N'Ho khan', 4),
    (N'Cảm cúm', N'Đau họng', 3),
    (N'Cảm cúm', N'Đau nhức cơ thể', 3),

    (N'Đau đầu', N'Đau đầu', 5),
    (N'Đau đầu', N'Đau căng vùng cổ vai gáy', 4),
    (N'Đau đầu', N'Chóng mặt', 2),
    (N'Đau đầu', N'Mệt mỏi', 2),
    (N'Đau đầu', N'Sốt', 1),

    (N'Tiêu chảy', N'Tiêu chảy', 5),
    (N'Tiêu chảy', N'Đau bụng', 4),
    (N'Tiêu chảy', N'Buồn nôn', 3),
    (N'Tiêu chảy', N'Mất nước', 4),
    (N'Tiêu chảy', N'Đầy hơi chướng bụng', 2),
    (N'Tiêu chảy', N'Mệt mỏi', 2),
    (N'Tiêu chảy', N'Sốt', 1),

    (N'Dị ứng / Mề đay', N'Ngứa da', 5),
    (N'Dị ứng / Mề đay', N'Mề đay', 5),
    (N'Dị ứng / Mề đay', N'Phát ban', 4),
    (N'Dị ứng / Mề đay', N'Sổ mũi', 2),
    (N'Dị ứng / Mề đay', N'Chảy nước mắt', 2),
    (N'Dị ứng / Mề đay', N'Hắt hơi', 2),

    (N'Đau dạ dày', N'Đau rát thượng vị', 5),
    (N'Đau dạ dày', N'Ợ chua', 5),
    (N'Đau dạ dày', N'Khó tiêu', 4),
    (N'Đau dạ dày', N'Đầy hơi chướng bụng', 4),
    (N'Đau dạ dày', N'Buồn nôn', 3),
    (N'Đau dạ dày', N'Đau bụng', 3)
) AS src(TenBenh, TenTrieuChung, TrongSo)
JOIN Benh b ON b.TenBenh = src.TenBenh
JOIN TrieuChung t ON t.TenTrieuChung = src.TenTrieuChung;

-- =========================
-- 5. BẢNG THUỐC
-- Xóa mềm: DangHoatDong (có FK từ KetQuaDuDoan)
-- =========================
CREATE TABLE Thuoc (
    MaThuoc         INT IDENTITY(1,1) PRIMARY KEY,
    TenThuoc        NVARCHAR(150)   NOT NULL,
    HoatChat        NVARCHAR(255),
    NhomThuoc       NVARCHAR(100),              
    DangBaoChe      NVARCHAR(100),             
    CongDung        NVARCHAR(MAX),
    LieuDung        NVARCHAR(MAX),
    CachDung        NVARCHAR(MAX),
    TacDungPhu      NVARCHAR(MAX),
    LuuY            NVARCHAR(MAX),
    CanKeDon        BIT             DEFAULT 0,  -- 0: OTC (tự mua), 1: cần kê đơn bác sĩ
    DangHoatDong    BIT             DEFAULT 1,
    NgayTao         DATETIME        DEFAULT GETDATE(),
    NgayCapNhat     DATETIME        NULL        
);
GO

INSERT INTO Thuoc (TenThuoc, HoatChat, NhomThuoc, DangBaoChe, CongDung, LieuDung, CachDung, TacDungPhu, LuuY, CanKeDon) VALUES  (N'Paracetamol 500mg', N'Paracetamol', N'Giảm đau hạ sốt', N'Viên nén', N'Giảm đau đầu và hạ sốt nhẹ đến trung bình do cảm cúm thông thường, an toàn cho hầu hết người lớn', N'500-1000 mg mỗi 4-6 giờ khi cần', N'Uống sau ăn với nhiều nước', N'Hiếm gặp; quá liều có thể gây tổn thương gan', N'Không dùng quá 4g/ngày, tránh kết hợp với rượu', 0),
                                                                                                                                (N'Ibuprofen 200mg', N'Ibuprofen', N'Kháng viêm không steroid (NSAID)', N'Viên nén', N'Giảm đau nhức cơ thể và hạ sốt kèm tác dụng kháng viêm cho cảm cúm có đau nhức nhiều', N'200-400 mg mỗi 6-8 giờ khi cần', N'Uống sau ăn no để giảm kích ứng dạ dày', N'Đau dạ dày, buồn nôn, ợ nóng', N'Tránh dùng khi đang đau dạ dày hoặc viêm loét tiêu hoá', 0),
                                                                                                                                (N'Cetirizine 10mg', N'Cetirizine', N'Kháng histamin H1', N'Viên nén', N'Giảm sổ mũi hắt hơi và chảy nước mắt do cảm cúm kèm dị ứng đường hô hấp', N'10 mg uống 1 lần/ngày vào buổi tối', N'Uống with nước, không phụ thuộc bữa ăn', N'Buồn ngủ nhẹ, khô miệng', N'Thận trọng khi lái xe hoặc vận hành máy móc', 0),
                                                                                                                                (N'Dextromethorphan 15mg', N'Dextromethorphan', N'Giảm ho (tác động trung ương)', N'Siro', N'Giảm ho khan kéo dài do cảm cúm không kèm đờm', N'10-15 ml mỗi 6-8 giờ khi cần', N'Uống trực tiếp hoặc pha với nước ấm', N'Buồn ngủ, chóng mặt nhẹ, buồn nôn', N'Không dùng cho ho có đờm nhiều hoặc trẻ dưới 6 tuổi', 0),
                                                                                                                                (N'Vitamin C 500mg', N'Acid ascorbic', N'Vitamin và khoáng chất', N'Viên sủi', N'Hỗ trợ tăng cường đề kháng giúp rút ngắn thời gian hồi phục khi cảm cúm', N'500 mg uống 1 lần/ngày', N'Hoà tan viên sủi trong nước rồi uống', N'Rối loạn tiêu hoá nhẹ nếu dùng liều cao', N'Người có tiền sử sỏi thận nên hỏi ý kiến dược sĩ', 0),
                                                                                                                                (N'Ibuprofen 400mg', N'Ibuprofen', N'Kháng viêm không steroid (NSAID)', N'Viên nén', N'Giảm đau đầu mức độ trung bình kèm cảm giác căng tức do viêm', N'400 mg mỗi 6-8 giờ khi cần', N'Uống sau ăn no để giảm kích ứng dạ dày', N'Đau dạ dày, buồn nôn, ợ nóng', N'Tránh dùng khi đang đau dạ dày hoặc viêm loét tiêu hoá', 0),
                                                                                                                                (N'Aspirin 500mg', N'Acid acetylsalicylic', N'Kháng viêm không steroid (NSAID)', N'Viên nén', N'Giảm đau đầu kèm tác dụng chống viêm nhẹ cho người lớn', N'500 mg mỗi 4-6 giờ khi cần', N'Uống sau ăn với nhiều nước', N'Kích ứng dạ dày, ù tai nếu dùng liều cao', N'Không dùng cho người dưới 18 tuổi do nguy cơ hội chứng Reye', 0),
                                                                                                                                (N'Cafein kết hợp Paracetamol', N'Paracetamol và Cafein', N'Giảm đau hạ sốt phối hợp', N'Viên nén', N'Giảm đau đầu nhanh hơn nhờ cafein hỗ trợ tăng hấp thu và giảm cảm giác mệt mỏi kèm đau đầu', N'1 viên mỗi 6 giờ khi cần', N'Uống với nước, tránh dùng gần giờ ngủ', N'Mất ngủ, hồi hộp nếu dùng nhiều', N'Hạn chế dùng cho người nhạy cảm với cafein hoặc mất ngủ', 0),
                                                                                                                                (N'Cao dán giảm đau Salonpas', N'Methyl salicylate và Menthol', N'Giảm đau tại chỗ', N'Cao dán', N'Giảm đau đầu do căng cơ vùng cổ vai gáy bằng tác dụng giảm đau tại chỗ', N'Dán 1 miếng lên vùng đau, tối đa 2 lần/ngày', N'Dán trực tiếp lên da sạch và khô', N'Kích ứng da nhẹ, nóng rát tại vị trí dán', N'Không dán lên vết thương hở hoặc da bị kích ứng', 0),
                                                                                                                                (N'Oresol', N'Muối bù điện giải (ORS)', N'Bù nước điện giải', N'Bột pha dung dịch', N'Bù nước và điện giải đã mất do tiêu chảy giúp ngăn ngừa mất nước', N'Pha 1 gói với 200ml nước, uống sau mỗi lần đi ngoài', N'Pha đúng tỉ lệ với nước sạch, uống trong vòng 24 giờ sau khi pha', N'Hiếm gặp; buồn nôn nhẹ nếu uống quá nhanh', N'Pha đúng tỉ lệ ghi trên bao bì, tránh pha quá đặc', 0),
                                                                                                                                (N'Loperamide 2mg', N'Loperamide', N'Giảm nhu động ruột', N'Viên nang', N'Giảm số lần đi ngoài bằng cách làm chậm nhu động ruột cho tiêu chảy không nhiễm khuẩn', N'2 mg sau lần đi ngoài đầu tiên, tối đa 8mg/ngày', N'Uống với nước', N'Táo bón, chướng bụng, chóng mặt', N'Không dùng khi tiêu chảy có sốt cao hoặc phân có máu', 0),
                                                                                                                                (N'Smecta (Diosmectite)', N'Diosmectite', N'Hấp thụ độc tố đường ruột', N'Bột pha hỗn dịch', N'Hấp thụ độc tố và bao phủ niêm mạc ruột giúp giảm tiêu chảy cấp', N'1 gói pha với nước, 2-3 lần/ngày', N'Pha với nửa cốc nước, uống xa các thuốc khác ít nhất 2 giờ', N'Táo bón nhẹ', N'Uống cách xa các thuốc uống khác để tránh giảm hấp thu', 0),
                                                                                                                                (N'Men vi sinh Probiotic', N'Lactobacillus acidophilus', N'Cân bằng vi sinh đường ruột', N'Gói bột', N'Bổ sung lợi khuẩn giúp cân bằng hệ vi sinh đường ruột hỗ trợ phục hồi sau tiêu chảy', N'1 gói/lần, 2 lần/ngày', N'Pha với nước nguội hoặc ăn trực tiếp', N'Đầy hơi nhẹ trong vài ngày đầu', N'Không pha với nước quá nóng làm mất hoạt tính lợi khuẩn', 0),
                                                                                                                                (N'Berberin 50mg', N'Berberin clorid', N'Kháng khuẩn đường ruột nhẹ', N'Viên nén', N'Hỗ trợ giảm tiêu chảy do rối loạn tiêu hoá hoặc nhiễm khuẩn nhẹ đường ruột', N'2 viên mỗi 8 giờ', N'Uống trước ăn với nước', N'Hiếm gặp; rối loạn tiêu hoá nhẹ', N'Không dùng cho phụ nữ có thai', 0),
                                                                                                                                (N'Loratadine 10mg', N'Loratadine', N'Kháng histamin H1 thế hệ 2', N'Viên nén', N'Giảm ngứa và mề đay do dị ứng mà ít gây buồn ngủ, phù hợp dùng ban ngày', N'10 mg uống 1 lần/ngày', N'Uống với nước, không phụ thuộc bữa ăn', N'Khô miệng, đau đầu nhẹ', N'Hiệu quả chậm hơn thuốc kháng histamin thế hệ 1 nhưng ít buồn ngủ', 0),
                                                                                                                                (N'Chlorpheniramine 4mg', N'Chlorpheniramine maleate', N'Kháng histamin H1 thế hệ 1', N'Viên nén', N'Giảm nhanh ngứa và mề đay cấp nhưng gây buồn ngủ nên phù hợp dùng buổi tối', N'4 mg mỗi 6-8 giờ khi cần', N'Uống với nước, tránh dùng khi cần tỉnh táo', N'Buồn ngủ nhiều, khô miệng', N'Không lái xe hoặc vận hành máy móc sau khi dùng', 0),
                                                                                                                                (N'Kem bôi Hydrocortisone 1%', N'Hydrocortisone', N'Kháng viêm corticoid tại chỗ', N'Kem bôi da', N'Giàm viêm và ngứa tại vùng da bị mề đay hoặc dị ứng khu trú', N'Bôi lớp mỏng lên vùng da tổn thương, 1-2 lần/ngày', N'Bôi sau khi vệ sinh sạch vùng da, không băng kín', N'Mỏng da nếu dùng kéo dài, kích ứng nhẹ', N'Không bôi lên vùng da có vết thương hở hoặc nhiễm trùng', 0),
                                                                                                                                (N'Fexofenadine 60mg', N'Fexofenadine', N'Kháng histamin H1 thế hệ 2', N'Viên nén', N'Giảm ngứa mề đay và sổ mũi do dị ứng với tác dụng kéo dài, ít gây buồn ngủ', N'60 mg uống 1-2 lần/ngày', N'Uống với nước, tránh dùng cùng nước ép trái cây', N'Đau đầu nhẹ, buồn nôn hiếm gặp', N'Không dùng cùng nước cam hoặc nước táo vì giảm hấp thu', 0),
                                                                                                                                (N'Calamine Lotion', N'Calamine và Kẽm oxit', N'Làm dịu da tại chỗ', N'Dung dịch bôi ngoài da', N'Làm dịu cảm giác ngứa rát trên da do mề đay, côn trùng đốt hoặc dị ứng nhẹ', N'Lắc đều và bôi lên vùng da ngứa, 2-3 lần/ngày', N'Bôi một lớp mỏng lên da sạch, để khô tự nhiên', N'Hiếm gặp; khô da nhẹ', N'Tránh bôi gần mắt và niêm mạc', 0),
                                                                                                                                (N'Antacid (Nhôm hydroxit + Magie hydroxit)', N'Aluminium hydroxide và Magnesium hydroxide', N'Trung hoà acid dạ dày', N'Hỗn dịch uống', N'Trung hoà nhanh acid dư trong dạ dày giúp giảm ợ chua và đau rát thượng vị tức thời', N'10ml sau ăn 1-2 giờ và trước khi ngủ', N'Lắc kỹ trước khi uống', N'Táo bón hoặc tiêu chảy nhẹ tuỳ liều', N'Không dùng cùng lúc với các thuốc khác vì có thể giảm hấp thu', 0),
                                                                                                                                (N'Omeprazole 20mg (OTC)', N'Omeprazole', N'Ức chế bơm proton (PPI)', N'Viên nang', N'Giảm tiết acid dạ dày kéo dài, phù hợp cho ợ nóng và khó tiêu tái diễn', N'20 mg uống 1 lần/ngày trước ăn sáng', N'Uống nguyên viên với nước, không nhai', N'Đau đầu, đầy bụng, tiêu chảy nhẹ', N'Không dùng liên tục quá 14 ngày mà không hỏi dược sĩ', 0),
                                                                                                                                (N'Simethicone 80mg', N'Simethicone', N'Chống đầy hơi', N'Viên nhai', N'Giảm đầy hơi chướng bụng kèm đau dạ dày do tích khí trong đường tiêu hoá', N'80 mg sau mỗi bữa ăn và trước khi ngủ', N'Nhai kỹ trước khi nuốt', N'Hiếm gặp, rất an toàn', N'Có thể dùng kết hợp với các thuốc giảm acid khác', 0),
                                                                                                                                (N'Sucralfate 1g', N'Sucralfate', N'Bảo vệ niêm mạc dạ dày', N'Viên nén', N'Tạo lớp bảo vệ niêm mạc dạ dày giúp giảm đau rát và hỗ trợ làm lành tổn thương nhẹ', N'1 g uống trước ăn 1 giờ, 2-4 lần/ngày', N'Uống với nước khi bụng đói', N'Táo bón, khô miệng', N'Không nên dùng cùng lúc với thuốc kháng acid khác', 0),
                                                                                                                                (N'Domperidone 10mg', N'Domperidone', N'Điều hoà vận động tiêu hoá', N'Viên nén', N'Giảm cảm giác đầy bụng, buồn nôn và khó tiêu bằng cách tăng vận động dạ dày', N'10 mg trước ăn 15-30 phút, 3 lần/ngày', N'Uống với nước trước bữa ăn', N'Khô miệng, nhức đầu nhẹ, hiếm gặp rối loạn nhịp tim', N'Không dùng quá 1 tuần liên tục mà không hỏi ý kiến dược sĩ', 0);


-- =========================
-- 6. THÀNH PHẦN THUỐC (CHUẨN HOÁ)
-- Xóa cứng: lookup table, không có business history
-- =========================
CREATE TABLE ThanhPhan (
    MaThanhPhan     INT IDENTITY(1,1) PRIMARY KEY,
    TenThanhPhan    NVARCHAR(150)   NOT NULL
);
GO

-- Xóa cứng: mapping table
CREATE TABLE ThuocThanhPhan (
    MaThuoc         INT,
    MaThanhPhan     INT,

    PRIMARY KEY (MaThuoc, MaThanhPhan),
    FOREIGN KEY (MaThuoc)       REFERENCES Thuoc(MaThuoc),
    FOREIGN KEY (MaThanhPhan)   REFERENCES ThanhPhan(MaThanhPhan)
);
GO

-- =========================
-- 7. BỆNH - THUỐC
-- Xóa cứng: mapping table
-- =========================
CREATE TABLE BenhThuoc (
    MaBenh          INT,
    MaThuoc         INT,
    DoUuTien        INT             DEFAULT 1,          
    LoaiDieuTri     VARCHAR(20)     DEFAULT 'primary',  -- primary: đầu tay, secondary: thay thế

    PRIMARY KEY (MaBenh, MaThuoc),
    FOREIGN KEY (MaBenh)    REFERENCES Benh(MaBenh),
    FOREIGN KEY (MaThuoc)   REFERENCES Thuoc(MaThuoc),

    CONSTRAINT CK_BenhThuoc_LoaiDieuTri CHECK (LoaiDieuTri IN ('primary', 'secondary', 'alternative'))
);
GO

INSERT INTO BenhThuoc (MaBenh, MaThuoc, DoUuTien, LoaiDieuTri)
SELECT b.MaBenh, t.MaThuoc, src.DoUuTien, src.LoaiDieuTri
FROM (VALUES
    -- CẢM CÚM
    (N'Cảm cúm', N'Paracetamol 500mg',          1, 'primary'),
    (N'Cảm cúm', N'Ibuprofen 200mg',             2, 'primary'),
    (N'Cảm cúm', N'Cetirizine 10mg',             2, 'secondary'),
    (N'Cảm cúm', N'Dextromethorphan 15mg',        2, 'secondary'),
    (N'Cảm cúm', N'Vitamin C 500mg',             3, 'alternative'),
 
    -- ĐAU ĐẦU
    (N'Đau đầu', N'Paracetamol 500mg',           1, 'primary'),
    (N'Đau đầu', N'Ibuprofen 400mg',             1, 'primary'),
    (N'Đau đầu', N'Aspirin 500mg',               2, 'secondary'),
    (N'Đau đầu', N'Cafein kết hợp Paracetamol',  2, 'secondary'),
    (N'Đau đầu', N'Cao dán giảm đau Salonpas',   3, 'alternative'),
 
    -- TIÊU CHẢY
    (N'Tiêu chảy', N'Oresol',                    1, 'primary'),
    (N'Tiêu chảy', N'Loperamide 2mg',            1, 'primary'),
    (N'Tiêu chảy', N'Smecta (Diosmectite)',       2, 'secondary'),
    (N'Tiêu chảy', N'Men vi sinh Probiotic',      2, 'secondary'),
    (N'Tiêu chảy', N'Berberin 50mg',             3, 'alternative'),
 
    -- DỊ ỨNG / MỀ ĐAY
    (N'Dị ứng / Mề đay', N'Loratadine 10mg',         1, 'primary'),
    (N'Dị ứng / Mề đay', N'Chlorpheniramine 4mg',     1, 'primary'),
    (N'Dị ứng / Mề đay', N'Fexofenadine 60mg',        2, 'secondary'),
    (N'Dị ứng / Mề đay', N'Kem bôi Hydrocortisone 1%',2, 'secondary'),
    (N'Dị ứng / Mề đay', N'Calamine Lotion',          3, 'alternative'),
 
    -- ĐAU DẠ DÀY
    (N'Đau dạ dày', N'Antacid (Nhôm hydroxit + Magie hydroxit)', 1, 'primary'),
    (N'Đau dạ dày', N'Omeprazole 20mg (OTC)',                    1, 'primary'),
    (N'Đau dạ dày', N'Simethicone 80mg',                         2, 'secondary'),
    (N'Đau dạ dày', N'Sucralfate 1g',                            2, 'secondary'),
    (N'Đau dạ dày', N'Domperidone 10mg',                         3, 'alternative')
 
) AS src(TenBenh, TenThuoc, DoUuTien, LoaiDieuTri)
JOIN Benh b  ON b.TenBenh  = src.TenBenh
JOIN Thuoc t ON t.TenThuoc = src.TenThuoc;

-- =========================
-- 8. DỊ ỨNG
-- Xóa mềm: DangHoatDong
-- =========================
CREATE TABLE DiUng (
    MaDiUng         INT IDENTITY(1,1) PRIMARY KEY,
    TenDiUng        NVARCHAR(150)   NOT NULL,
    DangHoatDong    BIT             DEFAULT 1
);
GO

INSERT INTO DiUng (TenDiUng) VALUES (N'Dị ứng Penicillin'),
                                    (N'Dị ứng Aspirin / NSAID'),
                                    (N'Dị ứng Sulfonamide'),
                                    (N'Dị ứng phấn hoa'),
                                    (N'Dị ứng bụi nhà'),
                                    (N'Dị ứng hải sản'),
                                    (N'Dị ứng đậu phộng'),
                                    (N'Dị ứng Latex');
-- =========================
-- 9. CẢNH BÁO DỊ ỨNG THUỐC
-- Xóa cứng: mapping table
-- =========================
CREATE TABLE CanhBaoDiUngThuoc (
    MaThuoc     INT,
    MaDiUng     INT,
    NoiDung     NVARCHAR(MAX),

    PRIMARY KEY (MaThuoc, MaDiUng),
    FOREIGN KEY (MaThuoc)   REFERENCES Thuoc(MaThuoc),
    FOREIGN KEY (MaDiUng)   REFERENCES DiUng(MaDiUng)
);
GO

INSERT INTO CanhBaoDiUngThuoc (MaThuoc, MaDiUng, NoiDung)
SELECT t.MaThuoc, d.MaDiUng, src.NoiDung
FROM (VALUES
    (N'Aspirin 500mg',      N'Dị ứng Aspirin / NSAID',  N'Thuốc chứa Aspirin. Không dùng nếu bạn có tiền sử dị ứng với Aspirin hoặc NSAID.'),
    (N'Ibuprofen 200mg',    N'Dị ứng Aspirin / NSAID',  N'Ibuprofen thuộc nhóm NSAID. Có thể gây phản ứng chéo với người dị ứng Aspirin.'),
    (N'Ibuprofen 400mg',    N'Dị ứng Aspirin / NSAID',  N'Ibuprofen thuộc nhóm NSAID. Có thể gây phản ứng chéo với người dị ứng Aspirin.'),
    (N'Cetirizine 10mg',    N'Dị ứng hải sản',           N'Một số người dị ứng hải sản có thể nhạy cảm với Cetirizine. Hỏi dược sĩ trước khi dùng.'),
    (N'Loratadine 10mg',    N'Dị ứng phấn hoa',          N'Loratadine thường dùng cho dị ứng phấn hoa nhưng cần điều chỉnh liều nếu phản ứng nặng.'),
    (N'Fexofenadine 60mg',  N'Dị ứng phấn hoa',          N'Fexofenadine hiệu quả với dị ứng phấn hoa, theo dõi nếu triệu chứng không cải thiện sau 3 ngày.')
) AS src(TenThuoc, TenDiUng, NoiDung)
JOIN Thuoc t ON t.TenThuoc = src.TenThuoc
JOIN DiUng d ON d.TenDiUng = src.TenDiUng;

-- =========================
-- 10. BỆNH NỀN
-- Xóa mềm: DangHoatDong
-- =========================
CREATE TABLE BenhNen (
    MaBenhNen       INT IDENTITY(1,1) PRIMARY KEY,
    TenBenhNen      NVARCHAR(150)   NOT NULL,
    DangHoatDong    BIT             DEFAULT 1
);
GO

INSERT INTO BenhNen (TenBenhNen) VALUES (N'Viêm loét dạ dày tá tràng'),
                                        (N'Suy thận mạn'),
                                        (N'Suy gan'),
                                        (N'Tiểu đường'),
                                        (N'Tăng huyết áp')

-- =========================
-- 11. CẢNH BÁO BỆNH NỀN
-- Xóa cứng: mapping table
-- =========================
CREATE TABLE CanhBaoBenhNenThuoc (
    MaThuoc     INT,
    MaBenhNen   INT,
    NoiDung     NVARCHAR(MAX),

    PRIMARY KEY (MaThuoc, MaBenhNen),
    FOREIGN KEY (MaThuoc)   REFERENCES Thuoc(MaThuoc),
    FOREIGN KEY (MaBenhNen) REFERENCES BenhNen(MaBenhNen)
);
GO

INSERT INTO CanhBaoBenhNenThuoc (MaThuoc, MaBenhNen, NoiDung)
SELECT t.MaThuoc, bn.MaBenhNen, src.NoiDung
FROM (VALUES
    -- Aspirin
    (N'Aspirin 500mg', N'Viêm loét dạ dày tá tràng', N'Aspirin kích ứng mạnh niêm mạc dạ dày. Không dùng khi đang viêm loét dạ dày.'),
    (N'Aspirin 500mg', N'Suy thận mạn',               N'Aspirin tích lũy ở người suy thận, tăng nguy cơ chảy máu. Hỏi bác sĩ trước khi dùng.'),
 
    -- Ibuprofen 200mg
    (N'Ibuprofen 200mg', N'Viêm loét dạ dày tá tràng', N'Ibuprofen có thể làm nặng thêm loét dạ dày. Uống sau ăn, kết hợp thuốc bảo vệ dạ dày nếu cần.'),
    (N'Ibuprofen 200mg', N'Suy thận mạn',               N'NSAID giảm lưu lượng máu thận, nguy hiểm với người suy thận. Dùng Paracetamol thay thế.'),
    (N'Ibuprofen 200mg', N'Tăng huyết áp',               N'NSAID có thể làm tăng huyết áp và giảm hiệu quả thuốc hạ áp. Thận trọng khi dùng.'),
 
    -- Ibuprofen 400mg
    (N'Ibuprofen 400mg', N'Viêm loét dạ dày tá tràng', N'Ibuprofen có thể làm nặng thêm loét dạ dày. Uống sau ăn, kết hợp thuốc bảo vệ dạ dày nếu cần.'),
    (N'Ibuprofen 400mg', N'Suy thận mạn',               N'NSAID giảm lưu lượng máu thận, nguy hiểm với người suy thận. Dùng Paracetamol thay thế.'),
    (N'Ibuprofen 400mg', N'Tăng huyết áp',               N'NSAID có thể làm tăng huyết áp và giảm hiệu quả thuốc hạ áp. Thận trọng khi dùng.'),
 
    -- Paracetamol
    (N'Paracetamol 500mg', N'Suy gan', N'Paracetamol chuyển hóa qua gan. Người suy gan cần giảm liều hoặc hỏi bác sĩ.'),
 
    -- Omeprazole
    (N'Omeprazole 20mg (OTC)', N'Suy gan', N'Omeprazole chuyển hóa qua gan. Người suy gan cần giảm liều, hỏi bác sĩ.'),
 
    -- Domperidone
    (N'Domperidone 10mg', N'Suy gan',       N'Domperidone chuyển hóa qua gan. Không dùng cho người suy gan nặng.'),
    (N'Domperidone 10mg', N'Tăng huyết áp', N'Domperidone có thể ảnh hưởng nhịp tim, thận trọng với người có bệnh tim mạch.'),
 
    -- Chlorpheniramine
    (N'Chlorpheniramine 4mg', N'Tiểu đường',   N'Một số dạng bào chế Chlorpheniramine có đường. Chọn dạng không đường.'),
    (N'Chlorpheniramine 4mg', N'Tăng huyết áp', N'Thận trọng khi dùng kháng histamin thế hệ 1 với người tăng huyết áp.'),
 
    -- Sucralfate
    (N'Sucralfate 1g', N'Suy thận mạn', N'Sucralfate chứa nhôm, người suy thận có thể tích lũy nhôm. Hỏi bác sĩ trước khi dùng.')
 
) AS src(TenThuoc, TenBenhNen, NoiDung)
JOIN Thuoc   t  ON t.TenThuoc    = src.TenThuoc
JOIN BenhNen bn ON bn.TenBenhNen = src.TenBenhNen;
-- =========================
-- 12. TƯƠNG TÁC THUỐC (TABLE MỚI)
-- Xóa cứng: mapping table
-- Scope tham khảo: hiển thị cảnh báo khi user đang dùng nhiều thuốc cùng lúc
-- =========================
CREATE TABLE TuongTacThuoc (
    MaThuoc1            INT,
    MaThuoc2            INT,
    MucDoNghiemTrong    INT             NOT NULL,   -- 1: nhẹ, 2: trung bình, 3: nghiêm trọng
    MoTa                NVARCHAR(MAX),              -- mô tả tương tác để hiển thị cho user tham khảo

    PRIMARY KEY (MaThuoc1, MaThuoc2),
    FOREIGN KEY (MaThuoc1) REFERENCES Thuoc(MaThuoc),
    FOREIGN KEY (MaThuoc2) REFERENCES Thuoc(MaThuoc),

    CONSTRAINT CK_TuongTacThuoc_MaThuoc     CHECK (MaThuoc1 < MaThuoc2), 
    CONSTRAINT CK_TuongTacThuoc_MucDo       CHECK (MucDoNghiemTrong BETWEEN 1 AND 3)
);
GO

INSERT INTO TuongTacThuoc (MaThuoc1, MaThuoc2, MucDoNghiemTrong, MoTa)
SELECT
    CASE WHEN t1.MaThuoc < t2.MaThuoc THEN t1.MaThuoc ELSE t2.MaThuoc END,
    CASE WHEN t1.MaThuoc < t2.MaThuoc THEN t2.MaThuoc ELSE t1.MaThuoc END,
    src.MucDo,
    src.MoTa
FROM (VALUES
    (N'Aspirin 500mg',      N'Ibuprofen 200mg',     2, N'Hai thuốc cùng nhóm NSAID, dùng chung tăng nguy cơ chảy máu tiêu hóa và tổn thương thận.'),
    (N'Aspirin 500mg',      N'Ibuprofen 400mg',     2, N'Hai thuốc cùng nhóm NSAID, dùng chung tăng nguy cơ chảy máu tiêu hóa và tổn thương thận.'),
    (N'Ibuprofen 200mg',    N'Ibuprofen 400mg',     3, N'Không dùng hai dạng liều Ibuprofen cùng lúc, nguy cơ quá liều nghiêm trọng.'),
    (N'Aspirin 500mg',      N'Sucralfate 1g',       1, N'Sucralfate giảm hấp thu Aspirin. Uống cách nhau ít nhất 2 giờ.'),
    (N'Omeprazole 20mg (OTC)', N'Sucralfate 1g',    1, N'Sucralfate cần môi trường acid để hoạt động, Omeprazole giảm acid có thể làm giảm hiệu quả Sucralfate. Uống cách nhau ít nhất 2 giờ.'),
    (N'Smecta (Diosmectite)', N'Loperamide 2mg',    1, N'Smecta có thể hấp phụ Loperamide, làm giảm hiệu quả. Uống cách nhau ít nhất 2 giờ.'),
    (N'Smecta (Diosmectite)', N'Berberin 50mg',     1, N'Smecta có thể hấp phụ Berberin, làm giảm hiệu quả. Uống cách nhau ít nhất 2 giờ.'),
    (N'Chlorpheniramine 4mg', N'Dextromethorphan 15mg', 1, N'Cả hai đều gây buồn ngủ và ức chế thần kinh trung ương. Dùng chung tăng nguy cơ buồn ngủ quá mức.'),
    (N'Cetirizine 10mg',    N'Chlorpheniramine 4mg', 2, N'Hai kháng histamin dùng chung không tăng hiệu quả mà tăng tác dụng phụ (buồn ngủ, khô miệng).'),
    (N'Loratadine 10mg',    N'Cetirizine 10mg',     2, N'Hai kháng histamin dùng chung không tăng hiệu quả mà tăng tác dụng phụ.'),
    (N'Antacid (Nhôm hydroxit + Magie hydroxit)', N'Omeprazole 20mg (OTC)', 1, N'Antacid giảm hấp thu Omeprazole nếu dùng cùng lúc. Uống Omeprazole trước ăn, Antacid sau ăn.')
) AS src(TenThuoc1, TenThuoc2, MucDo, MoTa)
JOIN Thuoc t1 ON t1.TenThuoc = src.TenThuoc1
JOIN Thuoc t2 ON t2.TenThuoc = src.TenThuoc2;

-- =========================
-- 13. QUY TẮC GỢI Ý THUỐC
-- Xóa mềm: DangHoatDong (quy tắc có thể lỗi thời)
-- MaTrieuChung nullable: quy tắc có thể là "Bệnh X → Thuốc Y" không cần triệu chứng
-- =========================
CREATE TABLE QuyTacGoiYThuoc (
    MaQuyTac        INT IDENTITY(1,1) PRIMARY KEY,
    MaBenh          INT             NOT NULL,
    MaTrieuChung    INT             NULL,       -- NULL = áp dụng cho bệnh không phụ thuộc triệu chứng
    MaThuoc         INT             NOT NULL,
    MucDoMin        INT,                        -- mức độ triệu chứng tối thiểu để áp dụng quy tắc
    MucDoMax        INT,                        -- mức độ triệu chứng tối đa
    DoUuTien        INT             DEFAULT 1,  -- độ ưu tiên của quy tắc khi scoring
    LyDo            NVARCHAR(MAX),             -- lý do hiển thị cho user ("được dùng cho bệnh X vì...")
    DangHoatDong    BIT             DEFAULT 1,

    FOREIGN KEY (MaBenh)        REFERENCES Benh(MaBenh),
    FOREIGN KEY (MaTrieuChung)  REFERENCES TrieuChung(MaTrieuChung),
    FOREIGN KEY (MaThuoc)       REFERENCES Thuoc(MaThuoc),

    CONSTRAINT CK_QuyTac_MucDo CHECK (
        MucDoMin IS NULL OR (
            MucDoMin BETWEEN 1 AND 3 AND
            MucDoMax BETWEEN 1 AND 3 AND
            MucDoMin <= MucDoMax
        )
    )
);
GO

-- =========================
-- 14. LỊCH SỬ DỰ ĐOÁN
-- Xóa mềm: giữ nguyên vì là audit log
-- =========================
CREATE TABLE LichSuDuDoan (
    MaLichSu        INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung     INT,
    MaBenh          INT,
    GhiChu          NVARCHAR(MAX),
    NgayTao         DATETIME    DEFAULT GETDATE(),

    FOREIGN KEY (MaNguoiDung)   REFERENCES NguoiDung(MaNguoiDung),
    FOREIGN KEY (MaBenh)        REFERENCES Benh(MaBenh)
);
GO

-- =========================
-- 15. TRIỆU CHỨNG TRONG LỊCH SỬ
-- Xóa cứng: detail của lịch sử, không cần mềm
-- =========================
CREATE TABLE ChiTietTrieuChung (
    MaLichSu        INT,
    MaTrieuChung    INT,
    MucDo           INT,    -- 1: nhẹ, 2: trung bình, 3: nặng

    PRIMARY KEY (MaLichSu, MaTrieuChung),
    FOREIGN KEY (MaLichSu)      REFERENCES LichSuDuDoan(MaLichSu),
    FOREIGN KEY (MaTrieuChung)  REFERENCES TrieuChung(MaTrieuChung),

    CONSTRAINT CK_MucDo_CT CHECK (MucDo BETWEEN 1 AND 3)
);
GO

-- =========================
-- 16. DỊ ỨNG TRONG LỊCH SỬ
-- Xóa cứng: snapshot tại thời điểm dự đoán
-- =========================
CREATE TABLE LichSuDiUng (
    MaLichSu    INT,
    MaDiUng     INT,

    PRIMARY KEY (MaLichSu, MaDiUng),
    FOREIGN KEY (MaLichSu)  REFERENCES LichSuDuDoan(MaLichSu),
    FOREIGN KEY (MaDiUng)   REFERENCES DiUng(MaDiUng)
);
GO

-- =========================
-- 17. BỆNH NỀN TRONG LỊCH SỬ
-- Xóa cứng: snapshot tại thời điểm dự đoán
-- =========================
CREATE TABLE LichSuBenhNen (
    MaLichSu    INT,
    MaBenhNen   INT,

    PRIMARY KEY (MaLichSu, MaBenhNen),
    FOREIGN KEY (MaLichSu)  REFERENCES LichSuDuDoan(MaLichSu),
    FOREIGN KEY (MaBenhNen) REFERENCES BenhNen(MaBenhNen)
);
GO

-- =========================
-- 18. KẾT QUẢ DỰ ĐOÁN
-- Thêm: TenThuocSnapshot, LieuDungSnapshot
-- Lý do: nếu thông tin thuốc thay đổi sau này, lịch sử vẫn phản ánh đúng thời điểm dự đoán
-- =========================
CREATE TABLE KetQuaDuDoan (
    MaKetQua            INT IDENTITY(1,1) PRIMARY KEY,
    MaLichSu            INT,
    MaThuoc             INT,
    Diem                FLOAT,              -- điểm similarity từ ML model (0.0 - 1.0)
    LyDo                NVARCHAR(MAX),      -- giải thích tại sao gợi ý thuốc này (hiển thị cho user)
    CanhBao             NVARCHAR(MAX),      -- cảnh báo dị ứng / bệnh nền / tương tác (chỉ tham khảo)
    TenThuocSnapshot    NVARCHAR(150),      -- snapshot tên thuốc tại thời điểm dự đoán
    LieuDungSnapshot    NVARCHAR(MAX),      -- snapshot liều dùng tại thời điểm dự đoán

    FOREIGN KEY (MaLichSu)  REFERENCES LichSuDuDoan(MaLichSu),
    FOREIGN KEY (MaThuoc)   REFERENCES Thuoc(MaThuoc)
);
GO

-- =========================
-- 19. ĐÁNH GIÁ DỰ ĐOÁN (TABLE MỚI)
-- Xóa cứng: feedback log, không xóa
-- Mục đích 1: reranking ngay lập tức (tính tỉ lệ HuuIch theo cặp bệnh-thuốc)
-- Mục đích 2: training data cho giai đoạn fine-tune ML sau này
-- =========================
CREATE TABLE DanhGiaDuDoan (
    MaDanhGia       INT IDENTITY(1,1) PRIMARY KEY,
    MaKetQua        INT             NOT NULL,
    MaNguoiDung     INT             NOT NULL,
    HuuIch          BIT             NOT NULL,  
    GhiChu          NVARCHAR(MAX),          
    NgayTao         DATETIME        DEFAULT GETDATE(),

    FOREIGN KEY (MaKetQua)      REFERENCES KetQuaDuDoan(MaKetQua),
    FOREIGN KEY (MaNguoiDung)   REFERENCES NguoiDung(MaNguoiDung),

    CONSTRAINT UQ_DanhGia_KetQua_NguoiDung UNIQUE (MaKetQua, MaNguoiDung)  
);
GO

-- =========================
-- DỮ LIỆU MẪU ADMIN
-- =========================
INSERT INTO NguoiDung (HoTen, Email, MatKhauMaHoa, VaiTro)
VALUES (N'Quản trị viên', 'admin@example.com', 'hashed_password', 'Admin');
GO