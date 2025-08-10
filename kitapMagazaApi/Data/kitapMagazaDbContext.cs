using Microsoft.EntityFrameworkCore;
using kitapMagazaApi.Models;

namespace kitapMagazaApi.Data
{
    public class kitapMagazaDbContext : DbContext
    {
        public kitapMagazaDbContext(DbContextOptions<kitapMagazaDbContext> options) : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Kitap> Kitaplar { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Decimal configuration for prices
            modelBuilder.Entity<Kitap>()
                .Property(b => b.Price)
                .HasColumnType("decimal(10,2)");
                
            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(10,2)");
            
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(10,2)");
            
            // Relationships
            modelBuilder.Entity<Kitap>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Kitaplar)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
                
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Kitap)
                .WithMany(b => b.OrderItems)
                .HasForeignKey(oi => oi.KitapId)
                .OnDelete(DeleteBehavior.Restrict);
                
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);
                
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Kitap)
                .WithMany(b => b.Favorites)
                .HasForeignKey(f => f.KitapId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
