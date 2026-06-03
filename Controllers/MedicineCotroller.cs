using Projcet_CNPM.DTOs.Medicin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_CNPM.Data;
using Project_CNPM.Services;

namespace Project_CNPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineManagement _medicineManagement;

        public MedicineController(IMedicineManagement medicineManagement)
        {
            _medicineManagement = medicineManagement;
        }

        [HttpPost]
        public async Task<IActionResult> AddMedicine(MedicinDTO medicineDto)
        {
            if (string.IsNullOrEmpty(medicineDto.TenThuoc))
                return BadRequest(new { message = "Tên thuốc không được để trống!" });

            var result = await _medicineManagement.AddMedicineAsync(medicineDto);
            if (!result.IsSuccess) return BadRequest(new { message = result.Message });
            return Ok(new { message = result.Message });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedicine(int id, MedicinDTO medicineDto)
        {
            if (string.IsNullOrEmpty(medicineDto.TenThuoc))
                return BadRequest(new { message = "Tên thuốc không được để trống!" });

            var result = await _medicineManagement.UpdateMedicineAsync(id, medicineDto);
            if (!result.IsSuccess) return NotFound(new { message = result.Message });
            return Ok(new { message = result.Message });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicine(int id)
        {
            var result = await _medicineManagement.DeleteMedicineAsync(id);
            if (!result.IsSuccess) return NotFound(new { message = result.Message });
            return Ok(new { message = result.Message });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMedicines()
        {
            var medicines = await _medicineManagement.GetAllMedicinesAsync();
            return Ok(medicines);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMedicineById(int id)
        {
            var medicine = await _medicineManagement.GetMedicineByIdAsync(id);
            if (medicine == null) return NotFound(new { message = "Thuốc không tồn tại!" });
            return Ok(medicine);
        }
    }
}