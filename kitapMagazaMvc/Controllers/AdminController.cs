using Microsoft.AspNetCore.Mvc;
using kitapMagazaMvc.Services;
using kitapMagazaMvc.Models;

namespace kitapMagazaMvc.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApiService _apiService;

        public AdminController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // Admin paneline erişim kontrolü
        private bool IsAdmin()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            return userRole == "Admin";
        }

        private void CheckAdminAccess()
        {
            if (!IsAdmin())
            {
                throw new UnauthorizedAccessException("Bu sayfaya erişim yetkiniz yok.");
            }
        }

        // Dashboard
        public async Task<IActionResult> Index()
        {
            try
            {
                CheckAdminAccess();
                ViewData["Title"] = "Admin Paneli";
                
                var dashboardData = await _apiService.GetAdminDashboardAsync();
                return View(dashboardData);
            }
            catch (UnauthorizedAccessException)
            {
                TempData["Error"] = "Bu sayfaya erişim yetkiniz yok.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                TempData["Error"] = "Dashboard verisi yüklenirken hata oluştu.";
                return RedirectToAction("Index", "Home");
            }
        }

        // Kitap Yönetimi
        [Route("Admin/kitaps")]
        [Route("Admin/kitaplar")]
        public async Task<IActionResult> Kitaplar()
        {
            try
            {
                CheckAdminAccess();
                ViewData["Title"] = "Kitap Yönetimi";
                
                var kitaplar = await _apiService.GetKitaplarAsync();
                var categories = await _apiService.GetCategoriesAsync();
                
                ViewBag.Categories = categories;
                return View("Kitaplar", kitaplar);
            }
            catch (UnauthorizedAccessException)
            {
                TempData["Error"] = "Bu sayfaya erişim yetkiniz yok.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                TempData["Error"] = "Kitaplar yüklenirken hata oluştu.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Route("createkitap")]
        public async Task<IActionResult> Createkitap(CreatekitapDto kitap)
        {
            try
            {
                CheckAdminAccess();
                
                if (ModelState.IsValid)
                {
                    var success = await _apiService.CreatekitapAsync(kitap);
                    if (success)
                    {
                        TempData["Success"] = "Kitap başarıyla eklendi.";
                    }
                    else
                    {
                        TempData["Error"] = "Kitap eklenirken hata oluştu.";
                    }
                }
                else
                {
                    TempData["Error"] = "Lütfen tüm alanları doğru şekilde doldurun.";
                }
            }
            catch (UnauthorizedAccessException)
            {
                TempData["Error"] = "Bu işlemi gerçekleştirme yetkiniz yok.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Kitap eklenirken hata oluştu.";
            }

            return RedirectToAction("Kitaplar");
        }

        [HttpPost]
        [Route("updatekitap/{id}")]
        public async Task<IActionResult> Updatekitap(int id, UpdatekitapDto kitap)
        {
            try
            {
                CheckAdminAccess();
                
                if (ModelState.IsValid)
                {
                    var success = await _apiService.UpdatekitapAsync(id, kitap);
                    if (success)
                    {
                        TempData["Success"] = "Kitap başarıyla güncellendi.";
                    }
                    else
                    {
                        TempData["Error"] = "Kitap güncellenirken hata oluştu.";
                    }
                }
                else
                {
                    TempData["Error"] = "Lütfen tüm alanları doğru şekilde doldurun.";
                }
            }
            catch (UnauthorizedAccessException)
            {
                TempData["Error"] = "Bu işlemi gerçekleştirme yetkiniz yok.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Kitap güncellenirken hata oluştu.";
            }

            return RedirectToAction("Kitaplar");
        }

        [HttpPost]
        [Route("deletekitap/{id}")]
        public async Task<IActionResult> Deletekitap(int id)
        {
            try
            {
                CheckAdminAccess();
                
                var success = await _apiService.DeletekitapAsync(id);
                if (success)
                {
                    TempData["Success"] = "Kitap başarıyla silindi.";
                }
                else
                {
                    TempData["Error"] = "Kitap silinirken hata oluştu.";
                }
            }
            catch (UnauthorizedAccessException)
            {
                TempData["Error"] = "Bu işlemi gerçekleştirme yetkiniz yok.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Kitap silinirken hata oluştu.";
            }

            return RedirectToAction("Kitaplar");
        }

        // Kategori Yönetimi
        public async Task<IActionResult> Categories()
        {
            try
            {
                CheckAdminAccess();
                ViewData["Title"] = "Kategori Yönetimi";
                
                var categories = await _apiService.GetCategoriesAsync();
                return View(categories);
            }
            catch (UnauthorizedAccessException)
            {
                TempData["Error"] = "Bu sayfaya erişim yetkiniz yok.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                TempData["Error"] = "Kategoriler yüklenirken hata oluştu.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto category)
        {
            try
            {
                CheckAdminAccess();
                
                if (ModelState.IsValid)
                {
                    var success = await _apiService.CreateCategoryAsync(category);
                    if (success)
                    {
                        TempData["Success"] = "Kategori başarıyla eklendi.";
                    }
                    else
                    {
                        TempData["Error"] = "Kategori eklenirken hata oluştu.";
                    }
                }
                else
                {
                    TempData["Error"] = "Lütfen tüm alanları doğru şekilde doldurun.";
                }
            }
            catch (UnauthorizedAccessException)
            {
                TempData["Error"] = "Bu işlemi gerçekleştirme yetkiniz yok.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Kategori eklenirken hata oluştu.";
            }

            return RedirectToAction("Categories");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryDto category)
        {
            try
            {
                CheckAdminAccess();
                
                if (ModelState.IsValid)
                {
                    var success = await _apiService.UpdateCategoryAsync(id, category);
                    if (success)
                    {
                        TempData["Success"] = "Kategori başarıyla güncellendi.";
                    }
                    else
                    {
                        TempData["Error"] = "Kategori güncellenirken hata oluştu.";
                    }
                }
                else
                {
                    TempData["Error"] = "Lütfen tüm alanları doğru şekilde doldurun.";
                }
            }
            catch (UnauthorizedAccessException)
            {
                TempData["Error"] = "Bu işlemi gerçekleştirme yetkiniz yok.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Kategori güncellenirken hata oluştu.";
            }

            return RedirectToAction("Categories");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                CheckAdminAccess();
                
                var success = await _apiService.DeleteCategoryAsync(id);
                if (success)
                {
                    TempData["Success"] = "Kategori başarıyla silindi.";
                }
                else
                {
                    TempData["Error"] = "Kategori silinirken hata oluştu.";
                }
            }
            catch (UnauthorizedAccessException)
            {
                TempData["Error"] = "Bu işlemi gerçekleştirme yetkiniz yok.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Kategori silinirken hata oluştu.";
            }

            return RedirectToAction("Categories");
        }

        // Kullanıcı Yönetimi
        public async Task<IActionResult> Users()
        {
            try
            {
                CheckAdminAccess();
                ViewData["Title"] = "Kullanıcı Yönetimi";
                
                var users = await _apiService.GetAllUsersAsync();
                return View(users);
            }
            catch (UnauthorizedAccessException)
            {
                TempData["Error"] = "Bu sayfaya erişim yetkiniz yok.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                TempData["Error"] = "Kullanıcılar yüklenirken hata oluştu.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserRole(int id, string role)
        {
            try
            {
                CheckAdminAccess();
                
                var success = await _apiService.UpdateUserRoleAsync(id, role);
                if (success)
                {
                    TempData["Success"] = "Kullanıcı rolü başarıyla güncellendi.";
                }
                else
                {
                    TempData["Error"] = "Kullanıcı rolü güncellenirken hata oluştu.";
                }
            }
            catch (UnauthorizedAccessException)
            {
                TempData["Error"] = "Bu işlemi gerçekleştirme yetkiniz yok.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Kullanıcı rolü güncellenirken hata oluştu.";
            }

            return RedirectToAction("Users");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                CheckAdminAccess();
                
                var success = await _apiService.DeleteUserAsync(id);
                if (success)
                {
                    TempData["Success"] = "Kullanıcı başarıyla silindi.";
                }
                else
                {
                    TempData["Error"] = "Kullanıcı silinirken hata oluştu.";
                }
            }
            catch (UnauthorizedAccessException)
            {
                TempData["Error"] = "Bu işlemi gerçekleştirme yetkiniz yok.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Kullanıcı silinirken hata oluştu.";
            }

            return RedirectToAction("Users");
        }

        // Sipariş Yönetimi
        public async Task<IActionResult> Orders()
        {
            try
            {
                CheckAdminAccess();
                ViewData["Title"] = "Sipariş Yönetimi";
                
                var orders = await _apiService.GetAllOrdersAsync();
                return View(orders);
            }
            catch (UnauthorizedAccessException)
            {
                TempData["Error"] = "Bu sayfaya erişim yetkiniz yok.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                TempData["Error"] = "Siparişler yüklenirken hata oluştu.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Route("updateorderstatus/{id}")]
        public async Task<IActionResult> UpdateOrderStatus(int id, string status)
        {
            try
            {
                CheckAdminAccess();
                
                var success = await _apiService.UpdateOrderStatusAsync(id, status);
                if (success)
                {
                    TempData["Success"] = "Sipariş durumu başarıyla güncellendi.";
                }
                else
                {
                    TempData["Error"] = "Sipariş durumu güncellenirken hata oluştu.";
                }
            }
            catch (UnauthorizedAccessException)
            {
                TempData["Error"] = "Bu işlemi gerçekleştirme yetkiniz yok.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Sipariş durumu güncellenirken hata oluştu.";
            }

            return RedirectToAction("Orders");
        }
    }
}
