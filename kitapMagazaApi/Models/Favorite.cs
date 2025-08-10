using System.ComponentModel.DataAnnotations;

namespace kitapMagazaApi.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public int KitapId { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        // Navigation Properties
        public virtual User User { get; set; } = null!;
        public virtual Kitap Kitap { get; set; } = null!;
    }
}
