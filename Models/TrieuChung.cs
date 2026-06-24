using System;
using System.Collections.Generic;

namespace Project_CNPM.Models;

public partial class TrieuChung
{
    public int MaTrieuChung { get; set; }

    public string TenTrieuChung { get; set; } = null!;

    public string? MoTa { get; set; }

    public bool? DangHoatDong { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual ICollection<BenhTrieuChung> BenhTrieuChungs { get; set; } = new List<BenhTrieuChung>();

    public virtual ICollection<ChiTietTrieuChung> ChiTietTrieuChungs { get; set; } = new List<ChiTietTrieuChung>();

    public virtual ICollection<QuyTacGoiYthuoc> QuyTacGoiYthuocs { get; set; } = new List<QuyTacGoiYthuoc>();
}
