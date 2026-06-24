using System;
using System.Collections.Generic;

namespace Project_CNPM.Models;

public partial class DanhGiaDuDoan
{
    public int MaDanhGia { get; set; }

    public int MaKetQua { get; set; }

    public int MaNguoiDung { get; set; }

    public bool HuuIch { get; set; }

    public string? GhiChu { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual KetQuaDuDoan MaKetQuaNavigation { get; set; } = null!;

    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;
}
