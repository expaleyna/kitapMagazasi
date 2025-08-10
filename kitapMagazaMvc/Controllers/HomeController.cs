using Microsoft.AspNetCore.Mvc;
using kitapMagazaMvc.Models;
using kitapMagazaMvc.Services;

namespace kitapMagazaMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiService _apiService;
        
        public HomeController(ApiService apiService)
        {
            _apiService = apiService;
        }
        
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Anasayfa";
            
            var categories = await _apiService.GetCategoriesAsync();
            var featuredKitaplar = await _apiService.GetKitaplarAsync();
            
            ViewBag.Categories = categories.Take(6).ToList();
            ViewBag.FeaturedKitaplar = featuredKitaplar.Take(8).ToList();
            
            return View();
        }
        
        public IActionResult About()
        {
            ViewData["Title"] = "Hakkımızda";
            return View();
        }
        
        public IActionResult Contact()
        {
            ViewData["Title"] = "Bize Ulaşın";
            return View();
        }
        
        [HttpPost]
        public IActionResult Contact(string name, string email, string message)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(message))
            {
                TempData["Error"] = "Lütfen tüm alanları doldurunuz.";
                return View();
            }
            
            // Here you would normally send the email or save to database
            TempData["Success"] = "Mesajınız başarıyla gönderildi. En kısa sürede size dönüş yapacağız.";
            return RedirectToAction(nameof(Contact));
        }
        
        public IActionResult Privacy()
        {
            ViewData["Title"] = "Gizlilik Politikası";
            return View();
        }
    }
}
