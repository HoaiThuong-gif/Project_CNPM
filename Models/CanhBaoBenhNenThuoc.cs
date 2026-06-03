using System;
using System.Collections.Generic;

namespace Project_CNPM.Models;

public partial class CanhBaoBenhNenThuoc
{
    public int MaThuoc { get; set; }

    public int MaBenhNen { get; set; }

    public string? NoiDung { get; set; }

    public virtual BenhNen MaBenhNenNavigation { get; set; } = null!;

    public virtual Thuoc MaThuocNavigation { get; set; } = null!;
}
