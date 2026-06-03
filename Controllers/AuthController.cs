using Projcet_CNPM.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_CNPM.Data;
using Project_CNPM.Services;

namespace Project_CNPM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(loginDto loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.email) || string.IsNullOrEmpty(loginDto.password))
                return BadRequest(new { message = "Email và mật khẩu không được để trống!" });

            var result = await _authService.LoginAsync(loginDto);

             if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });
            
            
            return Ok(new { 
                message = "Đăng nhập thành công!",
                userId = result.User.MaNguoiDung,
                username = result.User.HoTen,
                });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(registerDto registerDto)
        {
            if (string.IsNullOrEmpty(registerDto.name) || string.IsNullOrEmpty(registerDto.email) || string.IsNullOrEmpty(registerDto.password) || string.IsNullOrEmpty(registerDto.confirmPassword))
                return BadRequest(new { message = "Vui lòng điền đầy đủ thông tin!" });

            if (registerDto.password != registerDto.confirmPassword)
                return BadRequest(new { message = "Mật khẩu xác nhận không khớp!" });

            var result = await _authService.RegisterAsync(registerDto);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = "Đăng ký thành công!" });
        }
    }
}