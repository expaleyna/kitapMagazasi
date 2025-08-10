using System.ComponentModel.DataAnnotations;

namespace kitapMagazaApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public DateTime OrderDate { get; set; } = DateTime.Now;
        
        [StringLength(50)]
        public string Status { get; set; } = "Pending";
        
        [StringLength(500)]
        public string? ShippingAddress { get; set; }
        
        public decimal TotalAmount { get; set; }
        
        // Navigation Properties
        public virtual User User { get; set; } = null!;
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
