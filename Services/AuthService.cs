using Project_CNPM.Models;
using Project_CNPM.Data;
using Projcet_CNPM.DTOs.Auth;

namespace Project_CNPM.Services
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
            var user = _context.NguoiDungs.FirstOrDefault(u => u.Email == request.email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.password, user.MatKhauMaHoa))
                return (false, "User not found", (NguoiDung?)null);

            return (true, "Login successful", user);
        }

        public async Task<(bool IsSuccess, string Message)> RegisterAsync(registerDto request)
        {
            var existingUser = _context.NguoiDungs.FirstOrDefault(u => u.Email == request.email);

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
    }
}
