using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class InventoryAppDbContext : IdentityDbContext<ApplicationUser>
    {
        public InventoryAppDbContext(DbContextOptions<InventoryAppDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<ProductStock> ProductStocks { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchOrderItem> PurchaseOrderItems { get; set; }
        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DbSet<SalesOrderItem> SalesOrderItems { get; set; }
        public DbSet<InventoryMoment> InventoryMoments { get; set; }
        public DbSet<Location> Locations { get; set; } 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var entity in modelBuilder.Model.GetEntityTypes()) 
            {
                if (typeof(BaseEntity).IsAssignableFrom(entity.ClrType)) 
                {
                    modelBuilder.Entity(entity.ClrType)
                        .Property<DateTime>("CreatedAt")
                        .HasDefaultValueSql("GETUTCDate()");
                }
            }
            modelBuilder.Entity<Product>()
              .HasIndex(p => p.SKU)
                .IsUnique();
            modelBuilder.Entity<Product>()
                .Property(p => p.PurchasePrice).HasPrecision(18, 2);
            modelBuilder.Entity<Product>()
                .Property(p => p.SalePrice).HasPrecision(18, 2);
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

        

            modelBuilder.Entity<ProductStock>()
                .HasIndex(x => new { x.ProductId, x.LocationId })
                .IsUnique();
            modelBuilder.Entity<ProductStock>()
                .Property(x => x.Qty).HasPrecision(18,3);
            
            modelBuilder.Entity<ProductStock>()
                .HasOne(s => s.Product)
                .WithMany(p => p.Stocks)
                .HasForeignKey(s =>s.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProductStock>()
                .HasOne(s => s.Location)
                .WithMany(l=> l.Stocks)
                .HasForeignKey(s => s.LocationId)
            .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(o=> o.Vendor)
                .WithMany(v=> v.PurchaseOrder)
                .HasForeignKey(o=> o.VendorId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PurchaseOrder>()
                .Property(o => o.Discount).HasPrecision(18, 2);
            modelBuilder.Entity<PurchaseOrder>()
                .Property(o => o.NonVendorCosts).HasPrecision(18, 2);
            modelBuilder.Entity<PurchOrderItem>()
               .HasOne(i => i.PurchOrder)
               .WithMany(o => o.Items)
               .HasForeignKey(i => i.PurchOrderId)
               .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<PurchOrderItem>()
                .HasOne(i => i.Product)
                .WithMany(p => p.PurchaseOrderItems)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PurchOrderItem>()
                .Property(i => i.Qty).HasPrecision(18, 3);
            modelBuilder.Entity<PurchOrderItem>()
                .Property(i => i.UnitPrice).HasPrecision(18, 2);
            modelBuilder.Entity<PurchOrderItem>()
                .Property(i => i.Discount).HasPrecision(18, 2);
            modelBuilder.Entity<SalesOrder>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.SalesOrders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SalesOrder>()
                .Property(o => o.Discount).HasPrecision(18, 2);
            modelBuilder.Entity<SalesOrderItem>()
                .HasOne(i => i.SalesOrder)
                .WithMany(o => o.SalesOrderItems)
                .HasForeignKey(i => i.SalesOrderId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<SalesOrderItem>()
                .HasOne(i => i.Product)
                .WithMany(p => p.SalesOrderItems)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SalesOrderItem>()
                .Property(i => i.Qty).HasPrecision(18, 3);
            modelBuilder.Entity<SalesOrderItem>()
                .Property(i => i.UnitPrice).HasPrecision(18, 2);
            modelBuilder.Entity<SalesOrderItem>()
                .Property(i => i.Discount).HasPrecision(18, 2);
            modelBuilder.Entity<InventoryMoment>()
                .HasOne(m=>m.Location)
                .WithMany()
                .HasForeignKey(m=> m.LocationId)
                .OnDelete(DeleteBehavior.SetNull);
            
        }
    }
}
