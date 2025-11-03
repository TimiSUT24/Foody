using Domain.Models;
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
    public class DbContext : IdentityDbContext<User>
    {
        public DbContext(DbContextOptions<DbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<NutritionValue> NutritionValues { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RawMaterial> RawMaterials { get; set; }
        public DbSet<Classification> Classifications { get; set; }

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
                .HasMany(f => f.Ingredients)
                .WithOne(i => i.Food!)
                .HasForeignKey(i => i.FoodId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Product>()
                .HasMany(f => f.RawMaterials)
                .WithOne(r => r.Food!)
                .HasForeignKey(r => r.FoodId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Product>()
                .HasMany(f => f.Classifications)
                .WithOne(c => c.Food!)
                .HasForeignKey(c => c.FoodId)
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
                .HasMany(c => c.Products)
                .WithOne(p => p.Category!)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>()
                    .HasIndex(p => p.Name);

            builder.Entity<Category>()
                    .HasIndex(c => c.Name)
                    .IsUnique();

            builder.Entity<Order>()
                    .HasIndex(o => o.CreatedAtUtc);

            builder.Entity<User>()
                    .HasIndex(u => u.Email)
                    .IsUnique();

        }
    }
}
