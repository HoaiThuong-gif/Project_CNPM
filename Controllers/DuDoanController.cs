using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_CNPM.Area.Admin.Services;

namespace Project_CNPM.Controllers
{
    [Authorize]
    public class DuDoanController : Controller
    {
        private readonly IDiseaseAdminService _diseaseAdminService;
        private readonly IMedicineAdminService _medicineAdminService;

        public DuDoanController(
            IDiseaseAdminService diseaseAdminService,
            IMedicineAdminService medicineAdminService)
        {
            _diseaseAdminService = diseaseAdminService;
            _medicineAdminService = medicineAdminService;
        }

        public async Task<IActionResult> TongQuan()
        {
            var diseases = await _diseaseAdminService.GetAllDiseasesAsync(false);
            var medicines = await _medicineAdminService.GetAllMedicinesAsync(false);

            ViewBag.ActiveDiseaseCount = diseases.Count(x => x.IsActive);
            ViewBag.ActiveMedicineCount = medicines.Count(x => x.IsActive);

            return View();
        }

        public IActionResult NhapThongTinBenh()
        {
            return View();
        }

        public IActionResult KetQuaDuDoan()
        {
            return View();
        }

        public IActionResult LichSuTraCuu()
        {
            return View();
        }

    }
}
