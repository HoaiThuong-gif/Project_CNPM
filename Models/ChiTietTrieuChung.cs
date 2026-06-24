using System;
using System.Collections.Generic;

namespace Project_CNPM.Models;

public partial class ChiTietTrieuChung
{
    public int MaLichSu { get; set; }

    public int MaTrieuChung { get; set; }

    public int? MucDo { get; set; }

    public virtual LichSuDuDoan MaLichSuNavigation { get; set; } = null!;

    public virtual TrieuChung MaTrieuChungNavigation { get; set; } = null!;
}
