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
    BiKhoa          BIT             DEFAULT 0,      -- khóa tạm thời
    DeleteAt        DATETIME        NULL,           -- NULL = chưa xóa, có giá trị = đã xóa mềm
    NgayTao         DATETIME        DEFAULT GETDATE(),

    CONSTRAINT CK_NguoiDung_VaiTro CHECK (VaiTro IN ('User', 'Admin'))
);
GO

-- =========================
-- 2. BẢNG BỆNH
-- Xóa mềm: DangHoatDong + DeleteAt (có FK từ LichSuDuDoan)
-- Thêm: NhomBenh, MucDoNghiemTrong để hỗ trợ ML scoring
-- =========================
CREATE TABLE Benh (
    MaBenh              INT IDENTITY(1,1) PRIMARY KEY,
    TenBenh             NVARCHAR(150)   NOT NULL,
    MoTa                NVARCHAR(MAX),
    NhomBenh            NVARCHAR(100),              -- hô hấp, tiêu hoá, tim mạch...
    MucDoNghiemTrong    INT             DEFAULT 1,  -- 1: nhẹ, 2: trung bình, 3: nặng (ảnh hưởng scoring)
    DangHoatDong        BIT             DEFAULT 1,
    DeleteAt            DATETIME        NULL,
    NgayTao             DATETIME        DEFAULT GETDATE(),
    NgayCapNhat         DATETIME        NULL,       -- track khi nào data thay đổi (quan trọng cho embedding)

    CONSTRAINT CK_Benh_MucDoNghiemTrong CHECK (MucDoNghiemTrong BETWEEN 1 AND 3)
);
GO

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

-- =========================
-- 5. BẢNG THUỐC
-- Xóa mềm: DangHoatDong (có FK từ KetQuaDuDoan)
-- Thêm: NhomThuoc, DangBaoChe, CanKeDon, NgayCapNhat
-- CanKeDon quan trọng: hệ thống chỉ gợi ý thuốc OTC (CanKeDon = 0)
-- NgayCapNhat để biết embedding có bị stale không
-- =========================
CREATE TABLE Thuoc (
    MaThuoc         INT IDENTITY(1,1) PRIMARY KEY,
    TenThuoc        NVARCHAR(150)   NOT NULL,
    HoatChat        NVARCHAR(255),
    NhomThuoc       NVARCHAR(100),              -- kháng sinh, giảm đau, hạ sốt, kháng viêm...
    DangBaoChe      NVARCHAR(100),              -- viên nén, siro, bột, tiêm...
    CongDung        NVARCHAR(MAX),
    LieuDung        NVARCHAR(MAX),
    CachDung        NVARCHAR(MAX),
    TacDungPhu      NVARCHAR(MAX),
    LuuY            NVARCHAR(MAX),
    CanKeDon        BIT             DEFAULT 0,  -- 0: OTC (tự mua), 1: cần kê đơn bác sĩ
    DangHoatDong    BIT             DEFAULT 1,
    NgayTao         DATETIME        DEFAULT GETDATE(),
    NgayCapNhat     DATETIME        NULL        -- cập nhật khi sửa thông tin → cần re-embed
);
GO

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
-- Thêm: DoUuTien, LoaiDieuTri để hỗ trợ ML reranking
-- =========================
CREATE TABLE BenhThuoc (
    MaBenh          INT,
    MaThuoc         INT,
    DoUuTien        INT             DEFAULT 1,          -- thuốc nào ưu tiên hơn cho bệnh này (dùng trong scoring)
    LoaiDieuTri     VARCHAR(20)     DEFAULT 'primary',  -- primary: đầu tay, secondary: thay thế

    PRIMARY KEY (MaBenh, MaThuoc),
    FOREIGN KEY (MaBenh)    REFERENCES Benh(MaBenh),
    FOREIGN KEY (MaThuoc)   REFERENCES Thuoc(MaThuoc),

    CONSTRAINT CK_BenhThuoc_LoaiDieuTri CHECK (LoaiDieuTri IN ('primary', 'secondary', 'alternative'))
);
GO

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

-- =========================
-- 9. CẢNH BÁO DỊ ỨNG THUỐC
-- Xóa cứng: mapping table
-- Scope tham khảo: chỉ hiển thị cảnh báo, không block kết quả
-- =========================
CREATE TABLE CanhBaoDiUngThuoc (
    MaThuoc     INT,
    MaDiUng     INT,
    NoiDung     NVARCHAR(MAX),  -- nội dung cảnh báo hiển thị cho user

    PRIMARY KEY (MaThuoc, MaDiUng),
    FOREIGN KEY (MaThuoc)   REFERENCES Thuoc(MaThuoc),
    FOREIGN KEY (MaDiUng)   REFERENCES DiUng(MaDiUng)
);
GO

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

-- =========================
-- 11. CẢNH BÁO BỆNH NỀN
-- Xóa cứng: mapping table
-- Scope tham khảo: chỉ hiển thị cảnh báo, không block kết quả
-- =========================
CREATE TABLE CanhBaoBenhNenThuoc (
    MaThuoc     INT,
    MaBenhNen   INT,
    NoiDung     NVARCHAR(MAX),  -- nội dung cảnh báo hiển thị cho user

    PRIMARY KEY (MaThuoc, MaBenhNen),
    FOREIGN KEY (MaThuoc)   REFERENCES Thuoc(MaThuoc),
    FOREIGN KEY (MaBenhNen) REFERENCES BenhNen(MaBenhNen)
);
GO

-- =========================
-- 12. TƯƠNG TÁC THUỐC (TABLE MỚI)
-- Xóa cứng: mapping table
-- Scope tham khảo: hiển thị cảnh báo khi user đang dùng nhiều thuốc cùng lúc
-- Không block kết quả, chỉ warn
-- =========================
CREATE TABLE TuongTacThuoc (
    MaThuoc1            INT,
    MaThuoc2            INT,
    MucDoNghiemTrong    INT             NOT NULL,   -- 1: nhẹ, 2: trung bình, 3: nghiêm trọng
    MoTa                NVARCHAR(MAX),              -- mô tả tương tác để hiển thị cho user tham khảo

    PRIMARY KEY (MaThuoc1, MaThuoc2),
    FOREIGN KEY (MaThuoc1) REFERENCES Thuoc(MaThuoc),
    FOREIGN KEY (MaThuoc2) REFERENCES Thuoc(MaThuoc),

    CONSTRAINT CK_TuongTacThuoc_MaThuoc     CHECK (MaThuoc1 < MaThuoc2),       -- tránh nhập trùng (A,B) và (B,A)
    CONSTRAINT CK_TuongTacThuoc_MucDo       CHECK (MucDoNghiemTrong BETWEEN 1 AND 3)
);
GO

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
    HuuIch          BIT             NOT NULL,   -- 1: hữu ích, 0: không hữu ích
    GhiChu          NVARCHAR(MAX),              -- user có thể ghi thêm lý do (optional)
    NgayTao         DATETIME        DEFAULT GETDATE(),

    FOREIGN KEY (MaKetQua)      REFERENCES KetQuaDuDoan(MaKetQua),
    FOREIGN KEY (MaNguoiDung)   REFERENCES NguoiDung(MaNguoiDung),

    CONSTRAINT UQ_DanhGia_KetQua_NguoiDung UNIQUE (MaKetQua, MaNguoiDung)  -- mỗi user chỉ đánh giá 1 lần / kết quả
);
GO

-- =========================
-- DỮ LIỆU MẪU ADMIN
-- =========================
INSERT INTO NguoiDung (HoTen, Email, MatKhauMaHoa, VaiTro)
VALUES (N'Quản trị viên', 'admin@example.com', 'hashed_password', 'Admin');
GO