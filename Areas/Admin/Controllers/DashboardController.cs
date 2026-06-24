using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_CNPM.Area.Admin.Services;

namespace Project_CNPM.Area.Admin.Controllers
{
    [Area("Admin")]
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles="Admin")]
    public class DashboardController : ControllerBase
    {   
        private readonly IDashboardAdminService _dashboardAndmin;

        public DashboardController(IDashboardAdminService dashboardAdminService)
        {
            _dashboardAndmin = dashboardAdminService;
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            var statistics = await _dashboardAndmin.GetSystemStatisticsAsync();
            
            return Ok(statistics);
        }
    }

}
