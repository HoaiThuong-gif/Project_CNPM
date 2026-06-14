using Project_CNPM.Models;
using Microsoft.EntityFrameworkCore;

namespace Project_CNPM.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Benh> Benhs { get; set; }

    public virtual DbSet<BenhNen> BenhNens { get; set; }

    public virtual DbSet<BenhThuoc> BenhThuocs { get; set; }

    public virtual DbSet<BenhTrieuChung> BenhTrieuChungs { get; set; }

    public virtual DbSet<CanhBaoBenhNenThuoc> CanhBaoBenhNenThuocs { get; set; }

    public virtual DbSet<CanhBaoDiUngThuoc> CanhBaoDiUngThuocs { get; set; }

    public virtual DbSet<ChiTietTrieuChung> ChiTietTrieuChungs { get; set; }

    public virtual DbSet<DanhGiaDuDoan> DanhGiaDuDoans { get; set; }

    public virtual DbSet<DiUng> DiUngs { get; set; }

    public virtual DbSet<KetQuaDuDoan> KetQuaDuDoans { get; set; }

    public virtual DbSet<LichSuDuDoan> LichSuDuDoans { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<QuyTacGoiYthuoc> QuyTacGoiYthuocs { get; set; }

    public virtual DbSet<ThanhPhan> ThanhPhans { get; set; }

    public virtual DbSet<Thuoc> Thuocs { get; set; }

    public virtual DbSet<TrieuChung> TrieuChungs { get; set; }

    public virtual DbSet<TuongTacThuoc> TuongTacThuocs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=127.0.0.1,1433;Database=WebsiteDuDoanThuoc;User Id=sa;Password=Kh@ng09102005;TrustServerCertificate=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Benh>(entity =>
        {
            entity.HasKey(e => e.MaBenh).HasName("PK__Benh__DB7E2D4999545760");

            entity.ToTable("Benh");

            entity.Property(e => e.DangHoatDong).HasDefaultValue(true);
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.MucDoNghiemTrong).HasDefaultValue(1);
            entity.Property(e => e.NgayCapNhat).HasColumnType("datetime");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NhomBenh).HasMaxLength(100);
            entity.Property(e => e.TenBenh).HasMaxLength(150);
        });

        modelBuilder.Entity<BenhNen>(entity =>
        {
            entity.HasKey(e => e.MaBenhNen).HasName("PK__BenhNen__E94B6BED43CFD0DA");

            entity.ToTable("BenhNen");

            entity.Property(e => e.DangHoatDong).HasDefaultValue(true);
            entity.Property(e => e.TenBenhNen).HasMaxLength(150);
        });

        modelBuilder.Entity<BenhThuoc>(entity =>
        {
            entity.HasKey(e => new { e.MaBenh, e.MaThuoc }).HasName("PK__BenhThuo__CFC5322B69FB3E32");

            entity.ToTable("BenhThuoc");

            entity.Property(e => e.DoUuTien).HasDefaultValue(1);
            entity.Property(e => e.LoaiDieuTri)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("primary");

            entity.HasOne(d => d.MaBenhNavigation).WithMany(p => p.BenhThuocs)
                .HasForeignKey(d => d.MaBenh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BenhThuoc__MaBen__46E78A0C");

            entity.HasOne(d => d.MaThuocNavigation).WithMany(p => p.BenhThuocs)
                .HasForeignKey(d => d.MaThuoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BenhThuoc__MaThu__47DBAE45");
        });

        modelBuilder.Entity<BenhTrieuChung>(entity =>
        {
            entity.HasKey(e => new { e.MaBenh, e.MaTrieuChung }).HasName("PK__BenhTrie__E45FC2F759BF3FCF");

            entity.ToTable("BenhTrieuChung");

            entity.Property(e => e.TrongSo).HasDefaultValue(1);

            entity.HasOne(d => d.MaBenhNavigation).WithMany(p => p.BenhTrieuChungs)
                .HasForeignKey(d => d.MaBenh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BenhTrieu__MaBen__35BCFE0A");

            entity.HasOne(d => d.MaTrieuChungNavigation).WithMany(p => p.BenhTrieuChungs)
                .HasForeignKey(d => d.MaTrieuChung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BenhTrieu__MaTri__36B12243");
        });

        modelBuilder.Entity<CanhBaoBenhNenThuoc>(entity =>
        {
            entity.HasKey(e => new { e.MaThuoc, e.MaBenhNen }).HasName("PK__CanhBaoB__8525409E35F7BABA");

            entity.ToTable("CanhBaoBenhNenThuoc");

            entity.HasOne(d => d.MaBenhNenNavigation).WithMany(p => p.CanhBaoBenhNenThuocs)
                .HasForeignKey(d => d.MaBenhNen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CanhBaoBe__MaBen__5629CD9C");

            entity.HasOne(d => d.MaThuocNavigation).WithMany(p => p.CanhBaoBenhNenThuocs)
                .HasForeignKey(d => d.MaThuoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CanhBaoBe__MaThu__5535A963");
        });

        modelBuilder.Entity<CanhBaoDiUngThuoc>(entity =>
        {
            entity.HasKey(e => new { e.MaThuoc, e.MaDiUng }).HasName("PK__CanhBaoD__C827743312E36031");

            entity.ToTable("CanhBaoDiUngThuoc");

            entity.HasOne(d => d.MaDiUngNavigation).WithMany(p => p.CanhBaoDiUngThuocs)
                .HasForeignKey(d => d.MaDiUng)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CanhBaoDi__MaDiU__4F7CD00D");

            entity.HasOne(d => d.MaThuocNavigation).WithMany(p => p.CanhBaoDiUngThuocs)
                .HasForeignKey(d => d.MaThuoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CanhBaoDi__MaThu__4E88ABD4");
        });

        modelBuilder.Entity<ChiTietTrieuChung>(entity =>
        {
            entity.HasKey(e => new { e.MaLichSu, e.MaTrieuChung }).HasName("PK__ChiTietT__FB62CD9447243565");

            entity.ToTable("ChiTietTrieuChung");

            entity.HasOne(d => d.MaLichSuNavigation).WithMany(p => p.ChiTietTrieuChungs)
                .HasForeignKey(d => d.MaLichSu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietTr__MaLic__6B24EA82");

            entity.HasOne(d => d.MaTrieuChungNavigation).WithMany(p => p.ChiTietTrieuChungs)
                .HasForeignKey(d => d.MaTrieuChung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietTr__MaTri__6C190EBB");
        });

        modelBuilder.Entity<DanhGiaDuDoan>(entity =>
        {
            entity.HasKey(e => e.MaDanhGia).HasName("PK__DanhGiaD__AA9515BFB9C3DE81");

            entity.ToTable("DanhGiaDuDoan");

            entity.HasIndex(e => new { e.MaKetQua, e.MaNguoiDung }, "UQ_DanhGia_KetQua_NguoiDung").IsUnique();

            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaKetQuaNavigation).WithMany(p => p.DanhGiaDuDoans)
                .HasForeignKey(d => d.MaKetQua)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DanhGiaDu__MaKet__7D439ABD");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.DanhGiaDuDoans)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DanhGiaDu__MaNgu__7E37BEF6");
        });

        modelBuilder.Entity<DiUng>(entity =>
        {
            entity.HasKey(e => e.MaDiUng).HasName("PK__DiUng__396821390B9E754F");

            entity.ToTable("DiUng");

            entity.Property(e => e.DangHoatDong).HasDefaultValue(true);
            entity.Property(e => e.TenDiUng).HasMaxLength(150);
        });

        modelBuilder.Entity<KetQuaDuDoan>(entity =>
        {
            entity.HasKey(e => e.MaKetQua).HasName("PK__KetQuaDu__D5B3102A2939BD17");

            entity.ToTable("KetQuaDuDoan");

            entity.Property(e => e.TenThuocSnapshot).HasMaxLength(150);

            entity.HasOne(d => d.MaLichSuNavigation).WithMany(p => p.KetQuaDuDoans)
                .HasForeignKey(d => d.MaLichSu)
                .HasConstraintName("FK__KetQuaDuD__MaLic__778AC167");

            entity.HasOne(d => d.MaThuocNavigation).WithMany(p => p.KetQuaDuDoans)
                .HasForeignKey(d => d.MaThuoc)
                .HasConstraintName("FK__KetQuaDuD__MaThu__787EE5A0");
        });

        modelBuilder.Entity<LichSuDuDoan>(entity =>
        {
            entity.HasKey(e => e.MaLichSu).HasName("PK__LichSuDu__C443222A839EC55E");

            entity.ToTable("LichSuDuDoan");

            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaBenhNavigation).WithMany(p => p.LichSuDuDoans)
                .HasForeignKey(d => d.MaBenh)
                .HasConstraintName("FK__LichSuDuD__MaBen__68487DD7");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.LichSuDuDoans)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("FK__LichSuDuD__MaNgu__6754599E");

            entity.HasMany(d => d.MaBenhNens).WithMany(p => p.MaLichSus)
                .UsingEntity<Dictionary<string, object>>(
                    "LichSuBenhNen",
                    r => r.HasOne<BenhNen>().WithMany()
                        .HasForeignKey("MaBenhNen")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__LichSuBen__MaBen__74AE54BC"),
                    l => l.HasOne<LichSuDuDoan>().WithMany()
                        .HasForeignKey("MaLichSu")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__LichSuBen__MaLic__73BA3083"),
                    j =>
                    {
                        j.HasKey("MaLichSu", "MaBenhNen").HasName("PK__LichSuBe__0AD79494D096B4D6");
                        j.ToTable("LichSuBenhNen");
                    });

            entity.HasMany(d => d.MaDiUngs).WithMany(p => p.MaLichSus)
                .UsingEntity<Dictionary<string, object>>(
                    "LichSuDiUng",
                    r => r.HasOne<DiUng>().WithMany()
                        .HasForeignKey("MaDiUng")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__LichSuDiU__MaDiU__70DDC3D8"),
                    l => l.HasOne<LichSuDuDoan>().WithMany()
                        .HasForeignKey("MaLichSu")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__LichSuDiU__MaLic__6FE99F9F"),
                    j =>
                    {
                        j.HasKey("MaLichSu", "MaDiUng").HasName("PK__LichSuDi__47D5A0390D4ADF50");
                        j.ToTable("LichSuDiUng");
                    });
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PK__NguoiDun__C539D762D8DE2D54");

            entity.ToTable("NguoiDung");

            entity.HasIndex(e => e.Email, "UQ__NguoiDun__A9D10534852C628A").IsUnique();

            entity.Property(e => e.BiKhoa).HasDefaultValue(false);
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.GioiTinh).HasMaxLength(10);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.MatKhauMaHoa)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.VaiTro)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("User");
        });

        modelBuilder.Entity<QuyTacGoiYthuoc>(entity =>
        {
            entity.HasKey(e => e.MaQuyTac).HasName("PK__QuyTacGo__0D38A08A15F4E539");

            entity.ToTable("QuyTacGoiYThuoc");

            entity.Property(e => e.DangHoatDong).HasDefaultValue(true);
            entity.Property(e => e.DoUuTien).HasDefaultValue(1);

            entity.HasOne(d => d.MaBenhNavigation).WithMany(p => p.QuyTacGoiYthuocs)
                .HasForeignKey(d => d.MaBenh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QuyTacGoi__MaBen__60A75C0F");

            entity.HasOne(d => d.MaThuocNavigation).WithMany(p => p.QuyTacGoiYthuocs)
                .HasForeignKey(d => d.MaThuoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QuyTacGoi__MaThu__628FA481");

            entity.HasOne(d => d.MaTrieuChungNavigation).WithMany(p => p.QuyTacGoiYthuocs)
                .HasForeignKey(d => d.MaTrieuChung)
                .HasConstraintName("FK__QuyTacGoi__MaTri__619B8048");
        });

        modelBuilder.Entity<ThanhPhan>(entity =>
        {
            entity.HasKey(e => e.MaThanhPhan).HasName("PK__ThanhPha__B84B504E006395E6");

            entity.ToTable("ThanhPhan");

            entity.Property(e => e.TenThanhPhan).HasMaxLength(150);
        });

        modelBuilder.Entity<Thuoc>(entity =>
        {
            entity.HasKey(e => e.MaThuoc).HasName("PK__Thuoc__4BB1F620274DD04B");

            entity.ToTable("Thuoc");

            entity.Property(e => e.CanKeDon).HasDefaultValue(false);
            entity.Property(e => e.DangBaoChe).HasMaxLength(100);
            entity.Property(e => e.DangHoatDong).HasDefaultValue(true);
            entity.Property(e => e.HoatChat).HasMaxLength(255);
            entity.Property(e => e.NgayCapNhat).HasColumnType("datetime");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NhomThuoc).HasMaxLength(100);
            entity.Property(e => e.TenThuoc).HasMaxLength(150);

            entity.HasMany(d => d.MaThanhPhans).WithMany(p => p.MaThuocs)
                .UsingEntity<Dictionary<string, object>>(
                    "ThuocThanhPhan",
                    r => r.HasOne<ThanhPhan>().WithMany()
                        .HasForeignKey("MaThanhPhan")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ThuocThan__MaTha__4222D4EF"),
                    l => l.HasOne<Thuoc>().WithMany()
                        .HasForeignKey("MaThuoc")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ThuocThan__MaThu__412EB0B6"),
                    j =>
                    {
                        j.HasKey("MaThuoc", "MaThanhPhan").HasName("PK__ThuocTha__B03543240D683A54");
                        j.ToTable("ThuocThanhPhan");
                    });
        });

        modelBuilder.Entity<TrieuChung>(entity =>
        {
            entity.HasKey(e => e.MaTrieuChung).HasName("PK__TrieuChu__F21EFBE2F8627BE8");

            entity.ToTable("TrieuChung");

            entity.Property(e => e.DangHoatDong).HasDefaultValue(true);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TenTrieuChung).HasMaxLength(150);
        });

        modelBuilder.Entity<TuongTacThuoc>(entity =>
        {
            entity.HasKey(e => new { e.MaThuoc1, e.MaThuoc2 }).HasName("PK__TuongTac__E65B64067A46C050");

            entity.ToTable("TuongTacThuoc");

            entity.HasOne(d => d.MaThuoc1Navigation).WithMany(p => p.TuongTacThuocMaThuoc1Navigations)
                .HasForeignKey(d => d.MaThuoc1)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TuongTacT__MaThu__59063A47");

            entity.HasOne(d => d.MaThuoc2Navigation).WithMany(p => p.TuongTacThuocMaThuoc2Navigations)
                .HasForeignKey(d => d.MaThuoc2)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TuongTacT__MaThu__59FA5E80");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
