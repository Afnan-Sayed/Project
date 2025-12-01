using ERP_API.DataAccess.DataContext;
using ERP_API.DataAccess.Entities.Customers;
using ERP_API.DataAccess.Entities.Finance;
using ERP_API.DataAccess.Entities.Inventory;
using ERP_API.DataAccess.Entities.InventoryAdjustment;
using ERP_API.DataAccess.Entities.Purchasing;
using ERP_API.DataAccess.Entities.Sales;
using ERP_API.DataAccess.Entities.Suppliers;
using ERP_API.DataAccess.Entities.User;
using ERP_API.DataAccess.Entities.Warehouse;
using ERP_API.DataAccess.Interfaces;
using ERP_API.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_API.DataAccess.DataContext
{
    public class ErpUnitOfWork : IErpUnitOfWork
    {
        private readonly ErpDBContext _context;

        // 1. Define Lazy fields for all repositories
        private IBaseRepository<User, Guid>? _users;
        private IBaseRepository<Supplier, Guid>? _suppliers;
        private IBaseRepository<Customer, Guid>? _customers;
        private readonly Lazy<IBaseRepository<Product, int>> _products;
        private readonly Lazy<IBaseRepository<ProductVariation, int>> _productVariations;
        private readonly Lazy<IBaseRepository<ProductPackage, int>> _productPackages;
        private readonly Lazy<IBaseRepository<PackageType, int>> _packageTypes;
        private readonly Lazy<IBaseRepository<Warehouse, int>> _warehouses;
        private readonly Lazy<IBaseRepository<WarehouseStock, int>> _warehouseStocks;
        private readonly Lazy<IBaseRepository<StockTransferLog, int>> _stockTransferLogs;
        private readonly Lazy<IBaseRepository<InventoryAdjustment, int>> _inventoryAdjustments;
        private IBaseRepository<PurchaseInvoice, int>? _purchaseInvoices;
        private IBaseRepository<PurchaseInvoiceItem, int>? _purchaseInvoiceItems;
        private IBaseRepository<PurchaseReturn, int>? _purchaseReturns;
        private IBaseRepository<PurchaseReturnItem, int>? _purchaseReturnItems;
        private IBaseRepository<SalesInvoice, int>? _salesInvoices;
        private IBaseRepository<SalesInvoiceItem, int>? _salesInvoiceItems;
        private IBaseRepository<SalesReturn, int>? _salesReturns;
        private IBaseRepository<SalesReturnItem, int>? _salesReturnItems;
        private IBaseRepository<Safe, int>? _safes;
        private IBaseRepository<PaymentPermission, int>? _paymentPermissions;
        private IBaseRepository<ReceiptPermission, int>? _receiptPermissions;
        private IBaseRepository<ExpenseType, int>? _expenseTypes;
        private IBaseRepository<RevenueSource, int>? _revenueSources;

        // 2. Constructor: Inject Context and Initialize Lazies
        public ErpUnitOfWork(ErpDBContext context)
        {
            _context = context;

            // Note: We use () => new ... (Lambda expression)
            // This ensures the object is ONLY created when someone asks for .Value

            _products = new Lazy<IBaseRepository<Product, int>>(() =>
                new BaseRepository<Product, int>(_context));

            _productVariations = new Lazy<IBaseRepository<ProductVariation, int>>(() =>
                new BaseRepository<ProductVariation, int>(_context));

            _productPackages = new Lazy<IBaseRepository<ProductPackage, int>>(() =>
                new BaseRepository<ProductPackage, int>(_context));

            _packageTypes = new Lazy<IBaseRepository<PackageType, int>>(() =>
                new BaseRepository<PackageType, int>(_context));

            _warehouses = new Lazy<IBaseRepository<Warehouse, int>>(() =>
                new BaseRepository<Warehouse, int>(_context));

            _warehouseStocks = new Lazy<IBaseRepository<WarehouseStock, int>>(() =>
                new BaseRepository<WarehouseStock, int>(_context));

            _stockTransferLogs = new Lazy<IBaseRepository<StockTransferLog, int>>(() =>
                new BaseRepository<StockTransferLog, int>(_context));

            _inventoryAdjustments = new Lazy<IBaseRepository<InventoryAdjustment, int>>(() =>
                new BaseRepository<InventoryAdjustment, int>(_context));
        }

        // 3. Public Properties return the .Value
        //User Management
        public IBaseRepository<User, Guid> Users =>
            _users ??= new BaseRepository<User, Guid>(_context);

        //Suppliers & Customers
        public IBaseRepository<Supplier, Guid> Suppliers =>
            _suppliers ??= new BaseRepository<Supplier, Guid>(_context);
        public IBaseRepository<Customer, Guid> Customers =>
            _customers ??= new BaseRepository<Customer, Guid>(_context);



        public IBaseRepository<Product, int> Products => _products.Value;
        public IBaseRepository<ProductVariation, int> ProductVariations => _productVariations.Value;
        public IBaseRepository<ProductPackage, int> ProductPackages => _productPackages.Value;
        public IBaseRepository<PackageType, int> PackageTypes => _packageTypes.Value;
        public IBaseRepository<Warehouse, int> Warehouses => _warehouses.Value;
        public IBaseRepository<WarehouseStock, int> WarehouseStocks => _warehouseStocks.Value;
        public IBaseRepository<StockTransferLog, int> StockTransferLogs => _stockTransferLogs.Value;
        public IBaseRepository<InventoryAdjustment, int> InventoryAdjustments => _inventoryAdjustments.Value;



        //Purchasing
        public IBaseRepository<PurchaseInvoice, int> PurchaseInvoices =>
            _purchaseInvoices ??= new BaseRepository<PurchaseInvoice, int>(_context);
        public IBaseRepository<PurchaseInvoiceItem, int> PurchaseInvoiceItems =>
            _purchaseInvoiceItems ??= new BaseRepository<PurchaseInvoiceItem, int>(_context);
        public IBaseRepository<PurchaseReturn, int> PurchaseReturns =>
            _purchaseReturns ??= new BaseRepository<PurchaseReturn, int>(_context);
        public IBaseRepository<PurchaseReturnItem, int> PurchaseReturnItems =>
            _purchaseReturnItems ??= new BaseRepository<PurchaseReturnItem, int>(_context);

        //Sales
        public IBaseRepository<SalesInvoice, int> SalesInvoices =>
            _salesInvoices ??= new BaseRepository<SalesInvoice, int>(_context);
        public IBaseRepository<SalesInvoiceItem, int> SalesInvoiceItems =>
            _salesInvoiceItems ??= new BaseRepository<SalesInvoiceItem, int>(_context);
        public IBaseRepository<SalesReturn, int> SalesReturns =>
            _salesReturns ??= new BaseRepository<SalesReturn, int>(_context);
        public IBaseRepository<SalesReturnItem, int> SalesReturnItems =>
            _salesReturnItems ??= new BaseRepository<SalesReturnItem, int>(_context);

        //Finance
        public IBaseRepository<Safe, int> Safes =>
            _safes ??= new BaseRepository<Safe, int>(_context);
        public IBaseRepository<PaymentPermission, int> PaymentPermissions =>
            _paymentPermissions ??= new BaseRepository<PaymentPermission, int>(_context);
        public IBaseRepository<ReceiptPermission, int> ReceiptPermissions =>
            _receiptPermissions ??= new BaseRepository<ReceiptPermission, int>(_context);
        public IBaseRepository<ExpenseType, int> ExpenseTypes =>
            _expenseTypes ??= new BaseRepository<ExpenseType, int>(_context);
        public IBaseRepository<RevenueSource, int> RevenueSources =>
            _revenueSources ??= new BaseRepository<RevenueSource, int>(_context);
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}