using ERP_API.DataAccess.Entities.Customers;
using ERP_API.DataAccess.Entities.Finance;
using ERP_API.DataAccess.Entities.Inventory;
using ERP_API.DataAccess.Entities.InventoryAdjustment;
using ERP_API.DataAccess.Entities.Purchasing;
using ERP_API.DataAccess.Entities.Sales;
using ERP_API.DataAccess.Entities.Suppliers;
using ERP_API.DataAccess.Entities.User;
using ERP_API.DataAccess.Entities.Warehouse;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ERP_API.DataAccess.DataContext
{
    public class ErpDBContext : DbContext
    {
        public ErpDBContext(
           DbContextOptions<ErpDBContext> options) : base(options) { }
        
        //User Management
        public DbSet<User> Users { get; set; }

        //Suppliers & Customers
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Customer> Customers { get; set; }


        //Product Management
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVariation> ProductVariations { get; set; }
        public DbSet<ProductPackage> ProductPackages { get; set; }
        public DbSet<PackageType> PackageTypes { get; set; }
        public DbSet<Category> Categories { get; set; }



        //Warehouse Management
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseStock> WarehouseStocks { get; set; }
        public DbSet<StockTransferLog> StockTransferLogs { get; set; }


        //Inventory Adjustments
        public DbSet<InventoryAdjustment> InventoryAdjustments { get; set; }


        //Purchasing
        public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }
        public DbSet<PurchaseInvoiceItem> PurchaseInvoiceItems { get; set; }
        public DbSet<PurchaseReturn> PurchaseReturns { get; set; }
        public DbSet<PurchaseReturnItem> PurchaseReturnItems { get; set; }


        //Sales
        public DbSet<SalesInvoice> SalesInvoices { get; set; }
        public DbSet<SalesInvoiceItem> SalesInvoiceItems { get; set; }
        public DbSet<SalesReturn> SalesReturns { get; set; }
        public DbSet<SalesReturnItem> SalesReturnItems { get; set; }



        //Finance
        public DbSet<Safe> Safes { get; set; }
        public DbSet<PaymentPermission> PaymentPermissions { get; set; }
        public DbSet<ReceiptPermission> ReceiptPermissions { get; set; }
        public DbSet<ExpenseType> ExpenseTypes { get; set; }
        public DbSet<RevenueSource> RevenueSources { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // User Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(256);
            });


            //Supplier Configuration
            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).HasMaxLength(256);
            });

            //Customer Configuration
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).HasMaxLength(256);
            });


            //Product Configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(256);
                entity.HasOne(e => e.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull);
            });


            //ProductVariation Configuration
            modelBuilder.Entity<ProductVariation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SKU).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.SKU).IsUnique();
                entity.HasOne(e => e.Product)
                    .WithMany(p => p.Variations)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            //ProductPackage Configuration
            modelBuilder.Entity<ProductPackage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Barcode).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Barcode).IsUnique();
                entity.Property(e => e.PurchasePrice).HasPrecision(18, 2);
                entity.Property(e => e.SalesPrice).HasPrecision(18, 2);
                entity.Property(e => e.QinP).HasPrecision(18, 4);
            });

      
            //WarehouseStock Configuration
            modelBuilder.Entity<WarehouseStock>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity).HasPrecision(18, 4);
                entity.Property(e => e.MinStockLevel).HasPrecision(18,4);
                entity.HasIndex(e => new { e.WarehouseId, e.ProductPackageId }).IsUnique();
            });


            //PurchaseInvoice Configuration
            modelBuilder.Entity<PurchaseInvoice>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.InvoiceNumber).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.InvoiceNumber).IsUnique();
                entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
                entity.Property(e => e.NetAmount).HasPrecision(18, 2);
                entity.HasOne(e => e.Supplier)
                    .WithMany(s => s.PurchaseInvoices)
                    .HasForeignKey(e => e.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.User)
                    .WithMany(u => u.PurchaseInvoices)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            //SalesInvoice Configuration
            modelBuilder.Entity<SalesInvoice>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.InvoiceNumber).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.InvoiceNumber).IsUnique();
                entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
                entity.Property(e => e.NetAmount).HasPrecision(18, 2);
                entity.HasOne(e => e.Customer)
                    .WithMany(c => c.SalesInvoices)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.User)
                    .WithMany(u => u.SalesInvoices)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            //Safe Configuration
            modelBuilder.Entity<Safe>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(256);
                entity.Property(e => e.OpeningBalance).HasPrecision(18, 2);
                entity.Property(e => e.CurrentBalance).HasPrecision(18, 2);
            });


            //PaymentPermission Configuration
            modelBuilder.Entity<PaymentPermission>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasPrecision(18, 2);
                entity.HasOne(e => e.Safe)
                    .WithMany(s => s.Payments)
                    .HasForeignKey(e => e.SafeId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Supplier)
                    .WithMany(s => s.Payments)
                    .HasForeignKey(e => e.SupplierId)
                    .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.Customer)
                    .WithMany(c => c.Payments)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            //ReceiptPermission Configuration
            modelBuilder.Entity<ReceiptPermission>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasPrecision(18, 2);
                entity.HasOne(e => e.Safe)
                    .WithMany(s => s.Receipts)
                    .HasForeignKey(e => e.SafeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            //InventoryAdjustment Configuration
            modelBuilder.Entity<InventoryAdjustment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OldQuantity).HasPrecision(18,4);
                entity.Property(e => e.NewQuantity).HasPrecision(18,4);
                entity.Property(e => e.Difference).HasPrecision(18,4);
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<StockTransferLog>()
                .Property(l => l.Quantity)
                .HasColumnType("decimal(18,4)");

            modelBuilder.Entity<StockTransferLog>()
                .HasOne(l => l.FromWarehouse)
                .WithMany()
                .HasForeignKey(l => l.FromWarehouseId)
                .OnDelete(DeleteBehavior.Restrict); // <--- Crucial Change

            modelBuilder.Entity<StockTransferLog>()
                .HasOne(l => l.ToWarehouse)
                .WithMany()
                .HasForeignKey(l => l.ToWarehouseId)
                .OnDelete(DeleteBehavior.Restrict); // <--- Crucial Change

            

         }
    }


}