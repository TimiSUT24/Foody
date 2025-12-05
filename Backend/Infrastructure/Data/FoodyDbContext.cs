using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class FoodyDbContext : IdentityDbContext<User,               
                             IdentityRole<Guid>,  
                             Guid,               
                             IdentityUserClaim<Guid>,
                             IdentityUserRole<Guid>,
                             IdentityUserLogin<Guid>,
                             IdentityRoleClaim<Guid>,
                             IdentityUserToken<Guid>>
    {
        public FoodyDbContext(DbContextOptions<FoodyDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<SubSubCategory> SubSubCategories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<NutritionValue> NutritionValues { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ShippingInformation> ShippingInformation { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach(var relation in builder.Model.GetEntityTypes()
                .Where(rel => typeof(BaseEntity).IsAssignableFrom(rel.ClrType))) //Make sure to apply to all entities that inherit from BaseEntity
            {
                builder.Entity(relation.ClrType)
                   .Property<Guid>(nameof(BaseEntity.Id))
                   .ValueGeneratedNever();

                builder.Entity(relation.ClrType)
                    .Property<byte[]>(nameof(BaseEntity.RowVersion))
                    .IsRowVersion();

                builder.Entity(relation.ClrType)
                    .Property<DateTime>(nameof(BaseEntity.CreatedAtUtc))
                    .IsRequired();

                builder.Entity(relation.ClrType)
                    .Property<DateTime>(nameof(BaseEntity.UpdatedAtUtc))
                    .IsRequired();
            }

            builder.Entity<Product>()
                .HasMany(f => f.NutritionValues)
                .WithOne(n => n.Food!)
                .HasForeignKey(n => n.FoodId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Product>()
                .HasMany(a => a.ProductAttributes)
                .WithOne(f => f.Food)
                .HasForeignKey(f => f.FoodId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Order>()
                .HasOne(s => s.ShippingInformation)
                .WithOne(s => s.Order!)
                .HasForeignKey<ShippingInformation>(s => s.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Order>(e =>
            {
                e.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order!)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);


                e.Property(o => o.OrderStatus)
                .HasConversion<string>()
                .HasMaxLength(50);
            });                

            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Food)
                .WithMany()
                .HasForeignKey(oi => oi.FoodId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User!)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Category>()
                .HasMany(c => c.Food)
                .WithOne(p => p.Category!)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Category>()
               .HasMany(c => c.SubCategories)
               .WithOne(p => p.Category!)
               .HasForeignKey(p => p.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SubCategory>()
                .HasMany(c => c.Food)
                .WithOne(p => p.SubCategory)
                .HasForeignKey(p => p.SubCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SubCategory>()
                .HasMany(c => c.SubSubCategories)
                .WithOne(s => s.SubCategory)
                .HasForeignKey(f => f.SubCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SubSubCategory>()
                .HasMany(f => f.Food)
                .WithOne(p => p.SubSubCategory)
                .HasForeignKey(f => f.SubSubCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User>()
                .HasMany(u => u.RefreshTokens)
                .WithOne(rt => rt.User!)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RefreshToken>()
                .Property(r => r.Token)
                .IsRequired()
                .HasMaxLength(256);

            builder.Entity<RefreshToken>()
                .HasIndex(r => r.Token)
                .IsUnique();

            builder.Entity<RefreshToken>()
                .Property(r => r.ExpiryDate)
                .IsRequired();

            builder.Entity<ShippingInformation>(entity =>
            {
                entity.Property(e => e.Email)
                      .IsRequired()
                      .HasMaxLength(256);
                entity.Property(e => e.FirstName)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(e => e.LastName)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(e => e.Adress)
                      .IsRequired()
                      .HasMaxLength(200);
                entity.Property(e => e.City)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(e => e.PostalCode)
                      .IsRequired()
                      .HasMaxLength(10);
                entity.Property(e => e.PhoneNumber)
                      .IsRequired()
                      .HasMaxLength(20);
                entity.Property(e => e.State)
                        .IsRequired()
                        .HasMaxLength(50);
            });

        }
    }
}
