using Projcet_CNPM.DTOs.Auth;
using Project_CNPM.Models;

namespace Project_CNPM.Services;
public interface IAuthService
{
    Task<(bool IsSuccess, string Message, NguoiDung? User)> LoginAsync(loginDto request);
    Task<(bool IsSuccess, string Message)> RegisterAsync(registerDto request);
}