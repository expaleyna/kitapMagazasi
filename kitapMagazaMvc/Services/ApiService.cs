using kitapMagazaMvc.Models;
using Newtonsoft.Json;
using System.Text;

namespace kitapMagazaMvc.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        
        public ApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7000";
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }
        
        // Kitaplar
        public async Task<List<KitapDto>> GetKitaplarAsync(int? categoryId = null, decimal? minPrice = null, decimal? maxPrice = null, string? search = null, string? sortBy = null, string? sortOrder = null)
        {
            var queryParams = new List<string>();
            if (categoryId.HasValue) queryParams.Add($"categoryId={categoryId}");
            if (minPrice.HasValue) queryParams.Add($"minPrice={minPrice}");
            if (maxPrice.HasValue) queryParams.Add($"maxPrice={maxPrice}");
            if (!string.IsNullOrEmpty(search)) queryParams.Add($"search={Uri.EscapeDataString(search)}");
            if (!string.IsNullOrEmpty(sortBy)) queryParams.Add($"sortBy={sortBy}");
            if (!string.IsNullOrEmpty(sortOrder)) queryParams.Add($"sortOrder={sortOrder}");
            
            var queryString = queryParams.Any() ? "?" + string.Join("&", queryParams) : "";
            var response = await _httpClient.GetAsync($"/api/kitaplar{queryString}");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<KitapDto>>(json) ?? new List<KitapDto>();
            }
            
            return new List<KitapDto>();
        }
        
        public async Task<KitapDto?> GetKitapAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/kitaplar/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<KitapDto>(json);
            }
            
            return null;
        }
        
        // Categories
        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            var response = await _httpClient.GetAsync("/api/categories");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<CategoryDto>>(json) ?? new List<CategoryDto>();
            }
            
            return new List<CategoryDto>();
        }
        
        public async Task<CategoryDto?> GetCategoryAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/categories/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CategoryDto>(json);
            }
            
            return null;
        }
        
        // Users
        public async Task<List<UserDto>> GetUsersAsync()
        {
            var response = await _httpClient.GetAsync("/api/users");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<UserDto>>(json) ?? new List<UserDto>();
            }
            
            return new List<UserDto>();
        }
        
        public async Task<UserDto?> LoginAsync(string email, string password)
        {
            var loginData = new { Email = email, Password = password };
            var json = JsonConvert.SerializeObject(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("/api/users/login", content);
            
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserDto>(responseJson);
            }
            
            return null;
        }

        public async Task<bool> RegisterAsync(UserCreateDto userDto)
        {
            var json = JsonConvert.SerializeObject(userDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/users", content);
            return response.IsSuccessStatusCode;
        }
        
        // Orders
        public async Task<bool> CreateOrderAsync(int userId, string? shippingAddress, List<CartItem> cartItems)
        {
            var orderData = new
            {
                UserId = userId,
                ShippingAddress = shippingAddress,
                OrderItems = cartItems.Select(item => new
                {
                    kitapId = item.KitapId,
                    Quantity = item.Quantity
                }).ToList()
            };
            
            var json = JsonConvert.SerializeObject(orderData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("/api/orders", content);
            return response.IsSuccessStatusCode;
        }
        
        // Favorites
        public async Task<List<FavoriteDto>> GetFavoritesAsync(int userId)
        {
            var response = await _httpClient.GetAsync($"/api/favorites?userId={userId}");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<FavoriteDto>>(json) ?? new List<FavoriteDto>();
            }
            
            return new List<FavoriteDto>();
        }
        
        public async Task<bool> AddFavoriteAsync(int userId, int kitapId)
        {
            var favoriteData = new { UserId = userId, kitapId = kitapId };
            var json = JsonConvert.SerializeObject(favoriteData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("/api/favorites", content);
            return response.IsSuccessStatusCode;
        }
        
        public async Task<bool> RemoveFavoriteAsync(int userId, int kitapId)
        {
            var response = await _httpClient.DeleteAsync($"/api/favorites/user/{userId}/kitap/{kitapId}");
            return response.IsSuccessStatusCode;
        }

        // Admin Dashboard
        public async Task<AdminDashboardDto?> GetAdminDashboardAsync()
        {
            var response = await _httpClient.GetAsync("/api/admin/dashboard");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AdminDashboardDto>(json);
            }

            return null;
        }

        // Admin kitap Management
        public async Task<bool> CreatekitapAsync(CreatekitapDto kitap)
        {
            var json = JsonConvert.SerializeObject(kitap);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/admin/kitaps", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdatekitapAsync(int id, UpdatekitapDto kitap)
        {
            var json = JsonConvert.SerializeObject(kitap);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"/api/admin/kitaps/{id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeletekitapAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/admin/kitaps/{id}");
            return response.IsSuccessStatusCode;
        }

        // Admin Category Management
        public async Task<bool> CreateCategoryAsync(CreateCategoryDto category)
        {
            var json = JsonConvert.SerializeObject(category);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/admin/categories", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCategoryAsync(int id, UpdateCategoryDto category)
        {
            var json = JsonConvert.SerializeObject(category);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"/api/admin/categories/{id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/admin/categories/{id}");
            return response.IsSuccessStatusCode;
        }

        // Admin User Management
        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var response = await _httpClient.GetAsync("/api/admin/users");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<UserDto>>(json) ?? new List<UserDto>();
            }

            return new List<UserDto>();
        }

        public async Task<bool> UpdateUserRoleAsync(int id, string role)
        {
            var updateData = new { Role = role };
            var json = JsonConvert.SerializeObject(updateData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"/api/admin/users/{id}/role", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/admin/users/{id}");
            return response.IsSuccessStatusCode;
        }

        // Admin Order Management
        public async Task<List<AdminOrderDto>> GetAllOrdersAsync()
        {
            var response = await _httpClient.GetAsync("/api/admin/orders");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<AdminOrderDto>>(json) ?? new List<AdminOrderDto>();
            }

            return new List<AdminOrderDto>();
        }

        public async Task<bool> UpdateOrderStatusAsync(int id, string status)
        {
            var updateOrderStatusDto = new { Status = status };
            var json = JsonConvert.SerializeObject(updateOrderStatusDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"/api/admin/orders/{id}/status", content);
            return response.IsSuccessStatusCode;
        }
    }
}
