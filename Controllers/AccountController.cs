using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Project_CNPM.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public IActionResult DangNhap(string? returnUrl = null)
        {
            var safeReturnUrl = Url.IsLocalUrl(returnUrl) ? returnUrl : string.Empty;
            ViewBag.ReturnUrl = safeReturnUrl;
            ViewBag.LoginMessage = !string.IsNullOrWhiteSpace(safeReturnUrl)
                ? "Vui lòng đăng nhập để sử dụng chức năng này."
                : string.Empty;

            return View();
        }

        [AllowAnonymous]
        public IActionResult DangKy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DangXuat()
        {
            Response.Cookies.Delete("token");
            return RedirectToAction("Index", "Home");
        }
    }
}
