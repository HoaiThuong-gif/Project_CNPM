using System;
using System.Collections.Generic;

namespace Project_CNPM.Models;

public partial class QuyTacGoiYthuoc
{
    public int MaQuyTac { get; set; }

    public int MaBenh { get; set; }

    public int? MaTrieuChung { get; set; }

    public int MaThuoc { get; set; }

    public int? MucDoMin { get; set; }

    public int? MucDoMax { get; set; }

    public int? DoUuTien { get; set; }

    public string? LyDo { get; set; }

    public bool? DangHoatDong { get; set; }

    public virtual Benh MaBenhNavigation { get; set; } = null!;

    public virtual Thuoc MaThuocNavigation { get; set; } = null!;

    public virtual TrieuChung? MaTrieuChungNavigation { get; set; }
}
