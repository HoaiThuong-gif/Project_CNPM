using Microsoft.AspNetCore.Mvc;
using Project_CNPM.Area.Admin.Services;

namespace Project_CNPM.Controllers
{
    public class ThuocController : Controller
    {
        private readonly IMedicineAdminService _medicineAdminService;

        public ThuocController(IMedicineAdminService medicineAdminService)
        {
            _medicineAdminService = medicineAdminService;
        }

        public async Task<IActionResult> DanhSachThuoc()
        {
            var medicines = await _medicineAdminService.GetAllMedicinesAsync(false);
            return View(medicines.Where(x => x.IsActive).OrderBy(x => x.MedicineName));
        }

        public async Task<IActionResult> ChiTietThuoc(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(DanhSachThuoc));
            }

            var medicine = await _medicineAdminService.GetMedicineByIdAsync(id.Value);
            if (medicine == null || !medicine.IsActive)
            {
                return RedirectToAction("NotFoundPage", "Error");
            }

            return View(medicine);
        }
    }
}
