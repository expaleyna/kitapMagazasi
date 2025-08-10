namespace kitapMagazaMvc.Models
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string? ShippingAddress { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }
    
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int kitapId { get; set; }
        public string kitapTitle { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
    }
    
    public class FavoriteDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int kitapId { get; set; }
        public string kitapTitle { get; set; } = string.Empty;
        public string kitapAuthor { get; set; } = string.Empty;
        public decimal kitapPrice { get; set; }
        public string? kitapImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
