using System.ComponentModel.DataAnnotations;

namespace kitapMagazaApi.DTOs
{
    public class FavoriteCreateDto
    {
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public int KitapId { get; set; }
    }
    
    public class FavoriteResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int KitapId { get; set; }
        public string KitapTitle { get; set; } = string.Empty;
        public string KitapAuthor { get; set; } = string.Empty;
        public decimal KitapPrice { get; set; }
        public string? KitapImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
