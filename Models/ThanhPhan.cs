using System;
using System.Collections.Generic;

namespace Project_CNPM.Models;

public partial class ThanhPhan
{
    public int MaThanhPhan { get; set; }

    public string TenThanhPhan { get; set; } = null!;

    public virtual ICollection<Thuoc> MaThuocs { get; set; } = new List<Thuoc>();
}
