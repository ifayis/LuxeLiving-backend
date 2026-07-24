using FurnitureShop.Domain.Enitities;
using FurnitureShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FurnitureShop.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        public DbSet<Category> Categories => Set<Category>();

        public DbSet<Product> Products => Set<Product>();

        public DbSet<Cart> Carts => Set<Cart>();

        public DbSet<CartItem> CartItems => Set<CartItem>();

        public DbSet<Order> Orders => Set<Order>();

        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        public DbSet<Wishlist> Wishlists => Set<Wishlist>();

        public DbSet<WishlistItem> WishlistItems => Set<WishlistItem>();

        public DbSet<ShippingAddress> ShippingAddresses => Set<ShippingAddress>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureUsers(modelBuilder);
            ConfigureProducts(modelBuilder);
            ConfigureCart(modelBuilder);
            ConfigureWishlist(modelBuilder);
        }

        private static void ConfigureUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(x => x.Email)
                    .IsUnique();

                entity.HasIndex(x => x.RefreshToken);

                entity.HasIndex(x => x.PasswordResetToken);

                entity.HasIndex(x => x.EmailVerificationToken);

                entity.Property(x => x.FullName)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(x => x.Email)
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(x => x.PasswordHash)
                    .IsRequired();

                entity.Property(x => x.Role)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(x => x.UpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });
        }

        private static void ConfigureProducts(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Uncomment when Product.Price is decimal.
            //
            // modelBuilder.Entity<Product>()
            //     .Property(x => x.Price)
            //     .HasPrecision(18,2);
        }

        private static void ConfigureCart(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasIndex(x => x.UserId)
                    .IsUnique();

                entity.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(x => x.UpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(x => x.LastActivityAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(x => x.IsCheckedOut)
                    .HasDefaultValue(false);

                entity.Property(x => x.RowVersion)
                    .IsRowVersion();

                entity.HasMany(x => x.Items)
                    .WithOne(x => x.Cart)
                    .HasForeignKey(x => x.CartId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasIndex(x => x.CartId);

                entity.HasIndex(x => x.ProductId);

                entity.HasIndex(x => new
                {
                    x.CartId,
                    x.ProductId
                })
                .IsUnique();

                entity.HasOne(x => x.Product)
                    .WithMany()
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(x => x.Quantity)
                    .IsRequired();
            });
        }
        private static void ConfigureWishlist(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Wishlist>()
                .HasMany(x => x.Items)
                .WithOne(x => x.Wishlist)
                .HasForeignKey(x => x.WishlistId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WishlistItem>()
                .HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}