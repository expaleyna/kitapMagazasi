using System.ComponentModel.DataAnnotations;

namespace kitapMagazaApi.Models
{
    public class Category
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        // Navigation Properties
        public virtual ICollection<Kitap> Kitaplar { get; set; } = new List<Kitap>();
    }
}
