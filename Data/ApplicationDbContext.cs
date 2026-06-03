using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Project_CNPM.Models;

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

    public virtual DbSet<BenhTrieuChung> BenhTrieuChungs { get; set; }

    public virtual DbSet<CanhBaoBenhNenThuoc> CanhBaoBenhNenThuocs { get; set; }

    public virtual DbSet<CanhBaoDiUngThuoc> CanhBaoDiUngThuocs { get; set; }

    public virtual DbSet<ChiTietTrieuChung> ChiTietTrieuChungs { get; set; }

    public virtual DbSet<DiUng> DiUngs { get; set; }

    public virtual DbSet<KetQuaDuDoan> KetQuaDuDoans { get; set; }

    public virtual DbSet<LichSuDuDoan> LichSuDuDoans { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<QuyTacGoiYthuoc> QuyTacGoiYthuocs { get; set; }

    public virtual DbSet<ThanhPhan> ThanhPhans { get; set; }

    public virtual DbSet<Thuoc> Thuocs { get; set; }

    public virtual DbSet<TrieuChung> TrieuChungs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=WebsiteDuDoanThuoc;User Id=sa;Password=Kh@ng09102005;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Benh>(entity =>
        {
            entity.HasKey(e => e.MaBenh).HasName("PK__Benh__DB7E2D499B619B87");

            entity.ToTable("Benh");

            entity.Property(e => e.DangHoatDong).HasDefaultValue(true);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TenBenh).HasMaxLength(150);

            entity.HasMany(d => d.MaThuocs).WithMany(p => p.MaBenhs)
                .UsingEntity<Dictionary<string, object>>(
                    "BenhThuoc",
                    r => r.HasOne<Thuoc>().WithMany()
                        .HasForeignKey("MaThuoc")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__BenhThuoc__MaThu__4222D4EF"),
                    l => l.HasOne<Benh>().WithMany()
                        .HasForeignKey("MaBenh")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__BenhThuoc__MaBen__412EB0B6"),
                    j =>
                    {
                        j.HasKey("MaBenh", "MaThuoc").HasName("PK__BenhThuo__CFC5322B745053C1");
                        j.ToTable("BenhThuoc");
                    });
        });

        modelBuilder.Entity<BenhNen>(entity =>
        {
            entity.HasKey(e => e.MaBenhNen).HasName("PK__BenhNen__E94B6BED639F6BE9");

            entity.ToTable("BenhNen");

            entity.Property(e => e.TenBenhNen).HasMaxLength(150);
        });

        modelBuilder.Entity<BenhTrieuChung>(entity =>
        {
            entity.HasKey(e => new { e.MaBenh, e.MaTrieuChung }).HasName("PK__BenhTrie__E45FC2F7476BF24D");

            entity.ToTable("BenhTrieuChung");

            entity.Property(e => e.TrongSo).HasDefaultValue(1);

            entity.HasOne(d => d.MaBenhNavigation).WithMany(p => p.BenhTrieuChungs)
                .HasForeignKey(d => d.MaBenh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BenhTrieu__MaBen__32E0915F");

            entity.HasOne(d => d.MaTrieuChungNavigation).WithMany(p => p.BenhTrieuChungs)
                .HasForeignKey(d => d.MaTrieuChung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BenhTrieu__MaTri__33D4B598");
        });

        modelBuilder.Entity<CanhBaoBenhNenThuoc>(entity =>
        {
            entity.HasKey(e => new { e.MaThuoc, e.MaBenhNen }).HasName("PK__CanhBaoB__8525409EBFB30429");

            entity.ToTable("CanhBaoBenhNenThuoc");

            entity.HasOne(d => d.MaBenhNenNavigation).WithMany(p => p.CanhBaoBenhNenThuocs)
                .HasForeignKey(d => d.MaBenhNen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CanhBaoBe__MaBen__4D94879B");

            entity.HasOne(d => d.MaThuocNavigation).WithMany(p => p.CanhBaoBenhNenThuocs)
                .HasForeignKey(d => d.MaThuoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CanhBaoBe__MaThu__4CA06362");
        });

        modelBuilder.Entity<CanhBaoDiUngThuoc>(entity =>
        {
            entity.HasKey(e => new { e.MaThuoc, e.MaDiUng }).HasName("PK__CanhBaoD__C827743388196212");

            entity.ToTable("CanhBaoDiUngThuoc");

            entity.HasOne(d => d.MaDiUngNavigation).WithMany(p => p.CanhBaoDiUngThuocs)
                .HasForeignKey(d => d.MaDiUng)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CanhBaoDi__MaDiU__47DBAE45");

            entity.HasOne(d => d.MaThuocNavigation).WithMany(p => p.CanhBaoDiUngThuocs)
                .HasForeignKey(d => d.MaThuoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CanhBaoDi__MaThu__46E78A0C");
        });

        modelBuilder.Entity<ChiTietTrieuChung>(entity =>
        {
            entity.HasKey(e => new { e.MaLichSu, e.MaTrieuChung }).HasName("PK__ChiTietT__FB62CD94AC922A1D");

            entity.ToTable("ChiTietTrieuChung");

            entity.HasOne(d => d.MaLichSuNavigation).WithMany(p => p.ChiTietTrieuChungs)
                .HasForeignKey(d => d.MaLichSu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietTr__MaLic__5BE2A6F2");

            entity.HasOne(d => d.MaTrieuChungNavigation).WithMany(p => p.ChiTietTrieuChungs)
                .HasForeignKey(d => d.MaTrieuChung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietTr__MaTri__5CD6CB2B");
        });

        modelBuilder.Entity<DiUng>(entity =>
        {
            entity.HasKey(e => e.MaDiUng).HasName("PK__DiUng__39682139F2B2D3C7");

            entity.ToTable("DiUng");

            entity.Property(e => e.TenDiUng).HasMaxLength(150);
        });

        modelBuilder.Entity<KetQuaDuDoan>(entity =>
        {
            entity.HasKey(e => e.MaKetQua).HasName("PK__KetQuaDu__D5B3102A2152D8E5");

            entity.ToTable("KetQuaDuDoan");

            entity.HasOne(d => d.MaLichSuNavigation).WithMany(p => p.KetQuaDuDoans)
                .HasForeignKey(d => d.MaLichSu)
                .HasConstraintName("FK__KetQuaDuD__MaLic__68487DD7");

            entity.HasOne(d => d.MaThuocNavigation).WithMany(p => p.KetQuaDuDoans)
                .HasForeignKey(d => d.MaThuoc)
                .HasConstraintName("FK__KetQuaDuD__MaThu__693CA210");
        });

        modelBuilder.Entity<LichSuDuDoan>(entity =>
        {
            entity.HasKey(e => e.MaLichSu).HasName("PK__LichSuDu__C443222A54EB63EA");

            entity.ToTable("LichSuDuDoan");

            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaBenhNavigation).WithMany(p => p.LichSuDuDoans)
                .HasForeignKey(d => d.MaBenh)
                .HasConstraintName("FK__LichSuDuD__MaBen__59063A47");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.LichSuDuDoans)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("FK__LichSuDuD__MaNgu__5812160E");

            entity.HasMany(d => d.MaBenhNens).WithMany(p => p.MaLichSus)
                .UsingEntity<Dictionary<string, object>>(
                    "LichSuBenhNen",
                    r => r.HasOne<BenhNen>().WithMany()
                        .HasForeignKey("MaBenhNen")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__LichSuBen__MaBen__656C112C"),
                    l => l.HasOne<LichSuDuDoan>().WithMany()
                        .HasForeignKey("MaLichSu")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__LichSuBen__MaLic__6477ECF3"),
                    j =>
                    {
                        j.HasKey("MaLichSu", "MaBenhNen").HasName("PK__LichSuBe__0AD79494F8E46896");
                        j.ToTable("LichSuBenhNen");
                    });

            entity.HasMany(d => d.MaDiUngs).WithMany(p => p.MaLichSus)
                .UsingEntity<Dictionary<string, object>>(
                    "LichSuDiUng",
                    r => r.HasOne<DiUng>().WithMany()
                        .HasForeignKey("MaDiUng")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__LichSuDiU__MaDiU__619B8048"),
                    l => l.HasOne<LichSuDuDoan>().WithMany()
                        .HasForeignKey("MaLichSu")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__LichSuDiU__MaLic__60A75C0F"),
                    j =>
                    {
                        j.HasKey("MaLichSu", "MaDiUng").HasName("PK__LichSuDi__47D5A0395ECBBEAD");
                        j.ToTable("LichSuDiUng");
                    });
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PK__NguoiDun__C539D762E8A8AEA7");

            entity.ToTable("NguoiDung");

            entity.HasIndex(e => e.Email, "UQ__NguoiDun__A9D1053455CE0D5C").IsUnique();

            entity.Property(e => e.BiKhoa).HasDefaultValue(false);
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
            entity.HasKey(e => e.MaQuyTac).HasName("PK__QuyTacGo__0D38A08A19A0E5EF");

            entity.ToTable("QuyTacGoiYThuoc");

            entity.Property(e => e.DoUuTien).HasDefaultValue(1);

            entity.HasOne(d => d.MaBenhNavigation).WithMany(p => p.QuyTacGoiYthuocs)
                .HasForeignKey(d => d.MaBenh)
                .HasConstraintName("FK__QuyTacGoi__MaBen__5165187F");

            entity.HasOne(d => d.MaThuocNavigation).WithMany(p => p.QuyTacGoiYthuocs)
                .HasForeignKey(d => d.MaThuoc)
                .HasConstraintName("FK__QuyTacGoi__MaThu__534D60F1");

            entity.HasOne(d => d.MaTrieuChungNavigation).WithMany(p => p.QuyTacGoiYthuocs)
                .HasForeignKey(d => d.MaTrieuChung)
                .HasConstraintName("FK__QuyTacGoi__MaTri__52593CB8");
        });

        modelBuilder.Entity<ThanhPhan>(entity =>
        {
            entity.HasKey(e => e.MaThanhPhan).HasName("PK__ThanhPha__B84B504E40E2D6B1");

            entity.ToTable("ThanhPhan");

            entity.Property(e => e.TenThanhPhan).HasMaxLength(150);
        });

        modelBuilder.Entity<Thuoc>(entity =>
        {
            entity.HasKey(e => e.MaThuoc).HasName("PK__Thuoc__4BB1F620531608AD");

            entity.ToTable("Thuoc");

            entity.Property(e => e.DangHoatDong).HasDefaultValue(true);
            entity.Property(e => e.HoatChat).HasMaxLength(255);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TenThuoc).HasMaxLength(150);

            entity.HasMany(d => d.MaThanhPhans).WithMany(p => p.MaThuocs)
                .UsingEntity<Dictionary<string, object>>(
                    "ThuocThanhPhan",
                    r => r.HasOne<ThanhPhan>().WithMany()
                        .HasForeignKey("MaThanhPhan")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ThuocThan__MaTha__3E52440B"),
                    l => l.HasOne<Thuoc>().WithMany()
                        .HasForeignKey("MaThuoc")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ThuocThan__MaThu__3D5E1FD2"),
                    j =>
                    {
                        j.HasKey("MaThuoc", "MaThanhPhan").HasName("PK__ThuocTha__B0354324913A4E32");
                        j.ToTable("ThuocThanhPhan");
                    });
        });

        modelBuilder.Entity<TrieuChung>(entity =>
        {
            entity.HasKey(e => e.MaTrieuChung).HasName("PK__TrieuChu__F21EFBE28DF55CAA");

            entity.ToTable("TrieuChung");

            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TenTrieuChung).HasMaxLength(150);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
