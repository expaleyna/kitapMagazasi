using System.ComponentModel.DataAnnotations;

namespace kitapMagazaApi.DTOs
{
    public class CreatekitapDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Author { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        [Required]
        [Range(0.01, 999999.99)]
        public decimal Price { get; set; }
        
        [StringLength(255)]
        public string? ImageUrl { get; set; }
        
        [Required]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
        
        [Required]
        public int CategoryId { get; set; }
    }

    public class CreateCategoryDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
    }

    public class UpdateCategoryDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
    }

    public class UpdateUserRoleDto
    {
        [Required]
        [StringLength(20)]
        public string Role { get; set; } = string.Empty;
    }

    public class UpdateOrderStatusDto
    {
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty;
    }
}
