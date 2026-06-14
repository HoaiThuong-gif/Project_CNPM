using Microsoft.AspNetCore.Mvc;
using Project_CNPM.Area.Admin.Services;

namespace Project_CNPM.Area.Admin.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : Controller
    {
        private readonly IUserAdminService _userAdminService;

        public UserController(IUserAdminService userAdminService)
        {
            _userAdminService = userAdminService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers(bool includeDeleted = false)
        {
            var users = await _userAdminService.GetAllUsersAsync(includeDeleted);
            return Ok(users);
        }

        [HttpPatch("lock")]
        public async Task<IActionResult> ToggleUserLockStatus(int userId, bool isLocked)
        {
            var result = await _userAdminService.ToggleUserLockStatusAsync(userId, isLocked);

            if (!result)
                return BadRequest(new { message = "Not found user or user was deleted" });

            return Ok(new { message = isLocked ? "Lock user success!" : "Unlock user success!" });
        }

        [HttpPatch("delete")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var result = await _userAdminService.SoftDeleteUserAsync(userId);

            if (!result)
                return BadRequest(new { message = "Not found user or user was deleted" });

            return Ok(new { message = "Delete user success!" });
        }
    }
}
