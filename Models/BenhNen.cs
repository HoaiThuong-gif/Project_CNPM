using System;
using System.Collections.Generic;

namespace Project_CNPM.Models;

public partial class BenhNen
{
    public int MaBenhNen { get; set; }

    public string TenBenhNen { get; set; } = null!;

    public virtual ICollection<CanhBaoBenhNenThuoc> CanhBaoBenhNenThuocs { get; set; } = new List<CanhBaoBenhNenThuoc>();

    public virtual ICollection<LichSuDuDoan> MaLichSus { get; set; } = new List<LichSuDuDoan>();
}
