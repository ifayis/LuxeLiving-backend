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

        public DbSet<Review> Reviews => Set<Review>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureUsers(modelBuilder);
            ConfigureCategories(modelBuilder);
            ConfigureProducts(modelBuilder);
            ConfigureCart(modelBuilder);
            ConfigureWishlist(modelBuilder);
            ConfigureShippingAddresses(modelBuilder);
            ConfigureOrders(modelBuilder);
            ConfigureOrderItems(modelBuilder);
            ConfigureReview(modelBuilder);

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

        private static void ConfigureCategories(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(x => x.Name)
                    .IsUnique();

                entity.HasIndex(x => x.Slug)
                    .IsUnique();

                entity.HasIndex(x => x.DisplayOrder);

                entity.Property(x => x.Name)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(x => x.Slug)
                    .HasMaxLength(80)
                    .IsRequired();

                entity.Property(x => x.Description)
                    .HasMaxLength(500);

                entity.Property(x => x.ImageUrl)
                    .HasMaxLength(500);

                entity.Property(x => x.DisplayOrder)
                    .HasDefaultValue(0);

                entity.Property(x => x.IsActive)
                    .HasDefaultValue(true);

                entity.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(x => x.UpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(x => x.RowVersion)
                    .IsRowVersion();
            });
        }

        private static void ConfigureProducts(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasIndex(x => x.Name);

                entity.HasIndex(x => x.Slug)
                    .IsUnique();

                entity.HasIndex(x => x.SKU)
                    .IsUnique();

                entity.HasIndex(x => x.CategoryId);

                entity.HasIndex(x => x.IsActive);

                entity.HasIndex(x => x.IsFeatured);

                entity.HasIndex(x => x.IsNewArrival);

                entity.HasIndex(x => x.IsBestSeller);

                entity.HasIndex(x => new
                {
                    x.CategoryId,
                    x.IsActive
                });

                entity.Property(x => x.Name)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(x => x.Slug)
                    .HasMaxLength(180)
                    .IsRequired();

                entity.Property(x => x.SKU)
                    .HasMaxLength(30)
                    .IsRequired();

                entity.Property(x => x.Description)
                    .HasMaxLength(3000);

                entity.Property(x => x.ImageUrl)
                    .HasMaxLength(500);

                entity.Property(x => x.OriginalPrice)
                    .HasPrecision(18, 2);

                entity.Property(x => x.Price)
                    .HasPrecision(18, 2);

                entity.Property(x => x.DiscountPercentage)
                    .HasPrecision(5, 2);

                entity.Property(x => x.StockQuantity)
                    .IsRequired();

                entity.Property(x => x.IsActive)
                    .HasDefaultValue(true);

                entity.Property(x => x.IsFeatured)
                    .HasDefaultValue(false);

                entity.Property(x => x.IsNewArrival)
                    .HasDefaultValue(false);

                entity.Property(x => x.IsBestSeller)
                    .HasDefaultValue(false);

                entity.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(x => x.UpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(x => x.RowVersion)
                    .IsRowVersion();

                entity.HasOne(x => x.Category)
                    .WithMany(x => x.Products)
                    .HasForeignKey(x => x.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
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
            modelBuilder.Entity<Wishlist>(entity =>
            {
                entity.HasIndex(x => x.UserId)
                    .IsUnique();

                entity.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(x => x.UpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.HasMany(x => x.Items)
                    .WithOne(x => x.Wishlist)
                    .HasForeignKey(x => x.WishlistId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<WishlistItem>(entity =>
            {
                entity.HasIndex(x => new
                {
                    x.WishlistId,
                    x.ProductId
                })
                .IsUnique();

                entity.HasIndex(x => x.ProductId);

                entity.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(x => x.RowVersion)
                    .IsRowVersion();

                entity.HasOne(x => x.Product)
                    .WithMany()
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureShippingAddresses(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShippingAddress>(entity =>
            {
                entity.HasIndex(x => x.UserId);

                entity.HasIndex(x => new
                {
                    x.UserId,
                    x.IsDefault
                });

                entity.HasIndex(x => new
                {
                    x.UserId,
                    x.CreatedAt
                });

                entity.Property(x => x.FullName)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(x => x.PhoneNumber)
                    .HasMaxLength(10)
                    .IsRequired();

                entity.Property(x => x.AddressLine1)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(x => x.AddressLine2)
                    .HasMaxLength(200);

                entity.Property(x => x.City)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(x => x.State)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(x => x.Country)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(x => x.PinCode)
                    .HasMaxLength(6)
                    .IsRequired();

                entity.Property(x => x.AddressType)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(x => x.IsDefault)
                    .HasDefaultValue(false);

                entity.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(x => x.UpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(x => x.RowVersion)
                    .IsRowVersion();
            });
        }

        private static void ConfigureOrders(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.OrderNumber)
                    .HasMaxLength(30)
                    .IsRequired();

                entity.Property(x => x.CancellationReason)
                    .HasMaxLength(500);

                entity.Property(x => x.CancelledAt);

                entity.HasIndex(x => x.OrderNumber)
                    .IsUnique();

                entity.Property(x => x.SubTotal)
                    .HasColumnType("decimal(18,2)");

                entity.Property(x => x.ShippingCharge)
                    .HasColumnType("decimal(18,2)");

                entity.Property(x => x.Discount)
                    .HasColumnType("decimal(18,2)");

                entity.Property(x => x.Tax)
                    .HasColumnType("decimal(18,2)");

                entity.Property(x => x.GrandTotal)
                    .HasColumnType("decimal(18,2)");

                entity.Property(x => x.Status)
                    .HasConversion<string>();

                entity.Property(x => x.PaymentMethod)
                    .HasConversion<string>();

                entity.Property(x => x.CreatedAt)
                    .IsRequired();

                entity.HasOne(x => x.ShippingAddress)
                    .WithMany()
                    .HasForeignKey(x => x.ShippingAddressId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(x => x.Items)
                    .WithOne(x => x.Order)
                    .HasForeignKey(x => x.OrderId);
            });
        }

        private static void ConfigureOrderItems(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.ProductName)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(x => x.ProductImageUrl)
                    .HasMaxLength(500);

                entity.Property(x => x.UnitPrice)
                    .HasColumnType("decimal(18,2)");

                entity.Property(x => x.LineTotal)
                    .HasColumnType("decimal(18,2)");
            });
        }

        private static void ConfigureReview(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Rating)
                    .IsRequired();

                entity.Property(x => x.Comment)
                    .HasMaxLength(1000);

                entity.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(x => x.Product)
                    .WithMany(x => x.Reviews)
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.User)
                    .WithMany(x => x.Reviews)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Order)
                    .WithMany(x => x.Reviews)
                    .HasForeignKey(x => x.OrderId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(x => new
                {
                    x.UserId,
                    x.ProductId
                }).IsUnique();
            });
        }
    }
}