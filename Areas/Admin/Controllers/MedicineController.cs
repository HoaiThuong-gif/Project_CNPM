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
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineAdminService _medicineAdminService;

        public MedicineController(IMedicineAdminService medicineAdminService)
        {
            _medicineAdminService = medicineAdminService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMedicines()
        {
            var medicines = await _medicineAdminService.GetAllMedicinesAsync(true);
            return Ok(medicines);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMedicineById(int id)
        {
            var medicine = await _medicineAdminService.GetMedicineByIdAsync(id);

            if (medicine == null)
                return NotFound(new { message = "Not found medicine" });

            return Ok(medicine);
        }

        [HttpPost("create")]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> CreateMedicine([FromBody] MedicineCreateDto medicine)
        {
            var result = await _medicineAdminService.CreateMedicineAsync(medicine);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }

        [HttpPatch("update")]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> UpdateMedicine([FromBody] MedicineUpdateDto medicine)
        {
            var result = await _medicineAdminService.UpdateMedicineAsync(medicine);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }

        [HttpPatch("toggle")]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> ToggleMedicineStatus(int id, bool isActive)
        {
            var result = await _medicineAdminService.ToggleMedicineStatusAsync(id, isActive);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
    }
}
