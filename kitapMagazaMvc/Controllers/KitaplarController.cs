using Microsoft.AspNetCore.Mvc;
using kitapMagazaMvc.Models;
using kitapMagazaMvc.Services;
using Newtonsoft.Json;

namespace kitapMagazaMvc.Controllers
{
    [Route("kitaps")]
    [Route("kitaplar")]
    public class KitaplarController : Controller
    {
        private readonly ApiService _apiService;
        
        public KitaplarController(ApiService apiService)
        {
            _apiService = apiService;
        }
        
        [Route("")]
        [Route("index")]
        public async Task<IActionResult> Index(int? categoryId = null, decimal? minPrice = null, decimal? maxPrice = null, string? search = null, string? sortBy = null, string? sortOrder = null)
        {
            ViewData["Title"] = "Kitaplar";
            
            var kitaplar = await _apiService.GetKitaplarAsync(categoryId, minPrice, maxPrice, search, sortBy, sortOrder);
            var categories = await _apiService.GetCategoriesAsync();
            
            ViewBag.Categories = categories;
            ViewBag.SelectedCategory = categoryId;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.Search = search;
            ViewBag.SortBy = sortBy;
            ViewBag.SortOrder = sortOrder;
            
            return View(kitaplar);
        }
        
        [Route("details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var kitap = await _apiService.GetKitapAsync(id);
            if (kitap == null)
            {
                return NotFound();
            }
            
            ViewData["Title"] = kitap.Title;
            return View(kitap);
        }
        
        [HttpPost]
        [Route("addtocart")]
        public async Task<IActionResult> AddToCart(int kitapId, int quantity = 1)
        {
            var kitap = await _apiService.GetKitapAsync(kitapId);
            if (kitap == null)
            {
                TempData["Error"] = "Kitap bulunamadı.";
                return RedirectToAction(nameof(Index));
            }
            
            if (kitap.Stock < quantity)
            {
                TempData["Error"] = "Yeterli stok bulunmuyor.";
                return RedirectToAction(nameof(Details), new { id = kitapId });
            }
            
            // Get cart from session
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson) 
                ? new Cart() 
                : JsonConvert.DeserializeObject<Cart>(cartJson) ?? new Cart();
            
            // Add to cart
            var existingItem = cart.Items.FirstOrDefault(i => i.KitapId == kitapId);
            if (existingItem != null)
            {
                if (existingItem.Quantity + quantity <= kitap.Stock)
                {
                    existingItem.Quantity += quantity;
                }
                else
                {
                    TempData["Error"] = "Sepetteki miktar ile birlikte stok yetersiz.";
                    return RedirectToAction(nameof(Details), new { id = kitapId });
                }
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    KitapId = kitap.Id,
                    Title = kitap.Title,
                    Author = kitap.Author,
                    Price = kitap.Price,
                    ImageUrl = kitap.ImageUrl,
                    Quantity = quantity
                });
            }
            
            // Save cart to session
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
            
            TempData["Success"] = "Kitap sepete eklendi.";
            return RedirectToAction(nameof(Details), new { id = kitapId });
        }
        
        [Route("cart")]
        public IActionResult Cart()
        {
            ViewData["Title"] = "Sepetim";
            
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson) 
                ? new Cart() 
                : JsonConvert.DeserializeObject<Cart>(cartJson) ?? new Cart();
            
            return View(cart);
        }
        
        [HttpPost]
        public IActionResult UpdateCart(int kitapId, int quantity)
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson) 
                ? new Cart() 
                : JsonConvert.DeserializeObject<Cart>(cartJson) ?? new Cart();
            
            var item = cart.Items.FirstOrDefault(i => i.KitapId == kitapId);
            if (item != null)
            {
                if (quantity <= 0)
                {
                    cart.Items.Remove(item);
                }
                else
                {
                    item.Quantity = quantity;
                }
                
                HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
            }
            
            return RedirectToAction(nameof(Cart));
        }
        
        [HttpPost]
        public IActionResult RemoveFromCart(int kitapId)
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson) 
                ? new Cart() 
                : JsonConvert.DeserializeObject<Cart>(cartJson) ?? new Cart();
            
            var item = cart.Items.FirstOrDefault(i => i.KitapId == kitapId);
            if (item != null)
            {
                cart.Items.Remove(item);
                HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
                TempData["Success"] = "Ürün sepetten kaldırıldı.";
            }
            
            return RedirectToAction(nameof(Cart));
        }
        
        [Route("checkout")]
        public IActionResult Checkout()
        {
            ViewData["Title"] = "Siparişi Tamamla";
            
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson) 
                ? new Cart() 
                : JsonConvert.DeserializeObject<Cart>(cartJson) ?? new Cart();
            
            if (!cart.Items.Any())
            {
                TempData["Error"] = "Sepetiniz boş.";
                return RedirectToAction(nameof(Cart));
            }
            
            return View(cart);
        }
        
        [HttpPost]
        [Route("placeorder")]
        public async Task<IActionResult> PlaceOrder(OrderDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                return View("Checkout", orderDto);
            }
            
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson) 
                ? new Cart() 
                : JsonConvert.DeserializeObject<Cart>(cartJson) ?? new Cart();
            
            if (!cart.Items.Any())
            {
                TempData["Error"] = "Sepetiniz boş.";
                return RedirectToAction(nameof(Cart));
            }
            
            // Create order
            var success = await _apiService.CreateOrderAsync(2, orderDto.ShippingAddress, cart.Items);
            
            if (success)
            {
                // Clear cart
                HttpContext.Session.Remove("Cart");
                TempData["Success"] = "Siparişiniz başarıyla oluşturuldu.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Error"] = "Sipariş oluşturulurken bir hata oluştu.";
                return View("Checkout", cart);
            }
        }
    }
}