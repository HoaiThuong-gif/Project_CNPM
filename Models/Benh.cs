using System;
using System.Collections.Generic;

namespace Project_CNPM.Models;

public partial class Benh
{
    public int MaBenh { get; set; }

    public string TenBenh { get; set; } = null!;

    public string? MoTa { get; set; }

    public string? NhomBenh { get; set; }

    public int? MucDoNghiemTrong { get; set; }

    public bool? DangHoatDong { get; set; }

    public DateTime? DeleteAt { get; set; }

    public DateTime? NgayTao { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public virtual ICollection<BenhThuoc> BenhThuocs { get; set; } = new List<BenhThuoc>();

    public virtual ICollection<BenhTrieuChung> BenhTrieuChungs { get; set; } = new List<BenhTrieuChung>();

    public virtual ICollection<LichSuDuDoan> LichSuDuDoans { get; set; } = new List<LichSuDuDoan>();

    public virtual ICollection<QuyTacGoiYthuoc> QuyTacGoiYthuocs { get; set; } = new List<QuyTacGoiYthuoc>();
}
