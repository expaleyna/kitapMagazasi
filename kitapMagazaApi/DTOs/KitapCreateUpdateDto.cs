using System.ComponentModel.DataAnnotations;

namespace kitapMagazaApi.DTOs
{
    public class KitapCreateDto
    {
        [Required(ErrorMessage = "Kitap başlığı gereklidir")]
        [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yazar adı gereklidir")]
        [StringLength(100, ErrorMessage = "Yazar adı en fazla 100 karakter olabilir")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "Açıklama gereklidir")]
        [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fiyat gereklidir")]
        [Range(0.01, 9999.99, ErrorMessage = "Fiyat 0.01 ile 9999.99 arasında olmalıdır")]
        public decimal Price { get; set; }

        [StringLength(500, ErrorMessage = "Resim URL'si en fazla 500 karakter olabilir")]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Stok miktarı gereklidir")]
        [Range(0, int.MaxValue, ErrorMessage = "Stok miktarı 0 veya pozitif olmalıdır")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Kategori seçilmelidir")]
        public int CategoryId { get; set; }
    }

    public class UpdatekitapDto
    {
        [Required(ErrorMessage = "Kitap başlığı gereklidir")]
        [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yazar adı gereklidir")]
        [StringLength(100, ErrorMessage = "Yazar adı en fazla 100 karakter olabilir")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "Açıklama gereklidir")]
        [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fiyat gereklidir")]
        [Range(0.01, 9999.99, ErrorMessage = "Fiyat 0.01 ile 9999.99 arasında olmalıdır")]
        public decimal Price { get; set; }

        [StringLength(500, ErrorMessage = "Resim URL'si en fazla 500 karakter olabilir")]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Stok miktarı gereklidir")]
        [Range(0, int.MaxValue, ErrorMessage = "Stok miktarı 0 veya pozitif olmalıdır")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Kategori seçilmelidir")]
        public int CategoryId { get; set; }
    }

    public class KitapResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
