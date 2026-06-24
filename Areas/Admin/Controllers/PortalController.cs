using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Project_CNPM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Authorize(Roles = "Admin")]
    public class PortalController : Controller
    {
        [HttpGet("")]
        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            return View("~/Areas/Admin/Views/Dashboard/Dashboard.cshtml");
        }

        [HttpGet("QuanLyBenh")]
        public IActionResult QuanLyBenh()
        {
            return View("~/Areas/Admin/Views/Disease/QuanLyBenh.cshtml");
        }

        [HttpGet("QuanLyThuoc")]
        public IActionResult QuanLyThuoc()
        {
            return View("~/Areas/Admin/Views/Medicine/QuanLyThuoc.cshtml");
        }

        [HttpGet("QuanLyNguoiDung")]
        public IActionResult QuanLyNguoiDung()
        {
            return View("~/Areas/Admin/Views/UserAdmin/QuanLyNguoiDung.cshtml");
        }

        [HttpGet("QuanLyTrieuChung")]
        public IActionResult QuanLyTrieuChung()
        {
            return View("~/Areas/Admin/Views/Symptom/QuanLyTrieuChung.cshtml");
        }

        [HttpGet("QuanLyDiUng")]
        public IActionResult QuanLyDiUng()
        {
            return View("~/Areas/Admin/Views/SafetyWarning/QuanLyDiUng.cshtml");
        }

        [HttpGet("ThongKe")]
        public IActionResult ThongKe()
        {
            return View("~/Areas/Admin/Views/SafetyWarning/ThongKe.cshtml");
        }
    }
}
