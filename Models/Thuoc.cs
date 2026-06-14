using System;
using System.Collections.Generic;

namespace Project_CNPM.Models;

public partial class Thuoc
{
    public int MaThuoc { get; set; }

    public string TenThuoc { get; set; } = null!;

    public string? HoatChat { get; set; }

    public string? NhomThuoc { get; set; }

    public string? DangBaoChe { get; set; }

    public string? CongDung { get; set; }

    public string? LieuDung { get; set; }

    public string? CachDung { get; set; }

    public string? TacDungPhu { get; set; }

    public string? LuuY { get; set; }

    public bool CanKeDon { get; set; }

    public bool? DangHoatDong { get; set; }

    public DateTime? NgayTao { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public virtual ICollection<BenhThuoc> BenhThuocs { get; set; } = new List<BenhThuoc>();

    public virtual ICollection<CanhBaoBenhNenThuoc> CanhBaoBenhNenThuocs { get; set; } = new List<CanhBaoBenhNenThuoc>();

    public virtual ICollection<CanhBaoDiUngThuoc> CanhBaoDiUngThuocs { get; set; } = new List<CanhBaoDiUngThuoc>();

    public virtual ICollection<KetQuaDuDoan> KetQuaDuDoans { get; set; } = new List<KetQuaDuDoan>();

    public virtual ICollection<QuyTacGoiYthuoc> QuyTacGoiYthuocs { get; set; } = new List<QuyTacGoiYthuoc>();

    public virtual ICollection<TuongTacThuoc> TuongTacThuocMaThuoc1Navigations { get; set; } = new List<TuongTacThuoc>();

    public virtual ICollection<TuongTacThuoc> TuongTacThuocMaThuoc2Navigations { get; set; } = new List<TuongTacThuoc>();

    public virtual ICollection<ThanhPhan> MaThanhPhans { get; set; } = new List<ThanhPhan>();
}
