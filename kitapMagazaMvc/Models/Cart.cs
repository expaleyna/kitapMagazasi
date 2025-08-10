namespace kitapMagazaMvc.Models
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        
        public void AddItem(KitapDto kitap, int quantity)
        {
            var existingItem = Items.FirstOrDefault(i => i.KitapId == kitap.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                Items.Add(new CartItem
                {
                    KitapId = kitap.Id,
                    Title = kitap.Title,
                    Author = kitap.Author,
                    Price = kitap.Price,
                    ImageUrl = kitap.ImageUrl,
                    Quantity = quantity
                });
            }
        }
        
        public void UpdateQuantity(int kitapId, int quantity)
        {
            var item = Items.FirstOrDefault(i => i.KitapId == kitapId);
            if (item != null)
            {
                if (quantity <= 0)
                {
                    Items.Remove(item);
                }
                else
                {
                    item.Quantity = quantity;
                }
            }
        }
        
        public void RemoveItem(int kitapId)
        {
            var item = Items.FirstOrDefault(i => i.KitapId == kitapId);
            if (item != null)
            {
                Items.Remove(item);
            }
        }
        
        public void Clear()
        {
            Items.Clear();
        }
        
        public decimal GetTotalPrice()
        {
            return Items.Sum(i => i.TotalPrice);
        }
        
        public int GetTotalItems()
        {
            return Items.Sum(i => i.Quantity);
        }
    }
    
    public class CartItem
    {
        public int KitapId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public int Quantity { get; set; }
        
        public decimal TotalPrice => Price * Quantity;
    }
}
