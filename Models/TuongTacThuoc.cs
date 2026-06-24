using System;
using System.Collections.Generic;

namespace Project_CNPM.Models;

public partial class TuongTacThuoc
{
    public int MaThuoc1 { get; set; }

    public int MaThuoc2 { get; set; }

    public int MucDoNghiemTrong { get; set; }

    public string? MoTa { get; set; }

    public virtual Thuoc MaThuoc1Navigation { get; set; } = null!;

    public virtual Thuoc MaThuoc2Navigation { get; set; } = null!;
}
