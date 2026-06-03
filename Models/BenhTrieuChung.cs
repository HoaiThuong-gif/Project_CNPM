using System;
using System.Collections.Generic;

namespace Project_CNPM.Models;

public partial class BenhTrieuChung
{
    public int MaBenh { get; set; }

    public int MaTrieuChung { get; set; }

    public int? TrongSo { get; set; }

    public virtual Benh MaBenhNavigation { get; set; } = null!;

    public virtual TrieuChung MaTrieuChungNavigation { get; set; } = null!;
}
