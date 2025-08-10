using System.ComponentModel.DataAnnotations;

namespace kitapMagazaMvc.Models
{
    public class CreatekitapDto
    {
        [Required(ErrorMessage = "Kitap başlığı gereklidir")]
        [StringLength(200, ErrorMessage = "Kitap başlığı en fazla 200 karakter olabilir")]
        public string Title { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Yazar adı gereklidir")]
        [StringLength(100, ErrorMessage = "Yazar adı en fazla 100 karakter olabilir")]
        public string Author { get; set; } = string.Empty;
        
        [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir")]
        public string? Description { get; set; }
        
        [Required(ErrorMessage = "Fiyat gereklidir")]
        [Range(0.01, 999999.99, ErrorMessage = "Fiyat 0.01 ile 999999.99 arasında olmalıdır")]
        public decimal Price { get; set; }
        
        [StringLength(255, ErrorMessage = "Resim URL'si en fazla 255 karakter olabilir")]
        public string? ImageUrl { get; set; }
        
        [Required(ErrorMessage = "Stok miktarı gereklidir")]
        [Range(0, int.MaxValue, ErrorMessage = "Stok miktarı 0 veya daha büyük olmalıdır")]
        public int Stock { get; set; }
        
        [Required(ErrorMessage = "Kategori seçimi gereklidir")]
        public int CategoryId { get; set; }
    }

    public class UpdatekitapDto
    {
        [Required(ErrorMessage = "Kitap başlığı gereklidir")]
        [StringLength(200, ErrorMessage = "Kitap başlığı en fazla 200 karakter olabilir")]
        public string Title { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Yazar adı gereklidir")]
        [StringLength(100, ErrorMessage = "Yazar adı en fazla 100 karakter olabilir")]
        public string Author { get; set; } = string.Empty;
        
        [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir")]
        public string? Description { get; set; }
        
        [Required(ErrorMessage = "Fiyat gereklidir")]
        [Range(0.01, 999999.99, ErrorMessage = "Fiyat 0.01 ile 999999.99 arasında olmalıdır")]
        public decimal Price { get; set; }
        
        [StringLength(255, ErrorMessage = "Resim URL'si en fazla 255 karakter olabilir")]
        public string? ImageUrl { get; set; }
        
        [Required(ErrorMessage = "Stok miktarı gereklidir")]
        [Range(0, int.MaxValue, ErrorMessage = "Stok miktarı 0 veya daha büyük olmalıdır")]
        public int Stock { get; set; }
        
        [Required(ErrorMessage = "Kategori seçimi gereklidir")]
        public int CategoryId { get; set; }
    }

    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "Kategori adı gereklidir")]
        [StringLength(50, ErrorMessage = "Kategori adı en fazla 50 karakter olabilir")]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
        public string? Description { get; set; }
    }

    public class UpdateCategoryDto
    {
        [Required(ErrorMessage = "Kategori adı gereklidir")]
        [StringLength(50, ErrorMessage = "Kategori adı en fazla 50 karakter olabilir")]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
        public string? Description { get; set; }
    }

    public class AdminDashboardDto
    {
        public int TotalUsers { get; set; }
        public int Totalkitaps { get; set; }
        public int TotalOrders { get; set; }
        public int TotalCategories { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<RecentOrderDto> RecentOrders { get; set; } = new List<RecentOrderDto>();
        public List<LowStockkitapDto> LowStockkitaps { get; set; } = new List<LowStockkitapDto>();
    }

    public class RecentOrderDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
    }

    public class LowStockkitapDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Stock { get; set; }
    }

    public class AdminOrderDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string? ShippingAddress { get; set; }
        public List<AdminOrderItemDto> OrderItems { get; set; } = new List<AdminOrderItemDto>();
    }

    public class AdminOrderItemDto
    {
        public string kitapTitle { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
