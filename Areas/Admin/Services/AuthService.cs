using Project_CNPM.Models;
using Project_CNPM.Data;
using Microsoft.EntityFrameworkCore;
using Project_CNPM.Area.Admin.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Project_CNPM.Area.Admin.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private string GenerateJwtToken(NguoiDung user)
        {
            var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is not configured.");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.MaNguoiDung.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.HoTen),
                new Claim(ClaimTypes.Role, user.VaiTro ?? "User") 
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7), // Cho phép đăng nhập trong 7 ngày
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

       public async Task<(bool IsSuccess, string Message, string? Token, NguoiDung? User)> LoginAsync(loginDto request)
        {
            var user = await _context.NguoiDungs.FirstOrDefaultAsync(u => u.Email == request.email);

            if (user == null || !IsPasswordValid(request.password, user.MatKhauMaHoa))
                return (false, "User not found or incorrect password", null, null);

            if (user.DeleteAt != null)
                return (false, "This account has been deleted", null, null);

            if (user.BiKhoa == true)
                return (false, "This account is currently locked by Administrator", null, null);

            string token = GenerateJwtToken(user);

            return (true, "Login successful", token, user);
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
                VaiTro = "User",
                BiKhoa = false,
                NgayTao = DateTime.Now
            };
            _context.NguoiDungs.Add(newUser);
            await _context.SaveChangesAsync();
            return (true, "Registration successful");
        }

        public async Task<(bool IsSuccess, string Message)> LogoutAsync(int userId)
        {
            return (true, "Logout successful");
        }

        private static bool IsPasswordValid(string plainPassword, string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(hashedPassword))
            {
                return false;
            }

            try
            {
                return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
            }
            catch
            {
                return false;
            }
        }
    }
}
