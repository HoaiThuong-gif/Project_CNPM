using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_CNPM.Area.Admin.DTOs;
using Project_CNPM.Area.Admin.Services;

namespace Project_CNPM.Area.Admin.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize(Roles="Admin")]
    public class SafetyWarningController : ControllerBase
    {
        private readonly ISafetyWarningAdminService _safetyWarningAdminService;

        public SafetyWarningController(ISafetyWarningAdminService safetyWarningAdminService)
        {
            _safetyWarningAdminService = safetyWarningAdminService;
        }

        [HttpGet("allergies")]
        public async Task<IActionResult> GetAllAllergies()
        {
            var allergies = await _safetyWarningAdminService.GetAllAllergiesAsync();
            return Ok(allergies);
        }

        [HttpPost("allergies/create")]
        public async Task<IActionResult> CreateAllergy([FromBody] AllergyCreateUpdateDto allergy)
        {
            var result = await _safetyWarningAdminService.CreateAllergyAsync(allergy);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }

        [HttpPatch("allergies/update")]
        public async Task<IActionResult> UpdateAllergy([FromBody] AllergyCreateUpdateDto allergy)
        {
            var result = await _safetyWarningAdminService.UpdateAllergyAsync(allergy);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }

        [HttpPatch("allergies/toggle")]
        public async Task<IActionResult> ToggleAllergyStatus(int id, bool isActive)
        {
            var result = await _safetyWarningAdminService.ToggleAllergyStatusAsync(id, isActive);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }

        [HttpGet("background-diseases")]
        public async Task<IActionResult> GetAllBackgroundDiseases()
        {
            var backgroundDiseases = await _safetyWarningAdminService.GetAllBackgroundDiseasesAsync();
            return Ok(backgroundDiseases);
        }

        [HttpPost("background-diseases/create")]
        public async Task<IActionResult> CreateBackgroundDisease([FromBody] BackgroundDiseaseCreateUpdateDto backgroundDisease)
        {
            var result = await _safetyWarningAdminService.CreateBackgroundDiseaseAsync(backgroundDisease);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }

        [HttpPatch("background-diseases/update")]
        public async Task<IActionResult> UpdateBackgroundDisease([FromBody] BackgroundDiseaseCreateUpdateDto backgroundDisease)
        {
            var result = await _safetyWarningAdminService.UpdateBackgroundDiseaseAsync(backgroundDisease);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }

        [HttpPatch("background-diseases/toggle")]
        public async Task<IActionResult> ToggleBackgroundDiseaseStatus(int id, bool isActive)
        {
            var result = await _safetyWarningAdminService.ToggleBackgroundDiseaseStatusAsync(id, isActive);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }

        [HttpGet("drug-interactions")]
        public async Task<IActionResult> GetAllDrugInteractions()
        {
            var drugInteractions = await _safetyWarningAdminService.GetAllDrugInteractionsAsync();
            return Ok(drugInteractions);
        }

        [HttpPost("drug-interactions/create")]
        public async Task<IActionResult> CreateDrugInteraction([FromBody] DrugInteractionCreateDto drugInteraction)
        {
            var result = await _safetyWarningAdminService.CreateDrugInteractionAsync(drugInteraction);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }

        [HttpDelete("drug-interactions/{medicine1Id}/{medicine2Id}")]
        public async Task<IActionResult> DeleteDrugInteraction(int medicine1Id, int medicine2Id)
        {
            var result = await _safetyWarningAdminService.DeleteDrugInteractionAsync(medicine1Id, medicine2Id);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
    }
}
