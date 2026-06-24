using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_CNPM.Area.Admin.DTOs;
using Project_CNPM.Area.Admin.Services;

namespace Project_CNPM.Area.Admin.Controllers
{
    [Area("Admin")]
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize]
    public class DiseaseController : ControllerBase
    {
        private readonly IDiseaseAdminService _diseaseAdminService;

        public DiseaseController(IDiseaseAdminService diseaseAdminService)
        {
            _diseaseAdminService = diseaseAdminService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDiseases()
        {
            var isAdmin = User.IsInRole("Admin");
            var diseases = await _diseaseAdminService.GetAllDiseasesAsync(isAdmin);

            if (!isAdmin)
            {
                diseases = diseases.Where(d => d.IsActive);
            }

            return Ok(diseases);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDiseasesById(int id)
        {
            var disease = await _diseaseAdminService.GetDiseaseByIdAsync(id);

            if (disease == null || (!User.IsInRole("Admin") && !disease.IsActive))
                return NotFound(new { message = "Khong tim thay benh" });

            return Ok(disease);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateDisease([FromBody] DiseaseCreateDto disease)
        {
            var result = await _diseaseAdminService.CreateDiseaseAsync(disease);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("update")]
        public async Task<IActionResult> UpdateDisease([FromBody] DiseaseUpdateDto disease)
        {
            var result = await _diseaseAdminService.UpdateDiseaseAsync(disease);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("toggle")]
        public async Task<IActionResult> DeleteDisease(int id)
        {
            var result = await _diseaseAdminService.ToggleDiseaseAsync(id);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
    }
}
