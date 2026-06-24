using System;
using System.Collections.Generic;

namespace Project_CNPM.Models;

public partial class LichSuDuDoan
{
    public int MaLichSu { get; set; }

    public int? MaNguoiDung { get; set; }

    public int? MaBenh { get; set; }

    public string? GhiChu { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual ICollection<ChiTietTrieuChung> ChiTietTrieuChungs { get; set; } = new List<ChiTietTrieuChung>();

    public virtual ICollection<KetQuaDuDoan> KetQuaDuDoans { get; set; } = new List<KetQuaDuDoan>();

    public virtual Benh? MaBenhNavigation { get; set; }

    public virtual NguoiDung? MaNguoiDungNavigation { get; set; }

    public virtual ICollection<BenhNen> MaBenhNens { get; set; } = new List<BenhNen>();

    public virtual ICollection<DiUng> MaDiUngs { get; set; } = new List<DiUng>();
}
