using Microsoft.EntityFrameworkCore;
using Project_CNPM.Data;
using Project_CNPM.DTOs.Medicine;
using Project_CNPM.Models;

namespace Project_CNPM.Area.Admin.Services{
    public class UserAdminService : IUserAdminService
    {
        private readonly ApplicationDbContext _context;

        public UserAdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserAdminViewDto>> GetAllUsersAsync() // Xóa tham số
        {
            // Bỏ đoạn if(!includeDeleted)
            return await _context.NguoiDungs.Select(u => new UserAdminViewDto
            {
                UserId = u.MaNguoiDung,
                FullName = u.HoTen,
                Email = u.Email,
                Phone = u.SoDienThoai ?? string.Empty,
                Gender = u.GioiTinh ?? string.Empty,
                DateOfBirth = u.NgaySinh,
                Role = u.VaiTro ?? "User",
                IsLocked = u.BiKhoa ?? false,
                IsDeleted = u.DeleteAt != null,
                CreatedAt = u.NgayTao ?? DateTime.Now
            }).ToListAsync();
        }

        public async Task<bool> ToggleUserLockStatusAsync(int userId, bool isLocked)
        {
            var user = await _context.NguoiDungs.FindAsync(userId);
            if (user == null || user.DeleteAt != null) return false;

            user.BiKhoa = isLocked;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftDeleteUserAsync(int userId)
        {
            var user = await _context.NguoiDungs.FindAsync(userId);
            if (user == null || user.DeleteAt != null) return false;

            user.DeleteAt = DateTime.Now;
            // Tùy chọn: Khi xóa mềm có thể khóa luôn tài khoản để chắc chắn user không login được nữa
            user.BiKhoa = true; 

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
