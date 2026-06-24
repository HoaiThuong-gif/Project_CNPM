using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_CNPM.Area.Admin.DTOs;
using Project_CNPM.Area.Admin.Services;

namespace Project_CNPM.Area.Admin.Controllers
{
    [Area("Admin")]
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class MedicineDiseaseMappingController : ControllerBase
    {
        private readonly IMedicineDiseaseMappingService _mappingService;

        public MedicineDiseaseMappingController(IMedicineDiseaseMappingService mappingService)
        {
            _mappingService = mappingService;
        }

        [HttpGet("medicine/{medicineId}/diseases")]
        public async Task<IActionResult> GetDiseasesOfMedicine(int medicineId)
        {
            var data = await _mappingService.GetDiseasesOfMedicineAsync(medicineId);
            return Ok(data);
        }

        [HttpGet("disease/{diseaseId}/medicines")]
        public async Task<IActionResult> GetMedicinesOfDisease(int diseaseId)
        {
            var data = await _mappingService.GetMedicinesOfDiseaseAsync(diseaseId);
            return Ok(data);
        }

        [HttpPost("link")]
        public async Task<IActionResult> LinkMedicineWithDisease([FromBody] LinkMedicineDiseaseDto request)
        {
            var result = await _mappingService.LinkMedicineWithDiseaseAsync(
                request.MedicineId, 
                request.DiseaseId, 
                request.Priority, 
                request.TreatmentType);

            if (result.IsSuccess)
                return Ok(new { message = result.Message });

            return BadRequest(new { message = result.Message });
        }

        [HttpDelete("unlink/medicine/{medicineId}/disease/{diseaseId}")]
        public async Task<IActionResult> UnlinkMedicineFromDisease(int medicineId, int diseaseId)
        {
            var result = await _mappingService.UnlinkMedicineFromDiseaseAsync(medicineId, diseaseId);

            if (result.IsSuccess)
                return Ok(new { message = result.Message });

            return BadRequest(new { message = result.Message });
        }
    }
}
