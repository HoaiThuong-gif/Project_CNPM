using Microsoft.AspNetCore.Mvc;
using Project_CNPM.Area.Admin.Services;

namespace Project_CNPM.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDiseaseAdminService _diseaseAdminService;
        private readonly IMedicineAdminService _medicineAdminService;

        public HomeController(
            IDiseaseAdminService diseaseAdminService,
            IMedicineAdminService medicineAdminService)
        {
            _diseaseAdminService = diseaseAdminService;
            _medicineAdminService = medicineAdminService;
        }

        public async Task<IActionResult> Index()
        {
            await LoadHomeStatisticsAsync();
            return View("TrangChu");
        }

        public async Task<IActionResult> TrangChu()
        {
            await LoadHomeStatisticsAsync();
            return View();
        }

        private async Task LoadHomeStatisticsAsync()
        {
            var diseases = await _diseaseAdminService.GetAllDiseasesAsync(false);
            var medicines = await _medicineAdminService.GetAllMedicinesAsync(false);

            ViewBag.ActiveDiseaseCount = diseases.Count(x => x.IsActive);
            ViewBag.ActiveMedicineCount = medicines.Count(x => x.IsActive);
        }
    }
}
