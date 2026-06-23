CREATE DATABASE WebsiteDuDoanThuoc;
GO

USE WebsiteDuDoanThuoc;
GO

-- =======================================================
-- PHẦN 1: TẠO CẤU TRÚC CÁC BẢNG
-- =======================================================

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

CREATE TABLE TrieuChung (
    MaTrieuChung    INT IDENTITY(1,1) PRIMARY KEY,
    TenTrieuChung   NVARCHAR(150)   NOT NULL,
    MoTa            NVARCHAR(MAX),
    DangHoatDong    BIT             DEFAULT 1,
    NgayTao         DATETIME        DEFAULT GETDATE()
);
GO

CREATE TABLE BenhTrieuChung (
    MaBenh          INT,
    MaTrieuChung    INT,
    TrongSo         INT     DEFAULT 1,
    PRIMARY KEY (MaBenh, MaTrieuChung),
    FOREIGN KEY (MaBenh)        REFERENCES Benh(MaBenh),
    FOREIGN KEY (MaTrieuChung)  REFERENCES TrieuChung(MaTrieuChung),
    CONSTRAINT CK_BenhTrieuChung_TrongSo CHECK (TrongSo BETWEEN 1 AND 5)
);
GO

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
    CanKeDon        BIT             DEFAULT 0,
    DangHoatDong    BIT             DEFAULT 1,
    NgayTao         DATETIME        DEFAULT GETDATE(),
    NgayCapNhat     DATETIME        NULL        
);
GO

CREATE TABLE ThanhPhan (
    MaThanhPhan     INT IDENTITY(1,1) PRIMARY KEY,
    TenThanhPhan    NVARCHAR(150)   NOT NULL
);
GO

CREATE TABLE ThuocThanhPhan (
    MaThuoc         INT,
    MaThanhPhan     INT,
    PRIMARY KEY (MaThuoc, MaThanhPhan),
    FOREIGN KEY (MaThuoc)       REFERENCES Thuoc(MaThuoc),
    FOREIGN KEY (MaThanhPhan)   REFERENCES ThanhPhan(MaThanhPhan)
);
GO

CREATE TABLE BenhThuoc (
    MaBenh          INT,
    MaThuoc         INT,
    DoUuTien        INT             DEFAULT 1,          
    LoaiDieuTri     VARCHAR(20)     DEFAULT 'primary',
    PRIMARY KEY (MaBenh, MaThuoc),
    FOREIGN KEY (MaBenh)    REFERENCES Benh(MaBenh),
    FOREIGN KEY (MaThuoc)   REFERENCES Thuoc(MaThuoc),
    CONSTRAINT CK_BenhThuoc_LoaiDieuTri CHECK (LoaiDieuTri IN ('primary', 'secondary', 'alternative'))
);
GO

CREATE TABLE DiUng (
    MaDiUng         INT IDENTITY(1,1) PRIMARY KEY,
    TenDiUng        NVARCHAR(150)   NOT NULL,
    DangHoatDong    BIT             DEFAULT 1
);
GO

CREATE TABLE CanhBaoDiUngThuoc (
    MaThuoc     INT,
    MaDiUng     INT,
    NoiDung     NVARCHAR(MAX),
    PRIMARY KEY (MaThuoc, MaDiUng),
    FOREIGN KEY (MaThuoc)   REFERENCES Thuoc(MaThuoc),
    FOREIGN KEY (MaDiUng)   REFERENCES DiUng(MaDiUng)
);
GO

CREATE TABLE BenhNen (
    MaBenhNen       INT IDENTITY(1,1) PRIMARY KEY,
    TenBenhNen      NVARCHAR(150)   NOT NULL,
    DangHoatDong    BIT             DEFAULT 1
);
GO

CREATE TABLE CanhBaoBenhNenThuoc (
    MaThuoc     INT,
    MaBenhNen   INT,
    NoiDung     NVARCHAR(MAX),
    PRIMARY KEY (MaThuoc, MaBenhNen),
    FOREIGN KEY (MaThuoc)   REFERENCES Thuoc(MaThuoc),
    FOREIGN KEY (MaBenhNen) REFERENCES BenhNen(MaBenhNen)
);
GO

CREATE TABLE TuongTacThuoc (
    MaThuoc1            INT,
    MaThuoc2            INT,
    MucDoNghiemTrong    INT             NOT NULL,
    MoTa                NVARCHAR(MAX),
    PRIMARY KEY (MaThuoc1, MaThuoc2),
    FOREIGN KEY (MaThuoc1) REFERENCES Thuoc(MaThuoc),
    FOREIGN KEY (MaThuoc2) REFERENCES Thuoc(MaThuoc),
    CONSTRAINT CK_TuongTacThuoc_MaThuoc     CHECK (MaThuoc1 < MaThuoc2), 
    CONSTRAINT CK_TuongTacThuoc_MucDo       CHECK (MucDoNghiemTrong BETWEEN 1 AND 3)
);
GO

CREATE TABLE QuyTacGoiYThuoc (
    MaQuyTac        INT IDENTITY(1,1) PRIMARY KEY,
    MaBenh          INT             NOT NULL,
    MaTrieuChung    INT             NULL,
    MaThuoc         INT             NOT NULL,
    MucDoMin        INT,
    MucDoMax        INT,
    DoUuTien        INT             DEFAULT 1,
    LyDo            NVARCHAR(MAX),
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

CREATE TABLE ChiTietTrieuChung (
    MaLichSu        INT,
    MaTrieuChung    INT,
    MucDo           INT,
    PRIMARY KEY (MaLichSu, MaTrieuChung),
    FOREIGN KEY (MaLichSu)      REFERENCES LichSuDuDoan(MaLichSu),
    FOREIGN KEY (MaTrieuChung)  REFERENCES TrieuChung(MaTrieuChung),
    CONSTRAINT CK_MucDo_CT CHECK (MucDo BETWEEN 1 AND 3)
);
GO

CREATE TABLE LichSuDiUng (
    MaLichSu    INT,
    MaDiUng     INT,
    PRIMARY KEY (MaLichSu, MaDiUng),
    FOREIGN KEY (MaLichSu)  REFERENCES LichSuDuDoan(MaLichSu),
    FOREIGN KEY (MaDiUng)   REFERENCES DiUng(MaDiUng)
);
GO

CREATE TABLE LichSuBenhNen (
    MaLichSu    INT,
    MaBenhNen   INT,
    PRIMARY KEY (MaLichSu, MaBenhNen),
    FOREIGN KEY (MaLichSu)  REFERENCES LichSuDuDoan(MaLichSu),
    FOREIGN KEY (MaBenhNen) REFERENCES BenhNen(MaBenhNen)
);
GO

CREATE TABLE KetQuaDuDoan (
    MaKetQua            INT IDENTITY(1,1) PRIMARY KEY,
    MaLichSu            INT,
    MaThuoc             INT,
    Diem                FLOAT,
    LyDo                NVARCHAR(MAX),
    CanhBao             NVARCHAR(MAX),
    TenThuocSnapshot    NVARCHAR(150),
    LieuDungSnapshot    NVARCHAR(MAX),
    FOREIGN KEY (MaLichSu)  REFERENCES LichSuDuDoan(MaLichSu),
    FOREIGN KEY (MaThuoc)   REFERENCES Thuoc(MaThuoc)
);
GO

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


-- =======================================================
-- PHẦN 2: CHÈN DỮ LIỆU
-- =======================================================

-- 1. NGƯỜI DÙNG
INSERT INTO NguoiDung (HoTen, Email, MatKhauMaHoa, VaiTro)
VALUES (N'Quản trị viên', 'admin@example.com', 'hashed_password', 'Admin');
GO

-- 2. BỆNH
INSERT INTO Benh (TenBenh, MoTa, NhomBenh, MucDoNghiemTrong, DangHoatDong) VALUES
    (N'Cảm cúm', N'Bệnh nhiễm siêu vi đường hô hấp trên, thường gây sốt nhẹ, đau họng, ho, sổ mũi, hắt hơi và mệt mỏi. Phần lớn trường hợp tự cải thiện sau vài ngày nếu nghỉ ngơi, uống đủ nước và theo dõi triệu chứng.', N'Hô hấp', 1, 1),
    (N'Đau đầu', N'Tình trạng đau hoặc căng tức vùng đầu, có thể liên quan căng thẳng, thiếu ngủ, cảm cúm, đau cơ vùng cổ vai gáy hoặc các nguyên nhân khác. Cần đi khám nếu đau dữ dội đột ngột, kéo dài hoặc kèm dấu hiệu thần kinh.', N'Thần kinh', 1, 1),
    (N'Tiêu chảy', N'Tình trạng đi ngoài phân lỏng nhiều lần trong ngày, có thể kèm đau bụng, buồn nôn, mất nước hoặc sốt. Ưu tiên bù nước và theo dõi dấu hiệu mất nước, phân máu hoặc sốt cao.', N'Tiêu hoá', 1, 1),
    (N'Dị ứng / Mề đay', N'Phản ứng quá mẫn biểu hiện bằng ngứa da, nổi mẩn đỏ, mề đay, phát ban, hắt hơi hoặc chảy nước mắt. Cần cảnh giác nếu có khó thở, phù môi mắt hoặc choáng.', N'Da liễu', 1, 1),
    (N'Đau dạ dày', N'Tình trạng đau rát thượng vị, ợ chua, khó tiêu, đầy hơi hoặc buồn nôn, thường liên quan tăng acid, kích ứng niêm mạc hoặc rối loạn tiêu hoá. Cần thận trọng với thuốc NSAID và dấu hiệu xuất huyết tiêu hoá.', N'Tiêu hoá', 2, 1),
    (N'Viêm họng', N'Tình trạng đau rát họng, khàn tiếng, khó nuốt hoặc ho do kích ứng/viêm đường hô hấp trên.', N'Hô hấp', 1, 1),
    (N'Viêm mũi dị ứng', N'Phản ứng viêm niêm mạc mũi do dị nguyên như bụi, phấn hoa, thời tiết hoặc lông thú.', N'Dị ứng - hô hấp', 1, 1),
    (N'Táo bón', N'Tình trạng đi tiêu khó, phân khô cứng hoặc số lần đi tiêu giảm.', N'Tiêu hoá', 1, 1),
    (N'Trào ngược dạ dày thực quản', N'Tình trạng acid hoặc dịch dạ dày trào ngược lên thực quản.', N'Tiêu hoá', 2, 1);
GO

-- 3. TRIỆU CHỨNG
INSERT INTO TrieuChung (TenTrieuChung, MoTa) VALUES 
    (N'Sốt', N'Nhiệt độ cơ thể tăng cao hơn mức bình thường'), (N'Đau đầu', N'Cảm giác đau nhức ở vùng đầu'),
    (N'Mệt mỏi', N'Cảm giác thiếu năng lượng, uể oải toàn thân'), (N'Sổ mũi', N'Chảy dịch mũi, nghẹt mũi'),
    (N'Hắt hơi', N'Phản xạ hắt hơi liên tục'), (N'Ho khan', N'Ho không có đờm'),
    (N'Đau họng', N'Cảm giác đau hoặc rát ở vùng họng'), (N'Đau nhức cơ thể', N'Đau mỏi các cơ, khớp toàn thân'),
    (N'Đau căng vùng cổ vai gáy', N'Đau do căng cơ ở vùng cổ, vai, gáy'), (N'Chóng mặt', N'Cảm giác mất thăng bằng, choáng váng'),
    (N'Buồn nôn', N'Cảm giác muốn nôn'), (N'Tiêu chảy', N'Đi ngoài phân lỏng nhiều lần trong ngày'),
    (N'Đau bụng', N'Đau ở vùng bụng'), (N'Mất nước', N'Cơ thể thiếu nước do mất dịch nhiều'),
    (N'Đầy hơi chướng bụng', N'Cảm giác bụng căng đầy do tích khí'), (N'Ngứa da', N'Cảm giác ngứa trên da'),
    (N'Mề đay', N'Nổi mẩn đỏ, sẩn phù trên da kèm ngứa'), (N'Phát ban', N'Da xuất hiện các nốt hoặc mảng đỏ'),
    (N'Chảy nước mắt', N'Mắt chảy nước nhiều bất thường'), (N'Ợ chua', N'Cảm giác nóng rát từ dạ dày lên thực quản'),
    (N'Khó tiêu', N'Cảm giác đầy bụng, khó chịu sau ăn'), (N'Đau rát thượng vị', N'Đau hoặc nóng rát ở vùng trên bụng, dưới xương ức'),
    (N'Nghẹt mũi', N'Cảm giác tắc hoặc khó thở qua mũi do niêm mạc mũi sưng nề hoặc nhiều dịch tiết.'),
    (N'Khàn tiếng', N'Giọng nói thay đổi, khàn hoặc mất tiếng do kích ứng, viêm họng hoặc viêm thanh quản.'),
    (N'Khó nuốt', N'Cảm giác đau hoặc vướng khi nuốt thức ăn, nước uống hoặc nước bọt.'),
    (N'Ngứa mũi', N'Cảm giác ngứa, kích thích trong mũi, thường gặp trong viêm mũi dị ứng.'),
    (N'Táo bón', N'Đi tiêu khó, phân khô cứng hoặc giảm số lần đi tiêu so với bình thường.'),
    (N'Nóng rát sau xương ức', N'Cảm giác nóng rát vùng ngực sau xương ức, thường liên quan trào ngược acid.'),
    (N'Ho có đờm', N'Ho kèm chất nhầy hoặc đờm trong đường hô hấp.');
GO

-- 4. BỆNH - TRIỆU CHỨNG
INSERT INTO BenhTrieuChung (MaBenh, MaTrieuChung, TrongSo)
SELECT b.MaBenh, t.MaTrieuChung, src.TrongSo
FROM (VALUES
    (N'Cảm cúm', N'Sốt', 4), (N'Cảm cúm', N'Đau đầu', 3), (N'Cảm cúm', N'Mệt mỏi', 3), (N'Cảm cúm', N'Sổ mũi', 5),
    (N'Cảm cúm', N'Hắt hơi', 4), (N'Cảm cúm', N'Ho khan', 4), (N'Cảm cúm', N'Đau họng', 3), (N'Cảm cúm', N'Đau nhức cơ thể', 3),
    (N'Đau đầu', N'Đau đầu', 5), (N'Đau đầu', N'Đau căng vùng cổ vai gáy', 4), (N'Đau đầu', N'Chóng mặt', 2),
    (N'Đau đầu', N'Mệt mỏi', 2), (N'Đau đầu', N'Sốt', 1), (N'Tiêu chảy', N'Tiêu chảy', 5), (N'Tiêu chảy', N'Đau bụng', 4),
    (N'Tiêu chảy', N'Buồn nôn', 3), (N'Tiêu chảy', N'Mất nước', 4), (N'Tiêu chảy', N'Đầy hơi chướng bụng', 2),
    (N'Tiêu chảy', N'Mệt mỏi', 2), (N'Tiêu chảy', N'Sốt', 1), (N'Dị ứng / Mề đay', N'Ngứa da', 5),
    (N'Dị ứng / Mề đay', N'Mề đay', 5), (N'Dị ứng / Mề đay', N'Phát ban', 4), (N'Dị ứng / Mề đay', N'Sổ mũi', 2),
    (N'Dị ứng / Mề đay', N'Chảy nước mắt', 2), (N'Dị ứng / Mề đay', N'Hắt hơi', 2), (N'Đau dạ dày', N'Đau rát thượng vị', 5),
    (N'Đau dạ dày', N'Ợ chua', 5), (N'Đau dạ dày', N'Khó tiêu', 4), (N'Đau dạ dày', N'Đầy hơi chướng bụng', 4),
    (N'Đau dạ dày', N'Buồn nôn', 3), (N'Đau dạ dày', N'Đau bụng', 3),
    (N'Viêm họng', N'Đau họng', 5), (N'Viêm họng', N'Khàn tiếng', 4), (N'Viêm họng', N'Khó nuốt', 4),
    (N'Viêm họng', N'Ho khan', 3), (N'Viêm họng', N'Sốt', 2), (N'Viêm mũi dị ứng', N'Hắt hơi', 5),
    (N'Viêm mũi dị ứng', N'Sổ mũi', 5), (N'Viêm mũi dị ứng', N'Nghẹt mũi', 4), (N'Viêm mũi dị ứng', N'Ngứa mũi', 5),
    (N'Viêm mũi dị ứng', N'Chảy nước mắt', 3), (N'Táo bón', N'Táo bón', 5), (N'Táo bón', N'Đau bụng', 3),
    (N'Táo bón', N'Đầy hơi chướng bụng', 4), (N'Táo bón', N'Khó tiêu', 2), (N'Trào ngược dạ dày thực quản', N'Ợ chua', 5),
    (N'Trào ngược dạ dày thực quản', N'Nóng rát sau xương ức', 5), (N'Trào ngược dạ dày thực quản', N'Khó tiêu', 4),
    (N'Trào ngược dạ dày thực quản', N'Buồn nôn', 2), (N'Trào ngược dạ dày thực quản', N'Đau rát thượng vị', 4)
) AS src(TenBenh, TenTrieuChung, TrongSo)
JOIN Benh b ON b.TenBenh = src.TenBenh
JOIN TrieuChung t ON t.TenTrieuChung = src.TenTrieuChung;
GO

-- 5. THUỐC
INSERT INTO Thuoc (TenThuoc, HoatChat, NhomThuoc, DangBaoChe, CongDung, LieuDung, CachDung, TacDungPhu, LuuY, CanKeDon) VALUES  
    (N'Paracetamol 500mg', N'Paracetamol', N'Giảm đau hạ sốt', N'Viên nén', N'Giảm đau đầu và hạ sốt nhẹ đến trung bình do cảm cúm thông thường', N'500-1000 mg mỗi 4-6 giờ khi cần', N'Uống sau ăn với nhiều nước', N'Hiếm gặp; quá liều có thể gây tổn thương gan', N'Không dùng quá 4g/ngày', 0),
    (N'Ibuprofen 200mg', N'Ibuprofen', N'Kháng viêm không steroid (NSAID)', N'Viên nén', N'Giảm đau nhức cơ thể và hạ sốt kèm tác dụng kháng viêm', N'200-400 mg mỗi 6-8 giờ khi cần', N'Uống sau ăn no', N'Đau dạ dày, buồn nôn, ợ nóng', N'Tránh dùng khi đang đau dạ dày', 0),
    (N'Cetirizine 10mg', N'Cetirizine', N'Kháng histamin H1', N'Viên nén', N'Giảm sổ mũi hắt hơi và chảy nước mắt', N'10 mg uống 1 lần/ngày vào buổi tối', N'Uống với nước', N'Buồn ngủ nhẹ, khô miệng', N'Thận trọng khi lái xe', 0),
    (N'Dextromethorphan 15mg', N'Dextromethorphan', N'Giảm ho (tác động trung ương)', N'Siro', N'Giảm ho khan kéo dài do cảm cúm', N'10-15 ml mỗi 6-8 giờ khi cần', N'Uống trực tiếp hoặc pha với nước ấm', N'Buồn ngủ, chóng mặt', N'Không dùng cho ho có đờm nhiều', 0),
    (N'Vitamin C 500mg', N'Acid ascorbic', N'Vitamin và khoáng chất', N'Viên sủi', N'Hỗ trợ tăng cường đề kháng', N'500 mg uống 1 lần/ngày', N'Hoà tan viên sủi trong nước rồi uống', N'Rối loạn tiêu hoá nhẹ', N'Người có sỏi thận nên thận trọng', 0),
    (N'Ibuprofen 400mg', N'Ibuprofen', N'Kháng viêm không steroid (NSAID)', N'Viên nén', N'Giảm đau đầu mức độ trung bình', N'400 mg mỗi 6-8 giờ khi cần', N'Uống sau ăn no', N'Đau dạ dày, buồn nôn, ợ nóng', N'Tránh dùng khi đang đau dạ dày', 0),
    (N'Aspirin 500mg', N'Acid acetylsalicylic', N'Kháng viêm không steroid (NSAID)', N'Viên nén', N'Giảm đau đầu kèm tác dụng chống viêm nhẹ', N'500 mg mỗi 4-6 giờ khi cần', N'Uống sau ăn với nhiều nước', N'Kích ứng dạ dày', N'Không dùng cho trẻ em dưới 18 tuổi', 0),
    (N'Cafein kết hợp Paracetamol', N'Paracetamol và Cafein', N'Giảm đau hạ sốt phối hợp', N'Viên nén', N'Giảm đau đầu nhanh hơn nhờ cafein hỗ trợ', N'1 viên mỗi 6 giờ khi cần', N'Uống với nước, tránh dùng gần giờ ngủ', N'Mất ngủ, hồi hộp nếu dùng nhiều', N'Hạn chế cho người nhạy cảm cafein', 0),
    (N'Cao dán giảm đau Salonpas', N'Methyl salicylate và Menthol', N'Giảm đau tại chỗ', N'Cao dán', N'Giảm đau đầu do căng cơ vùng cổ vai gáy', N'Dán 1 miếng lên vùng đau, tối đa 2 lần/ngày', N'Dán trực tiếp lên da sạch', N'Kích ứng da nhẹ', N'Không dán lên vết thương hở', 0),
    (N'Oresol', N'Muối bù điện giải (ORS)', N'Bù nước điện giải', N'Bột pha dung dịch', N'Bù nước và điện giải đã mất do tiêu chảy', N'Pha 1 gói với 200ml nước, uống sau đi ngoài', N'Pha đúng tỉ lệ với nước sạch', N'Hiếm gặp', N'Tránh pha quá đặc', 0),
    (N'Loperamide 2mg', N'Loperamide', N'Giảm nhu động ruột', N'Viên nang', N'Giảm số lần đi ngoài', N'2 mg sau lần đi ngoài đầu tiên, tối đa 8mg/ngày', N'Uống với nước', N'Táo bón', N'Không dùng khi tiêu chảy có máu', 0),
    (N'Smecta (Diosmectite)', N'Diosmectite', N'Hấp thụ độc tố đường ruột', N'Bột pha hỗn dịch', N'Hấp thụ độc tố và bao phủ niêm mạc ruột', N'1 gói pha với nước, 2-3 lần/ngày', N'Uống xa các thuốc khác ít nhất 2 giờ', N'Táo bón nhẹ', N'Tránh làm giảm hấp thu thuốc khác', 0),
    (N'Men vi sinh Probiotic', N'Lactobacillus acidophilus', N'Cân bằng vi sinh đường ruột', N'Gói bột', N'Bổ sung lợi khuẩn', N'1 gói/lần, 2 lần/ngày', N'Pha với nước nguội', N'Đầy hơi nhẹ', N'Không pha với nước quá nóng', 0),
    (N'Berberin 50mg', N'Berberin clorid', N'Kháng khuẩn đường ruột nhẹ', N'Viên nén', N'Hỗ trợ giảm tiêu chảy do rối loạn tiêu hoá', N'2 viên mỗi 8 giờ', N'Uống trước ăn', N'Rối loạn tiêu hoá nhẹ', N'Không dùng cho phụ nữ có thai', 0),
    (N'Loratadine 10mg', N'Loratadine', N'Kháng histamin H1 thế hệ 2', N'Viên nén', N'Giảm ngứa và mề đay do dị ứng, ít buồn ngủ', N'10 mg uống 1 lần/ngày', N'Uống với nước', N'Khô miệng', N'Phù hợp dùng ban ngày', 0),
    (N'Chlorpheniramine 4mg', N'Chlorpheniramine maleate', N'Kháng histamin H1 thế hệ 1', N'Viên nén', N'Giảm nhanh ngứa và mề đay, gây buồn ngủ', N'4 mg mỗi 6-8 giờ khi cần', N'Tránh dùng khi cần tỉnh táo', N'Buồn ngủ nhiều', N'Không lái xe', 0),
    (N'Kem bôi Hydrocortisone 1%', N'Hydrocortisone', N'Kháng viêm corticoid tại chỗ', N'Kem bôi da', N'Giảm viêm và ngứa tại vùng da bị mề đay', N'Bôi lớp mỏng lên vùng da tổn thương, 1-2 lần/ngày', N'Bôi lên da sạch, không băng kín', N'Mỏng da', N'Không bôi lên vết thương hở', 0),
    (N'Fexofenadine 60mg', N'Fexofenadine', N'Kháng histamin H1 thế hệ 2', N'Viên nén', N'Giảm ngứa mề đay và sổ mũi, ít buồn ngủ', N'60 mg uống 1-2 lần/ngày', N'Uống với nước', N'Đau đầu nhẹ', N'Không dùng cùng nước cam', 0),
    (N'Calamine Lotion', N'Calamine và Kẽm oxit', N'Làm dịu da tại chỗ', N'Dung dịch bôi ngoài da', N'Làm dịu ngứa rát trên da', N'Lắc đều và bôi lên vùng da ngứa, 2-3 lần/ngày', N'Để khô tự nhiên', N'Khô da nhẹ', N'Tránh bôi gần mắt', 0),
    (N'Antacid (Nhôm hydroxit + Magie hydroxit)', N'Aluminium hydroxide và Magnesium hydroxide', N'Trung hoà acid dạ dày', N'Hỗn dịch uống', N'Giảm ợ chua và đau rát thượng vị tức thời', N'10ml sau ăn 1-2 giờ và trước khi ngủ', N'Lắc kỹ', N'Táo bón hoặc tiêu chảy', N'Uống cách thuốc khác', 0),
    (N'Omeprazole 20mg (OTC)', N'Omeprazole', N'Ức chế bơm proton (PPI)', N'Viên nang', N'Giảm tiết acid dạ dày kéo dài', N'20 mg uống 1 lần/ngày trước ăn sáng', N'Uống nguyên viên', N'Đau đầu', N'Không dùng quá 14 ngày tự ý', 0),
    (N'Simethicone 80mg', N'Simethicone', N'Chống đầy hơi', N'Viên nhai', N'Giảm đầy hơi chướng bụng', N'80 mg sau bữa ăn và trước khi ngủ', N'Nhai kỹ', N'An toàn', N'Dùng kết hợp với thuốc giảm acid khác được', 0),
    (N'Sucralfate 1g', N'Sucralfate', N'Bảo vệ niêm mạc dạ dày', N'Viên nén', N'Tạo lớp bảo vệ niêm mạc', N'1 g uống trước ăn 1 giờ, 2-4 lần/ngày', N'Uống với nước khi bụng đói', N'Táo bón', N'Không dùng cùng lúc thuốc kháng acid', 0),
    (N'Domperidone 10mg', N'Domperidone', N'Điều hoà vận động tiêu hoá', N'Viên nén', N'Giảm đầy bụng, buồn nôn', N'10 mg trước ăn 15-30 phút, 3 lần/ngày', N'Uống trước bữa ăn', N'Khô miệng', N'Không dùng quá 1 tuần liên tục', 0),
    (N'Viên ngậm sát khuẩn họng', N'Dichlorobenzyl alcohol và Amylmetacresol', N'Sát khuẩn họng tại chỗ', N'Viên ngậm', N'Làm dịu đau rát họng', N'Ngậm 1 viên mỗi 2-3 giờ', N'Ngậm tan chậm trong miệng', N'Kích ứng miệng nhẹ', N'Không dùng quá liều', 0),
    (N'Benzydamine xịt họng', N'Benzydamine', N'Kháng viêm giảm đau tại chỗ', N'Dung dịch xịt họng', N'Giảm đau rát họng, viêm họng nhẹ', N'Xịt 2-4 nhát/lần', N'Xịt trực tiếp vào vùng họng đau', N'Tê miệng', N'Đi khám nếu đau họng kéo dài', 0),
    (N'Nước muối súc họng 0.9%', N'Natri clorid', N'Vệ sinh họng miệng', N'Dung dịch súc họng', N'Hỗ trợ làm sạch họng', N'Súc họng 2-4 lần/ngày', N'Súc họng 20-30 giây', N'Hiếm gặp', N'Không thay thế điều trị nhiễm khuẩn', 0),
    (N'Xịt mũi nước muối biển', N'Natri clorid', N'Vệ sinh mũi', N'Dung dịch xịt mũi', N'Làm sạch dịch mũi', N'Xịt 1-2 nhát mỗi bên mũi', N'Lau sạch dịch sau khi xịt', N'Kích ứng mũi nhẹ', N'Dùng riêng chai xịt', 0),
    (N'Xylometazoline 0.05% xịt mũi', N'Xylometazoline', N'Co mạch mũi', N'Dung dịch xịt mũi', N'Giảm nghẹt mũi nhanh', N'Xịt 1 nhát mỗi bên mũi', N'Tránh xịt liên tục kéo dài', N'Khô mũi', N'Không lạm dụng', 0),
    (N'Lactulose siro', N'Lactulose', N'Nhuận tràng thẩm thấu', N'Siro uống', N'Làm mềm phân và hỗ trợ đi tiêu', N'15-30 ml/ngày', N'Nên uống đủ nước trong ngày', N'Đầy hơi', N'Không dùng khi đau bụng chưa rõ nguyên nhân', 0),
    (N'Psyllium husk', N'Chất xơ psyllium', N'Bổ sung chất xơ', N'Bột pha uống', N'Tăng lượng chất xơ', N'1 gói/lần, 1-2 lần/ngày', N'Pha với nhiều nước', N'Đầy hơi nhẹ', N'Uống thêm nhiều nước', 0),
    (N'Bisacodyl 5mg', N'Bisacodyl', N'Nhuận tràng kích thích', N'Viên bao tan trong ruột', N'Kích thích nhu động ruột', N'5-10 mg buổi tối', N'Nuốt nguyên viên', N'Đau quặn bụng', N'Không dùng kéo dài', 0),
    (N'Alginate hỗn dịch', N'Sodium alginate và antacid', N'Chống trào ngược', N'Hỗn dịch uống', N'Giảm ợ chua và nóng rát', N'10-20 ml sau ăn', N'Lắc kỹ', N'Đầy bụng', N'Kiểm tra thành phần natri', 0),
    (N'Famotidine 20mg', N'Famotidine', N'Kháng H2 giảm tiết acid', N'Viên nén', N'Giảm tiết acid dạ dày', N'20 mg khi có triệu chứng', N'Uống với nước', N'Đau đầu', N'Người suy thận cần lưu ý', 0);
GO

-- 6. THÀNH PHẦN THUỐC
INSERT INTO ThanhPhan (TenThanhPhan) VALUES
    (N'Paracetamol'), (N'Ibuprofen'), (N'Cetirizine'), (N'Dextromethorphan'),
    (N'Acid ascorbic'), (N'Acid acetylsalicylic'), (N'Cafein'), (N'Methyl salicylate'),
    (N'Menthol'), (N'Muối bù điện giải ORS'), (N'Loperamide'), (N'Diosmectite'),
    (N'Lactobacillus acidophilus'), (N'Berberin clorid'), (N'Loratadine'),
    (N'Chlorpheniramine maleate'), (N'Hydrocortisone'), (N'Fexofenadine'),
    (N'Calamine'), (N'Kẽm oxit'), (N'Aluminium hydroxide'), (N'Magnesium hydroxide'),
    (N'Omeprazole'), (N'Simethicone'), (N'Sucralfate'), (N'Domperidone'),
    (N'Dichlorobenzyl alcohol'), (N'Amylmetacresol'), (N'Benzydamine'), (N'Natri clorid'),
    (N'Xylometazoline'), (N'Lactulose'), (N'Psyllium'), (N'Bisacodyl'),
    (N'Sodium alginate'), (N'Famotidine');
GO

INSERT INTO ThuocThanhPhan (MaThuoc, MaThanhPhan)
SELECT t.MaThuoc, tp.MaThanhPhan
FROM (VALUES
    (N'Paracetamol 500mg', N'Paracetamol'), (N'Ibuprofen 200mg', N'Ibuprofen'),
    (N'Cetirizine 10mg', N'Cetirizine'), (N'Dextromethorphan 15mg', N'Dextromethorphan'),
    (N'Vitamin C 500mg', N'Acid ascorbic'), (N'Ibuprofen 400mg', N'Ibuprofen'),
    (N'Aspirin 500mg', N'Acid acetylsalicylic'), (N'Cafein kết hợp Paracetamol', N'Paracetamol'),
    (N'Cafein kết hợp Paracetamol', N'Cafein'), (N'Cao dán giảm đau Salonpas', N'Methyl salicylate'),
    (N'Cao dán giảm đau Salonpas', N'Menthol'), (N'Oresol', N'Muối bù điện giải ORS'),
    (N'Loperamide 2mg', N'Loperamide'), (N'Smecta (Diosmectite)', N'Diosmectite'),
    (N'Men vi sinh Probiotic', N'Lactobacillus acidophilus'), (N'Berberin 50mg', N'Berberin clorid'),
    (N'Loratadine 10mg', N'Loratadine'), (N'Chlorpheniramine 4mg', N'Chlorpheniramine maleate'),
    (N'Kem bôi Hydrocortisone 1%', N'Hydrocortisone'), (N'Fexofenadine 60mg', N'Fexofenadine'),
    (N'Calamine Lotion', N'Calamine'), (N'Calamine Lotion', N'Kẽm oxit'),
    (N'Antacid (Nhôm hydroxit + Magie hydroxit)', N'Aluminium hydroxide'),
    (N'Antacid (Nhôm hydroxit + Magie hydroxit)', N'Magnesium hydroxide'),
    (N'Omeprazole 20mg (OTC)', N'Omeprazole'), (N'Simethicone 80mg', N'Simethicone'),
    (N'Sucralfate 1g', N'Sucralfate'), (N'Domperidone 10mg', N'Domperidone'),
    (N'Viên ngậm sát khuẩn họng', N'Dichlorobenzyl alcohol'), (N'Viên ngậm sát khuẩn họng', N'Amylmetacresol'),
    (N'Benzydamine xịt họng', N'Benzydamine'), (N'Nước muối súc họng 0.9%', N'Natri clorid'),
    (N'Xịt mũi nước muối biển', N'Natri clorid'), (N'Xylometazoline 0.05% xịt mũi', N'Xylometazoline'),
    (N'Lactulose siro', N'Lactulose'), (N'Psyllium husk', N'Psyllium'),
    (N'Bisacodyl 5mg', N'Bisacodyl'), (N'Alginate hỗn dịch', N'Sodium alginate'),
    (N'Alginate hỗn dịch', N'Aluminium hydroxide'), (N'Alginate hỗn dịch', N'Magnesium hydroxide'),
    (N'Famotidine 20mg', N'Famotidine')
) AS src(TenThuoc, TenThanhPhan)
JOIN Thuoc t ON t.TenThuoc = src.TenThuoc
JOIN ThanhPhan tp ON tp.TenThanhPhan = src.TenThanhPhan;
GO

-- 7. BỆNH - THUỐC
INSERT INTO BenhThuoc (MaBenh, MaThuoc, DoUuTien, LoaiDieuTri)
SELECT b.MaBenh, t.MaThuoc, src.DoUuTien, src.LoaiDieuTri
FROM (VALUES
    (N'Cảm cúm', N'Paracetamol 500mg', 1, 'primary'), (N'Cảm cúm', N'Ibuprofen 200mg', 2, 'primary'),
    (N'Cảm cúm', N'Cetirizine 10mg', 2, 'secondary'), (N'Cảm cúm', N'Dextromethorphan 15mg', 2, 'secondary'),
    (N'Cảm cúm', N'Vitamin C 500mg', 3, 'alternative'),
    (N'Đau đầu', N'Paracetamol 500mg', 1, 'primary'), (N'Đau đầu', N'Ibuprofen 400mg', 1, 'primary'),
    (N'Đau đầu', N'Aspirin 500mg', 2, 'secondary'), (N'Đau đầu', N'Cafein kết hợp Paracetamol', 2, 'secondary'),
    (N'Đau đầu', N'Cao dán giảm đau Salonpas', 3, 'alternative'),
    (N'Tiêu chảy', N'Oresol', 1, 'primary'), (N'Tiêu chảy', N'Loperamide 2mg', 1, 'primary'),
    (N'Tiêu chảy', N'Smecta (Diosmectite)', 2, 'secondary'), (N'Tiêu chảy', N'Men vi sinh Probiotic', 2, 'secondary'),
    (N'Tiêu chảy', N'Berberin 50mg', 3, 'alternative'),
    (N'Dị ứng / Mề đay', N'Loratadine 10mg', 1, 'primary'), (N'Dị ứng / Mề đay', N'Chlorpheniramine 4mg', 1, 'primary'),
    (N'Dị ứng / Mề đay', N'Fexofenadine 60mg', 2, 'secondary'), (N'Dị ứng / Mề đay', N'Kem bôi Hydrocortisone 1%', 2, 'secondary'),
    (N'Dị ứng / Mề đay', N'Calamine Lotion', 3, 'alternative'),
    (N'Đau dạ dày', N'Antacid (Nhôm hydroxit + Magie hydroxit)', 1, 'primary'), (N'Đau dạ dày', N'Omeprazole 20mg (OTC)', 1, 'primary'),
    (N'Đau dạ dày', N'Simethicone 80mg', 2, 'secondary'), (N'Đau dạ dày', N'Sucralfate 1g', 2, 'secondary'),
    (N'Đau dạ dày', N'Domperidone 10mg', 3, 'alternative'),
    (N'Viêm họng', N'Viên ngậm sát khuẩn họng', 1, 'primary'), (N'Viêm họng', N'Benzydamine xịt họng', 1, 'primary'),
    (N'Viêm họng', N'Nước muối súc họng 0.9%', 2, 'secondary'), (N'Viêm họng', N'Dextromethorphan 15mg', 3, 'alternative'),
    (N'Viêm mũi dị ứng', N'Xịt mũi nước muối biển', 1, 'primary'), (N'Viêm mũi dị ứng', N'Cetirizine 10mg', 1, 'primary'),
    (N'Viêm mũi dị ứng', N'Loratadine 10mg', 1, 'primary'), (N'Viêm mũi dị ứng', N'Fexofenadine 60mg', 2, 'secondary'),
    (N'Viêm mũi dị ứng', N'Xylometazoline 0.05% xịt mũi', 3, 'alternative'),
    (N'Táo bón', N'Lactulose siro', 1, 'primary'), (N'Táo bón', N'Psyllium husk', 1, 'primary'),
    (N'Táo bón', N'Bisacodyl 5mg', 2, 'secondary'), (N'Táo bón', N'Simethicone 80mg', 3, 'alternative'),
    (N'Trào ngược dạ dày thực quản', N'Alginate hỗn dịch', 1, 'primary'), (N'Trào ngược dạ dày thực quản', N'Famotidine 20mg', 1, 'primary'),
    (N'Trào ngược dạ dày thực quản', N'Omeprazole 20mg (OTC)', 1, 'primary'), (N'Trào ngược dạ dày thực quản', N'Antacid (Nhôm hydroxit + Magie hydroxit)', 2, 'secondary'),
    (N'Trào ngược dạ dày thực quản', N'Sucralfate 1g', 3, 'alternative')
) AS src(TenBenh, TenThuoc, DoUuTien, LoaiDieuTri)
JOIN Benh b ON b.TenBenh = src.TenBenh
JOIN Thuoc t ON t.TenThuoc = src.TenThuoc;
GO

-- 8. DỊ ỨNG & CẢNH BÁO DỊ ỨNG THUỐC
INSERT INTO DiUng (TenDiUng) VALUES 
    (N'Dị ứng Penicillin'), (N'Dị ứng Aspirin / NSAID'), (N'Dị ứng Sulfonamide'),
    (N'Dị ứng phấn hoa'), (N'Dị ứng bụi nhà'), (N'Dị ứng hải sản'),
    (N'Dị ứng đậu phộng'), (N'Dị ứng Latex');
GO

INSERT INTO CanhBaoDiUngThuoc (MaThuoc, MaDiUng, NoiDung)
SELECT t.MaThuoc, d.MaDiUng, src.NoiDung
FROM (VALUES
    (N'Aspirin 500mg', N'Dị ứng Aspirin / NSAID', N'Thuốc chứa Aspirin. Không dùng nếu bạn có tiền sử dị ứng với Aspirin hoặc NSAID.'),
    (N'Ibuprofen 200mg', N'Dị ứng Aspirin / NSAID', N'Ibuprofen thuộc nhóm NSAID. Có thể gây phản ứng chéo với người dị ứng Aspirin.'),
    (N'Ibuprofen 400mg', N'Dị ứng Aspirin / NSAID', N'Ibuprofen thuộc nhóm NSAID. Có thể gây phản ứng chéo với người dị ứng Aspirin.'),
    (N'Cetirizine 10mg', N'Dị ứng hải sản', N'Một số người dị ứng hải sản có thể nhạy cảm với Cetirizine. Hỏi dược sĩ trước khi dùng.'),
    (N'Loratadine 10mg', N'Dị ứng phấn hoa', N'Loratadine thường dùng cho dị ứng phấn hoa nhưng cần điều chỉnh liều nếu phản ứng nặng.'),
    (N'Fexofenadine 60mg', N'Dị ứng phấn hoa', N'Fexofenadine hiệu quả với dị ứng phấn hoa, theo dõi nếu triệu chứng không cải thiện sau 3 ngày.'),
    (N'Viên ngậm sát khuẩn họng', N'Dị ứng Latex', N'Người có cơ địa dị ứng hoặc kích ứng niêm mạc nên ngưng dùng nếu thấy sưng ngứa.'),
    (N'Benzydamine xịt họng', N'Dị ứng Aspirin / NSAID', N'Người từng dị ứng thuốc giảm đau kháng viêm nên thận trọng.'),
    (N'Xịt mũi nước muối biển', N'Dị ứng bụi nhà', N'Có thể hỗ trợ rửa dị nguyên trong mũi nhưng không thay thế thuốc.'),
    (N'Xylometazoline 0.05% xịt mũi', N'Dị ứng phấn hoa', N'Chỉ giúp giảm nghẹt mũi tạm thời, không điều trị nền dị ứng.'),
    (N'Cao dán giảm đau Salonpas', N'Dị ứng Aspirin / NSAID', N'Sản phẩm có methyl salicylate. Thử trên vùng da nhỏ trước khi dùng.'),
    (N'Calamine Lotion', N'Dị ứng Latex', N'Người dị ứng Latex hoặc da quá nhạy cảm nên thử trên vùng da nhỏ trước khi bôi rộng.'),
    (N'Chlorpheniramine 4mg', N'Dị ứng phấn hoa', N'Có thể giúp giảm triệu chứng dị ứng nhưng gây buồn ngủ.')
) AS src(TenThuoc, TenDiUng, NoiDung)
JOIN Thuoc t ON t.TenThuoc = src.TenThuoc
JOIN DiUng d ON d.TenDiUng = src.TenDiUng;
GO

-- 9. BỆNH NỀN & CẢNH BÁO BỆNH NỀN THUỐC
INSERT INTO BenhNen (TenBenhNen) VALUES 
    (N'Viêm loét dạ dày tá tràng'), (N'Suy thận mạn'), (N'Suy gan'), (N'Tiểu đường'), (N'Tăng huyết áp');
GO

INSERT INTO CanhBaoBenhNenThuoc (MaThuoc, MaBenhNen, NoiDung)
SELECT t.MaThuoc, bn.MaBenhNen, src.NoiDung
FROM (VALUES
    (N'Aspirin 500mg', N'Viêm loét dạ dày tá tràng', N'Aspirin kích ứng mạnh niêm mạc dạ dày. Không dùng khi đang viêm loét dạ dày.'),
    (N'Aspirin 500mg', N'Suy thận mạn', N'Aspirin tích lũy ở người suy thận, tăng nguy cơ chảy máu. Hỏi bác sĩ trước khi dùng.'),
    (N'Ibuprofen 200mg', N'Viêm loét dạ dày tá tràng', N'Ibuprofen có thể làm nặng thêm loét dạ dày. Uống sau ăn, kết hợp thuốc bảo vệ dạ dày nếu cần.'),
    (N'Ibuprofen 200mg', N'Suy thận mạn', N'NSAID giảm lưu lượng máu thận, nguy hiểm với người suy thận. Dùng Paracetamol thay thế.'),
    (N'Ibuprofen 200mg', N'Tăng huyết áp', N'NSAID có thể làm tăng huyết áp và giảm hiệu quả thuốc hạ áp. Thận trọng khi dùng.'),
    (N'Ibuprofen 400mg', N'Viêm loét dạ dày tá tràng', N'Ibuprofen có thể làm nặng thêm loét dạ dày.'),
    (N'Ibuprofen 400mg', N'Suy thận mạn', N'NSAID giảm lưu lượng máu thận, nguy hiểm với người suy thận.'),
    (N'Ibuprofen 400mg', N'Tăng huyết áp', N'NSAID có thể làm tăng huyết áp và giảm hiệu quả thuốc hạ áp.'),
    (N'Paracetamol 500mg', N'Suy gan', N'Paracetamol chuyển hóa qua gan. Người suy gan cần giảm liều hoặc hỏi bác sĩ.'),
    (N'Omeprazole 20mg (OTC)', N'Suy gan', N'Omeprazole chuyển hóa qua gan. Người suy gan cần giảm liều, hỏi bác sĩ.'),
    (N'Domperidone 10mg', N'Suy gan', N'Domperidone chuyển hóa qua gan. Không dùng cho người suy gan nặng.'),
    (N'Domperidone 10mg', N'Tăng huyết áp', N'Domperidone có thể ảnh hưởng nhịp tim, thận trọng với người có bệnh tim mạch.'),
    (N'Chlorpheniramine 4mg', N'Tiểu đường', N'Một số dạng bào chế Chlorpheniramine có đường. Chọn dạng không đường.'),
    (N'Chlorpheniramine 4mg', N'Tăng huyết áp', N'Thận trọng khi dùng kháng histamin thế hệ 1 với người tăng huyết áp.'),
    (N'Sucralfate 1g', N'Suy thận mạn', N'Sucralfate chứa nhôm, người suy thận có thể tích lũy nhôm. Hỏi bác sĩ trước khi dùng.'),
    (N'Xylometazoline 0.05% xịt mũi', N'Tăng huyết áp', N'Thuốc co mạch mũi có thể làm tăng huyết áp hoặc gây hồi hộp ở người nhạy cảm. Không dùng kéo dài.'),
    (N'Xylometazoline 0.05% xịt mũi', N'Suy gan', N'Thận trọng nếu có bệnh gan nặng hoặc đang dùng nhiều thuốc khác; hỏi dược sĩ nếu cần dùng quá vài ngày.'),
    (N'Lactulose siro', N'Tiểu đường', N'Lactulose là đường tổng hợp; người tiểu đường nên theo dõi đường huyết và hỏi ý kiến nhân viên y tế nếu dùng thường xuyên.'),
    (N'Bisacodyl 5mg', N'Viêm loét dạ dày tá tràng', N'Không dùng nếu đau bụng cấp, nôn ói hoặc nghi tắc ruột. Cần đi khám nếu đau bụng dữ dội.'),
    (N'Famotidine 20mg', N'Suy thận mạn', N'Famotidine thải trừ qua thận; người suy thận có thể cần giảm liều. Hỏi bác sĩ hoặc dược sĩ trước khi dùng.'),
    (N'Alginate hỗn dịch', N'Tăng huyết áp', N'Một số chế phẩm alginate có natri. Người tăng huyết áp cần kiểm tra hàm lượng natri trên nhãn.'),
    (N'Berberin 50mg', N'Suy gan', N'Berberin có thể không phù hợp với người có bệnh gan nặng.'),
    (N'Cafein kết hợp Paracetamol', N'Suy gan', N'Paracetamol có nguy cơ gây độc gan khi dùng quá liều hoặc dùng chung với rượu.'),
    (N'Cafein kết hợp Paracetamol', N'Tăng huyết áp', N'Cafein có thể làm hồi hộp, mất ngủ hoặc tăng huyết áp ở người nhạy cảm.'),
    (N'Dextromethorphan 15mg', N'Suy gan', N'Dextromethorphan được chuyển hóa qua gan. Người suy gan nên thận trọng.'),
    (N'Kem bôi Hydrocortisone 1%', N'Tiểu đường', N'Corticosteroid bôi ngoài da khi dùng kéo dài/bôi diện rộng có thể ảnh hưởng kiểm soát đường huyết.'),
    (N'Men vi sinh Probiotic', N'Tiểu đường', N'Một số chế phẩm probiotic dạng gói có thể có đường hoặc tá dược tạo ngọt.'),
    (N'Nước muối súc họng 0.9%', N'Tăng huyết áp', N'Không nên nuốt lượng lớn dung dịch nước muối.'),
    (N'Psyllium husk', N'Tiểu đường', N'Psyllium có thể ảnh hưởng hấp thu đường và một số thuốc uống.'),
    (N'Smecta (Diosmectite)', N'Tiểu đường', N'Một số chế phẩm diosmectite có thể chứa glucose/sucrose.'),
    (N'Vitamin C 500mg', N'Suy thận mạn', N'Vitamin C liều cao có thể làm tăng nguy cơ sỏi thận ở người suy thận.'),
    (N'Oresol', N'Tăng huyết áp', N'Một số dung dịch bù điện giải có natri. Người tăng huyết áp cần pha đúng tỉ lệ.'),
    (N'Loperamide 2mg', N'Suy gan', N'Loperamide chuyển hóa qua gan. Người suy gan cần thận trọng.'),
    (N'Antacid (Nhôm hydroxit + Magie hydroxit)', N'Suy thận mạn', N'Antacid chứa nhôm/magie có thể tích lũy ở người suy thận.'),
    (N'Simethicone 80mg', N'Viêm loét dạ dày tá tràng', N'Simethicone thường an toàn nhưng không xử lý nguyên nhân loét. Cần đi khám nếu đau rát kéo dài.')
) AS src(TenThuoc, TenBenhNen, NoiDung)
JOIN Thuoc t ON t.TenThuoc = src.TenThuoc
JOIN BenhNen bn ON bn.TenBenhNen = src.TenBenhNen;
GO

-- 10. TƯƠNG TÁC THUỐC
INSERT INTO TuongTacThuoc (MaThuoc1, MaThuoc2, MucDoNghiemTrong, MoTa)
SELECT 
    CASE WHEN t1.MaThuoc < t2.MaThuoc THEN t1.MaThuoc ELSE t2.MaThuoc END,
    CASE WHEN t1.MaThuoc < t2.MaThuoc THEN t2.MaThuoc ELSE t1.MaThuoc END,
    src.MucDo, src.MoTa
FROM (VALUES
    (N'Aspirin 500mg', N'Ibuprofen 200mg', 2, N'Hai thuốc cùng nhóm NSAID, dùng chung tăng nguy cơ chảy máu tiêu hóa và tổn thương thận.'),
    (N'Aspirin 500mg', N'Ibuprofen 400mg', 2, N'Hai thuốc cùng nhóm NSAID, dùng chung tăng nguy cơ chảy máu.'),
    (N'Ibuprofen 200mg', N'Ibuprofen 400mg', 3, N'Không dùng hai dạng liều Ibuprofen cùng lúc, nguy cơ quá liều nghiêm trọng.'),
    (N'Aspirin 500mg', N'Sucralfate 1g', 1, N'Sucralfate giảm hấp thu Aspirin. Uống cách nhau ít nhất 2 giờ.'),
    (N'Omeprazole 20mg (OTC)', N'Sucralfate 1g', 1, N'Omeprazole giảm acid có thể làm giảm hiệu quả Sucralfate. Uống cách nhau ít nhất 2 giờ.'),
    (N'Smecta (Diosmectite)', N'Loperamide 2mg', 1, N'Smecta có thể hấp phụ Loperamide, làm giảm hiệu quả.'),
    (N'Smecta (Diosmectite)', N'Berberin 50mg', 1, N'Smecta có thể hấp phụ Berberin, làm giảm hiệu quả.'),
    (N'Chlorpheniramine 4mg', N'Dextromethorphan 15mg', 1, N'Cả hai đều gây buồn ngủ và ức chế thần kinh trung ương.'),
    (N'Cetirizine 10mg', N'Chlorpheniramine 4mg', 2, N'Hai kháng histamin dùng chung không tăng hiệu quả mà tăng tác dụng phụ.'),
    (N'Loratadine 10mg', N'Cetirizine 10mg', 2, N'Hai kháng histamin dùng chung không tăng hiệu quả mà tăng tác dụng phụ.'),
    (N'Antacid (Nhôm hydroxit + Magie hydroxit)', N'Omeprazole 20mg (OTC)', 1, N'Antacid giảm hấp thu Omeprazole nếu dùng cùng lúc.'),
    (N'Xylometazoline 0.05% xịt mũi', N'Cetirizine 10mg', 1, N'Dùng chung thường được nhưng cần theo dõi khô mũi, khô miệng hoặc khó chịu tăng lên.'),
    (N'Bisacodyl 5mg', N'Lactulose siro', 2, N'Dùng chung hai thuốc nhuận tràng có thể gây đau quặn bụng hoặc tiêu chảy.'),
    (N'Famotidine 20mg', N'Antacid (Nhôm hydroxit + Magie hydroxit)', 1, N'Antacid có thể ảnh hưởng hấp thu một số thuốc. Nên uống cách Famotidine ít nhất 1-2 giờ.'),
    (N'Alginate hỗn dịch', N'Omeprazole 20mg (OTC)', 1, N'Có thể phối hợp trong trào ngược nhưng nên dùng đúng thời điểm: Omeprazole trước ăn, alginate sau ăn.')
) AS src(TenThuoc1, TenThuoc2, MucDo, MoTa)
JOIN Thuoc t1 ON t1.TenThuoc = src.TenThuoc1
JOIN Thuoc t2 ON t2.TenThuoc = src.TenThuoc2;
GO