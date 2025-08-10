using Microsoft.AspNetCore.Mvc;
using kitapMagazaMvc.Models;
using kitapMagazaMvc.Services;

namespace kitapMagazaMvc.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly ApiService _apiService;
        
        public FavoritesController(ApiService apiService)
        {
            _apiService = apiService;
        }
        
        public async Task<IActionResult> Index(int userId = 2) // Default to user 2 for demo
        {
            ViewData["Title"] = "Favorilerim";
            
            var favorites = await _apiService.GetFavoritesAsync(userId);
            ViewBag.UserId = userId;
            
            return View(favorites);
        }
        
        [HttpPost]
        public async Task<IActionResult> Add(int userId, int kitapId)
        {
            var success = await _apiService.AddFavoriteAsync(userId, kitapId);
            
            if (success)
            {
                TempData["Success"] = "Kitap favorilere eklendi.";
            }
            else
            {
                TempData["Error"] = "Kitap zaten favorilerinizde veya bir hata oluştu.";
            }
            
            return RedirectToAction("Details", "kitaps", new { id = kitapId });
        }
        
        [HttpPost]
        public async Task<IActionResult> Remove(int userId, int kitapId)
        {
            var success = await _apiService.RemoveFavoriteAsync(userId, kitapId);
            
            if (success)
            {
                TempData["Success"] = "Kitap favorilerden kaldırıldı.";
            }
            else
            {
                TempData["Error"] = "Favori kaldırılırken bir hata oluştu.";
            }
            
            return RedirectToAction(nameof(Index), new { userId });
        }
    }
}
