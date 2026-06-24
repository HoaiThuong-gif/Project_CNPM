using System;
using System.Collections.Generic;

namespace Project_CNPM.Models;

public partial class BenhThuoc
{
    public int MaBenh { get; set; }

    public int MaThuoc { get; set; }

    public int? DoUuTien { get; set; }

    public string? LoaiDieuTri { get; set; }

    public virtual Benh MaBenhNavigation { get; set; } = null!;

    public virtual Thuoc MaThuocNavigation { get; set; } = null!;
}
