using Project_CNPM.Area.Admin.DTOs;
using Project_CNPM.Models;

namespace Project_CNPM.Area.Admin.Services
{
    public interface IAuthService
    {
        Task<(bool IsSuccess, string Message,string? Token, NguoiDung? User)> LoginAsync(loginDto request);
        Task<(bool IsSuccess, string Message)> RegisterAsync(registerDto request);
        Task<(bool IsSuccess, string Message)> LogoutAsync(int userId);
    }
}