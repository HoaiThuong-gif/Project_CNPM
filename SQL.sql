CREATE DATABASE WebsiteDuDoanThuoc;
GO

USE WebsiteDuDoanThuoc;
GO

-- =========================
-- 1. BẢNG NGƯỜI DÙNG
-- =========================
CREATE TABLE NguoiDung (
    MaNguoiDung INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    MatKhauMaHoa VARCHAR(255) NOT NULL,
    SoDienThoai VARCHAR(20),
    GioiTinh NVARCHAR(10),
    NgaySinh DATE,
    VaiTro VARCHAR(20) DEFAULT 'User',
    BiKhoa BIT DEFAULT 0,
    NgayTao DATETIME DEFAULT GETDATE(),

    CONSTRAINT CK_NguoiDung_VaiTro CHECK (VaiTro IN ('User','Admin'))
);
GO

-- =========================
-- 2. BẢNG BỆNH
-- =========================
CREATE TABLE Benh (
    MaBenh INT IDENTITY(1,1) PRIMARY KEY,
    TenBenh NVARCHAR(150) NOT NULL,
    MoTa NVARCHAR(MAX),
    DangHoatDong BIT DEFAULT 1,
    NgayTao DATETIME DEFAULT GETDATE()
);
GO

-- =========================
-- 3. BẢNG TRIỆU CHỨNG
-- =========================
CREATE TABLE TrieuChung (
    MaTrieuChung INT IDENTITY(1,1) PRIMARY KEY,
    TenTrieuChung NVARCHAR(150) NOT NULL,
    MoTa NVARCHAR(MAX),
    NgayTao DATETIME DEFAULT GETDATE()
);
GO

-- =========================
-- 4. BỆNH - TRIỆU CHỨNG (CÓ TRỌNG SỐ)
-- =========================
CREATE TABLE BenhTrieuChung (
    MaBenh INT,
    MaTrieuChung INT,
    TrongSo INT DEFAULT 1,

    PRIMARY KEY (MaBenh, MaTrieuChung),

    FOREIGN KEY (MaBenh) REFERENCES Benh(MaBenh),
    FOREIGN KEY (MaTrieuChung) REFERENCES TrieuChung(MaTrieuChung),

    CONSTRAINT CK_BenhTrieuChung_TrongSo CHECK (TrongSo BETWEEN 1 AND 5)
);
GO

-- =========================
-- 5. BẢNG THUỐC
-- =========================
CREATE TABLE Thuoc (
    MaThuoc INT IDENTITY(1,1) PRIMARY KEY,
    TenThuoc NVARCHAR(150) NOT NULL,
    HoatChat NVARCHAR(255),
    CongDung NVARCHAR(MAX),
    LieuDung NVARCHAR(MAX),
    CachDung NVARCHAR(MAX),
    TacDungPhu NVARCHAR(MAX),
    LuuY NVARCHAR(MAX),
    DangHoatDong BIT DEFAULT 1,
    NgayTao DATETIME DEFAULT GETDATE()
);
GO

-- =========================
-- 6. THÀNH PHẦN THUỐC (CHUẨN HOÁ)
-- =========================
CREATE TABLE ThanhPhan (
    MaThanhPhan INT IDENTITY(1,1) PRIMARY KEY,
    TenThanhPhan NVARCHAR(150) NOT NULL
);
GO

CREATE TABLE ThuocThanhPhan (
    MaThuoc INT,
    MaThanhPhan INT,

    PRIMARY KEY (MaThuoc, MaThanhPhan),

    FOREIGN KEY (MaThuoc) REFERENCES Thuoc(MaThuoc),
    FOREIGN KEY (MaThanhPhan) REFERENCES ThanhPhan(MaThanhPhan)
);
GO

-- =========================
-- 7. BỆNH - THUỐC
-- =========================
CREATE TABLE BenhThuoc (
    MaBenh INT,
    MaThuoc INT,

    PRIMARY KEY (MaBenh, MaThuoc),

    FOREIGN KEY (MaBenh) REFERENCES Benh(MaBenh),
    FOREIGN KEY (MaThuoc) REFERENCES Thuoc(MaThuoc)
);
GO

-- =========================
-- 8. DỊ ỨNG
-- =========================
CREATE TABLE DiUng (
    MaDiUng INT IDENTITY(1,1) PRIMARY KEY,
    TenDiUng NVARCHAR(150) NOT NULL
);
GO

-- =========================
-- 9. CẢNH BÁO DỊ ỨNG THUỐC
-- =========================
CREATE TABLE CanhBaoDiUngThuoc (
    MaThuoc INT,
    MaDiUng INT,
    NoiDung NVARCHAR(MAX),

    PRIMARY KEY (MaThuoc, MaDiUng),

    FOREIGN KEY (MaThuoc) REFERENCES Thuoc(MaThuoc),
    FOREIGN KEY (MaDiUng) REFERENCES DiUng(MaDiUng)
);
GO

-- =========================
-- 10. BỆNH NỀN
-- =========================
CREATE TABLE BenhNen (
    MaBenhNen INT IDENTITY(1,1) PRIMARY KEY,
    TenBenhNen NVARCHAR(150) NOT NULL
);
GO

-- =========================
-- 11. CẢNH BÁO BỆNH NỀN
-- =========================
CREATE TABLE CanhBaoBenhNenThuoc (
    MaThuoc INT,
    MaBenhNen INT,
    NoiDung NVARCHAR(MAX),

    PRIMARY KEY (MaThuoc, MaBenhNen),

    FOREIGN KEY (MaThuoc) REFERENCES Thuoc(MaThuoc),
    FOREIGN KEY (MaBenhNen) REFERENCES BenhNen(MaBenhNen)
);
GO

-- =========================
-- 12. QUY TẮC GỢI Ý THUỐC
-- =========================
CREATE TABLE QuyTacGoiYThuoc (
    MaQuyTac INT IDENTITY(1,1) PRIMARY KEY,
    MaBenh INT,
    MaTrieuChung INT,
    MaThuoc INT,
    MucDoMin INT,
    MucDoMax INT,
    DoUuTien INT DEFAULT 1,
    LyDo NVARCHAR(MAX),

    FOREIGN KEY (MaBenh) REFERENCES Benh(MaBenh),
    FOREIGN KEY (MaTrieuChung) REFERENCES TrieuChung(MaTrieuChung),
    FOREIGN KEY (MaThuoc) REFERENCES Thuoc(MaThuoc),

    CONSTRAINT CK_MucDo CHECK (
        MucDoMin BETWEEN 1 AND 3 AND
        MucDoMax BETWEEN 1 AND 3 AND
        MucDoMin <= MucDoMax
    )
);
GO

-- =========================
-- 13. LỊCH SỬ DỰ ĐOÁN
-- =========================
CREATE TABLE LichSuDuDoan (
    MaLichSu INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT,
    MaBenh INT,
    GhiChu NVARCHAR(MAX),
    NgayTao DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung),
    FOREIGN KEY (MaBenh) REFERENCES Benh(MaBenh)
);
GO

-- =========================
-- 14. TRIỆU CHỨNG TRONG LỊCH SỬ
-- =========================
CREATE TABLE ChiTietTrieuChung (
    MaLichSu INT,
    MaTrieuChung INT,
    MucDo INT,

    PRIMARY KEY (MaLichSu, MaTrieuChung),

    FOREIGN KEY (MaLichSu) REFERENCES LichSuDuDoan(MaLichSu),
    FOREIGN KEY (MaTrieuChung) REFERENCES TrieuChung(MaTrieuChung),

    CONSTRAINT CK_MucDo_CT CHECK (MucDo BETWEEN 1 AND 3)
);
GO

-- =========================
-- 15. DỊ ỨNG TRONG LỊCH SỬ
-- =========================
CREATE TABLE LichSuDiUng (
    MaLichSu INT,
    MaDiUng INT,

    PRIMARY KEY (MaLichSu, MaDiUng),

    FOREIGN KEY (MaLichSu) REFERENCES LichSuDuDoan(MaLichSu),
    FOREIGN KEY (MaDiUng) REFERENCES DiUng(MaDiUng)
);
GO

-- =========================
-- 16. BỆNH NỀN TRONG LỊCH SỬ
-- =========================
CREATE TABLE LichSuBenhNen (
    MaLichSu INT,
    MaBenhNen INT,

    PRIMARY KEY (MaLichSu, MaBenhNen),

    FOREIGN KEY (MaLichSu) REFERENCES LichSuDuDoan(MaLichSu),
    FOREIGN KEY (MaBenhNen) REFERENCES BenhNen(MaBenhNen)
);
GO

-- =========================
-- 17. KẾT QUẢ DỰ ĐOÁN
-- =========================
CREATE TABLE KetQuaDuDoan (
    MaKetQua INT IDENTITY(1,1) PRIMARY KEY,
    MaLichSu INT,
    MaThuoc INT,
    Diem FLOAT,
    LyDo NVARCHAR(MAX),
    CanhBao NVARCHAR(MAX),

    FOREIGN KEY (MaLichSu) REFERENCES LichSuDuDoan(MaLichSu),
    FOREIGN KEY (MaThuoc) REFERENCES Thuoc(MaThuoc)
);
GO

-- =========================
-- DỮ LIỆU MẪU ADMIN
-- =========================
INSERT INTO NguoiDung (HoTen, Email, MatKhauMaHoa, VaiTro)
VALUES (N'Quản trị viên', 'admin@example.com', 'hashed_password', 'Admin');
GO
        