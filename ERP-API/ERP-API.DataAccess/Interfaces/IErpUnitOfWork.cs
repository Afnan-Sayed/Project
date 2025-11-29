using ERP_DataLayer.Entities.Inventory;
using ERP_DataLayer.Entities.InventoryAdjustment;
using ERP_DataLayer.Entities.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_DataLayer.Contracts
{
    public interface IErpUnitOfWork
    {
        IBaseRepository<Product, int> Products { get; }
        IBaseRepository<ProductVariation, int> ProductVariations { get; }
        IBaseRepository<ProductPackage, int> ProductPackages { get; }

        IBaseRepository<PackageType, int> PackageTypes { get; }

        IBaseRepository<Warehouse, int> Warehouses { get; }
        IBaseRepository<WarehouseStock, int> WarehouseStocks { get; }

        IBaseRepository<StockTransferLog, int> StockTransferLogs { get; }

        IBaseRepository<InventoryAdjustment, int> InventoryAdjustments { get; }

        void SaveChanges();
    }
}
