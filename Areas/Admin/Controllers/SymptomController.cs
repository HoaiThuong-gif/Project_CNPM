using Project_CNPM.Area.Admin.DTOs;
using Project_CNPM.Area.Admin.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Project_CNPM.Area.Admin.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize(Roles="Admin")]

    public class SymptomController : ControllerBase
    {
        private readonly ISymptomAdminService _symptomAdminService;

        public SymptomController(ISymptomAdminService symptomAdminService)
        {
            _symptomAdminService = symptomAdminService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSymptom()
        {
            var symptoms = await _symptomAdminService.GetAllSymptomsAsync();
            
            return Ok(symptoms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSymptomById(int id)
        {
            var symptom = await _symptomAdminService.GetSymptomByIdAsync(id);

             if (symptom == null)
                return NotFound(new { message = "Không tìm thấy triệu chứng"});

            return Ok(symptom);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateSymptom([FromBody] SymptomCreateUpdateDto symptom)
        {
            var result = await _symptomAdminService.CreateSymptomAsync(symptom);

            if(!result.IsSuccess)
                return BadRequest(new {message = result.Message});
            
            return Ok(new {message = result.Message});
        }

        [HttpPatch("update")]
        public async Task<IActionResult> UpdateSymptom([FromBody] SymptomCreateUpdateDto symptom)
        {
            var result = await _symptomAdminService.UpdateSymptomAsync(symptom);

             if(!result.IsSuccess)
                return BadRequest(new {message = result.Message});
            
            return Ok(new {message = result.Message});
        }

        [HttpPatch("toggle")]
        public async Task<IActionResult> DeleteSymptom(int id)
        {
            var result = await _symptomAdminService.ToggleSymptomAsync(id);

             if(!result.IsSuccess)
                return BadRequest(new {message = result.Message});
            
            return Ok(new {message = result.Message});
        }   
    }
}