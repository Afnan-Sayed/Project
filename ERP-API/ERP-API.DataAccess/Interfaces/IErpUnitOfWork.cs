using ERP_API.DataAccess.Entities.Customers;
using ERP_API.DataAccess.Entities.Finance;
using ERP_API.DataAccess.Entities.Inventory;
using ERP_API.DataAccess.Entities.InventoryAdjustment;
using ERP_API.DataAccess.Entities.Purchasing;
using ERP_API.DataAccess.Entities.Sales;
using ERP_API.DataAccess.Entities.Suppliers;
using ERP_API.DataAccess.Entities.User;
using ERP_API.DataAccess.Entities.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_API.DataAccess.Interfaces
{
    public interface IErpUnitOfWork
    {

        //Product Management
        IBaseRepository<Product, int> Products { get; }
        IBaseRepository<ProductVariation, int> ProductVariations { get; }
        IBaseRepository<ProductPackage, int> ProductPackages { get; }
        //IBaseRepository<Category, int> Categories { get; }
        IBaseRepository<PackageType, int> PackageTypes { get; }


        //Warehouse Management
        IBaseRepository<Warehouse, int> Warehouses { get; }
        IBaseRepository<WarehouseStock, int> WarehouseStocks { get; }
        IBaseRepository<StockTransferLog, int> StockTransferLogs { get; }

        //Inventory Adjustments
        IBaseRepository<InventoryAdjustment, int> InventoryAdjustments { get; }
 

        //User Management
        IBaseRepository<User, Guid> Users { get; }

        //Suppliers & Customers
        IBaseRepository<Supplier, Guid> Suppliers { get; }
        IBaseRepository<Customer, Guid> Customers { get; }

       

        // Purchasing
        IBaseRepository<PurchaseInvoice, int> PurchaseInvoices { get; }
        IBaseRepository<PurchaseInvoiceItem, int> PurchaseInvoiceItems { get; }
        IBaseRepository<PurchaseReturn, int> PurchaseReturns { get; }
        IBaseRepository<PurchaseReturnItem, int> PurchaseReturnItems { get; }

        //Sales
        IBaseRepository<SalesInvoice, int> SalesInvoices { get; }
        IBaseRepository<SalesInvoiceItem, int> SalesInvoiceItems { get; }
        IBaseRepository<SalesReturn, int> SalesReturns { get; }
        IBaseRepository<SalesReturnItem, int> SalesReturnItems { get; }

        //Finance
        IBaseRepository<Safe, int> Safes { get; }
        IBaseRepository<PaymentPermission, int> PaymentPermissions { get; }
        IBaseRepository<ReceiptPermission, int> ReceiptPermissions { get; }
        IBaseRepository<ExpenseType, int> ExpenseTypes { get; }
        IBaseRepository<RevenueSource, int> RevenueSources { get; }


        void SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
