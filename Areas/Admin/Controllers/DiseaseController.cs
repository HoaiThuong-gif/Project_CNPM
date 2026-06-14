using Project_CNPM.Area.Admin.DTOs;
using Project_CNPM.Area.Admin.Services;
using Microsoft.AspNetCore.Mvc;

namespace Project_CNPM.Area.Admin.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class DiseaseController : Controller
    {
        private readonly IDiseaseAdminService _diseaseAdminService;

        public DiseaseController(IDiseaseAdminService diseaseAdminService)
        {
            _diseaseAdminService = diseaseAdminService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDiseases()
        {
            var diseases = await _diseaseAdminService.GetAllDiseasesAsync(true);
            
            return Ok(diseases);
        }


        [HttpGet("id")]
        public async Task<IActionResult> GetDiseasesById(int id)
        {
            var diseases = await _diseaseAdminService.GetDiseaseByIdAsync(id);

            if (diseases == null)
                return NotFound(new { message = "Không tìm thấy thuốc"});

            return Ok(diseases);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateDisease(DiseaseCreateDto disease)
        {
            var result = await _diseaseAdminService.CreateDiseaseAsync(disease);
           
            if (!result.IsSuccess)
                return BadRequest(new {message = result.Message});
            
            return Ok(new {message = result.Message});
        }

        [HttpPatch("Update")]
        public async Task<IActionResult> UpdateDisease(DiseaseUpdateDto disease)
        {
            var result = await _diseaseAdminService.UpdateDiseaseAsync(disease);

            if (!result.IsSuccess)
                return BadRequest(new {message = result.Message});
            
            return Ok(new {message = result.Message});
        }

        [HttpPatch("delete")]
        public async Task<IActionResult> DeleteDisease(int id)
        {
            var result = await _diseaseAdminService.SoftDeleteDiseaseAsync(id);

            if (!result.IsSuccess)
                return BadRequest(new {message = result.Message});
            
            return Ok(new {message = result.Message});
        }
    }
}