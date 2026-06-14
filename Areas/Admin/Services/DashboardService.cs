using Microsoft.EntityFrameworkCore;
using Project_CNPM.Data;
using Project_CNPM.Area.Admin.DTOs;

namespace Project_CNPM.Area.Admin.Services

{
    public class DashboardAdminService : IDashboardAdminService
    {
        private readonly ApplicationDbContext _context;

        public DashboardAdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SystemStatisticsDto> GetSystemStatisticsAsync()
        {
            return new SystemStatisticsDto
            {
                TotalActiveUsers = await _context.NguoiDungs.CountAsync(u => u.DeleteAt == null && u.VaiTro == "User"),
                TotalActiveMedicines = await _context.Thuocs.CountAsync(t => t.DangHoatDong == true),
                TotalActiveDiseases = await _context.Benhs.CountAsync(b => b.DeleteAt == null && b.DangHoatDong == true),
                TotalPredictionsMade = await _context.LichSuDuDoans.CountAsync()
            };
        }
    }
}