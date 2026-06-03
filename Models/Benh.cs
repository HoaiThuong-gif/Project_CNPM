using System;
using System.Collections.Generic;

namespace Project_CNPM.Models;

public partial class Benh
{
    public int MaBenh { get; set; }

    public string TenBenh { get; set; } = null!;

    public string? MoTa { get; set; }

    public bool? DangHoatDong { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual ICollection<BenhTrieuChung> BenhTrieuChungs { get; set; } = new List<BenhTrieuChung>();

    public virtual ICollection<LichSuDuDoan> LichSuDuDoans { get; set; } = new List<LichSuDuDoan>();

    public virtual ICollection<QuyTacGoiYthuoc> QuyTacGoiYthuocs { get; set; } = new List<QuyTacGoiYthuoc>();

    public virtual ICollection<Thuoc> MaThuocs { get; set; } = new List<Thuoc>();
}
