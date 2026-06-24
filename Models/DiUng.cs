using System;
using System.Collections.Generic;

namespace Project_CNPM.Models;

public partial class DiUng
{
    public int MaDiUng { get; set; }

    public string TenDiUng { get; set; } = null!;

    public bool? DangHoatDong { get; set; }

    public virtual ICollection<CanhBaoDiUngThuoc> CanhBaoDiUngThuocs { get; set; } = new List<CanhBaoDiUngThuoc>();

    public virtual ICollection<LichSuDuDoan> MaLichSus { get; set; } = new List<LichSuDuDoan>();
}
