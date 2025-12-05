using ERP_API.DataAccess.Entities.Customers;
using ERP_API.DataAccess.Entities.Finance;
using ERP_API.DataAccess.Entities.Inventory;
using ERP_API.DataAccess.Entities.InventoryAdjustment;
using ERP_API.DataAccess.Entities.Purchasing;
using ERP_API.DataAccess.Entities.Sales;
using ERP_API.DataAccess.Entities.Suppliers;
using ERP_API.DataAccess.Entities.User;
using ERP_API.DataAccess.Entities.Warehouse;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
    public class ErpDBContext : IdentityDbContext<AppUser>
    {
        public ErpDBContext(
           DbContextOptions<ErpDBContext> options) : base(options) { }
        
       
        //App User
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppUserPermission> AppUserPermissions { get; set; }


        //Finance
        public DbSet<MainSafe> MainSafes { get; set; }
        public DbSet<MainSafeLedgerEntry> MainSafeLedgerEntries { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ProfitSource> ProfitSources { get; set; }


        //Suppliers & Customers
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerTransaction> CustomerTransactions { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierTransaction> SupplierTransactions { get; set; }





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




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
                entity.Property(e => e.UserId).IsRequired();
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
                entity.Property(e => e.UserId).IsRequired();
            });



    //////////////////////////////////////////////////////////////////////////
            // ProductPackage Prices
            modelBuilder.Entity<ProductPackage>()
                .Property(p => p.PurchasePrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ProductPackage>()
                .Property(p => p.SalesPrice)
                .HasColumnType("decimal(18,2)");


            modelBuilder.Entity<ProductPackage>()
                .Property(p => p.QinP)
                .HasColumnType("decimal(18,4)");

            // WarehouseStock Quantities
            modelBuilder.Entity<WarehouseStock>()
                .Property(w => w.Quantity)
                .HasColumnType("decimal(18,4)");

            modelBuilder.Entity<WarehouseStock>()
                .Property(w => w.MinStockLevel)
                .HasColumnType("decimal(18,4)");

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

            modelBuilder.Entity<InventoryAdjustment>().Property(a => a.OldQuantity).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<InventoryAdjustment>().Property(a => a.NewQuantity).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<InventoryAdjustment>().Property(a => a.Difference).HasColumnType("decimal(18,4)");
        }
    }


}