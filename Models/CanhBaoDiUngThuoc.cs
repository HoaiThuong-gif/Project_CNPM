using System;
using System.Collections.Generic;

namespace Project_CNPM.Models;

public partial class CanhBaoDiUngThuoc
{
    public int MaThuoc { get; set; }

    public int MaDiUng { get; set; }

    public string? NoiDung { get; set; }

    public virtual DiUng MaDiUngNavigation { get; set; } = null!;

    public virtual Thuoc MaThuocNavigation { get; set; } = null!;
}
