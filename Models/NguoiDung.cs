using System;
using System.Collections.Generic;

namespace Project_CNPM.Models;

public partial class NguoiDung
{
    public int MaNguoiDung { get; set; }

    public string HoTen { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string MatKhauMaHoa { get; set; } = null!;

    public string? SoDienThoai { get; set; }

    public string? GioiTinh { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public string? VaiTro { get; set; }

    public bool? BiKhoa { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual ICollection<LichSuDuDoan> LichSuDuDoans { get; set; } = new List<LichSuDuDoan>();
}
