using Microsoft.AspNetCore.Mvc;
using Project_CNPM.Area.Admin.DTOs;
using Project_CNPM.Area.Admin.Services;

namespace Project_CNPM.Area.Admin.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class MedicineController : Controller
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

        [HttpGet("id")]
        public async Task<IActionResult> GetMedicineById(int id)
        {
            var medicine = await _medicineAdminService.GetMedicineByIdAsync(id);

            if (medicine == null)
                return NotFound(new { message = "Not found medicine" });

            return Ok(medicine);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateMedicine(MedicineCreateDto medicine)
        {
            var result = await _medicineAdminService.CreateMedicineAsync(medicine);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }

        [HttpPatch("update")]
        public async Task<IActionResult> UpdateMedicine(MedicineUpdateDto medicine)
        {
            var result = await _medicineAdminService.UpdateMedicineAsync(medicine);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }

        [HttpPatch("status")]
        public async Task<IActionResult> ToggleMedicineStatus(int id, bool isActive)
        {
            var result = await _medicineAdminService.ToggleMedicineStatusAsync(id, isActive);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
    }
}
