using Microsoft.AspNetCore.Mvc;
using kitapMagazaMvc.Services;
using kitapMagazaMvc.Models;

namespace kitapMagazaMvc.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApiService _apiService;

        public AuthController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Zaten giriş yapmışsa anasayfaya yönlendir
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["Title"] = "Giriş Yap";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                TempData["Error"] = "E-posta ve şifre alanları gereklidir.";
                return View();
            }

            try
            {
                var user = await _apiService.LoginAsync(email, password);
                if (user != null)
                {
                    // Session'a kullanıcı bilgilerini kaydet
                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");
                    HttpContext.Session.SetString("UserEmail", user.Email);
                    HttpContext.Session.SetString("UserRole", user.Role);

                    TempData["Success"] = $"Hoş geldiniz, {user.FirstName}!";

                    // Admin ise admin paneline yönlendir
                    if (user.Role == "Admin")
                    {
                        return RedirectToAction("Index", "Admin");
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Error"] = "E-posta veya şifre hatalı.";
                    return View();
                }
            }
            catch (Exception)
            {
                TempData["Error"] = "Giriş yapılırken bir hata oluştu. Lütfen tekrar deneyin.";
                return View();
            }
        }

        public IActionResult Logout()
        {
            // Session'ı temizle
            HttpContext.Session.Clear();
            TempData["Success"] = "Başarıyla çıkış yaptınız.";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewData["Title"] = "Kayıt Ol";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserCreateDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return View(userDto);
            }

            try
            {
                var success = await _apiService.RegisterAsync(userDto);
                if (success)
                {
                    TempData["Success"] = "Kayıt başarıyla tamamlandı. Şimdi giriş yapabilirsiniz.";
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["Error"] = "Kayıt olurken bir hata oluştu.";
                    return View(userDto);
                }
            }
            catch (Exception)
            {
                TempData["Error"] = "Kayıt olurken bir hata oluştu. Lütfen tekrar deneyin.";
                return View(userDto);
            }
        }
    }
}
