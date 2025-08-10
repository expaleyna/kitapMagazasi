using Microsoft.AspNetCore.Mvc;
using kitapMagazaMvc.Services;

namespace kitapMagazaMvc.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApiService _apiService;
        
        public CategoriesController(ApiService apiService)
        {
            _apiService = apiService;
        }
        
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Kategoriler";
            
            var categories = await _apiService.GetCategoriesAsync();
            return View(categories);
        }
        
        public async Task<IActionResult> Details(int id)
        {
            var category = await _apiService.GetCategoryAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            
            ViewData["Title"] = category.Name;
            
            var kitaplar = await _apiService.GetKitaplarAsync(id);
            ViewBag.Kitaplar = kitaplar;
            
            return View(category);
        }
    }
}
