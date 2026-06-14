using Project_CNPM.Models;
using Project_CNPM.Data;
using Microsoft.EntityFrameworkCore;
using Project_CNPM.Area.Admin.DTOs;

namespace Project_CNPM.Area.Admin.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<(bool IsSuccess, string Message, NguoiDung? User)> LoginAsync(loginDto request)
        {
            var user = await _context.NguoiDungs.FirstOrDefaultAsync(u => u.Email == request.email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.password, user.MatKhauMaHoa))
                return (false, "User not found", (NguoiDung?)null);

            return (true, "Login successful", user);
        }

        public async Task<(bool IsSuccess, string Message)> RegisterAsync(registerDto request)
        {
            var existingUser = await _context.NguoiDungs.FirstOrDefaultAsync(u => u.Email == request.email);

            if (existingUser != null) return (false, "Email already exists");

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.password);
            var newUser = new NguoiDung
            {
                HoTen = request.name,
                Email = request.email,
                MatKhauMaHoa = hashedPassword,
            };
            _context.NguoiDungs.Add(newUser);
            await _context.SaveChangesAsync();
            return (true, "Registration successful");
        }

        public async Task<(bool IsSuccess, string Message)> LogoutAsync(int userId)
        {
            return (true, "Logout successful");
        }
    }
}
