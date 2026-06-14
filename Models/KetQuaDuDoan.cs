using System;
using System.Collections.Generic;

namespace Project_CNPM.Models;

public partial class KetQuaDuDoan
{
    public int MaKetQua { get; set; }

    public int? MaLichSu { get; set; }

    public int? MaThuoc { get; set; }

    public double? Diem { get; set; }

    public string? LyDo { get; set; }

    public string? CanhBao { get; set; }

    public string? TenThuocSnapshot { get; set; }

    public string? LieuDungSnapshot { get; set; }

    public virtual ICollection<DanhGiaDuDoan> DanhGiaDuDoans { get; set; } = new List<DanhGiaDuDoan>();

    public virtual LichSuDuDoan? MaLichSuNavigation { get; set; }

    public virtual Thuoc? MaThuocNavigation { get; set; }
}
