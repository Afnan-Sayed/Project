using ERP_Application.Contracts;
using ERP_Application.DTOs.Warehouse;
using ERP_DataLayer.Contracts;
using ERP_DataLayer.Entities.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Application.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IErpUnitOfWork _unitOfWork;

        public WarehouseService(IErpUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            // ✅ AUTOMATIC SETUP: Ensure Main Warehouse exists
            EnsureMainWarehouseExists();
        }

        private void EnsureMainWarehouseExists()
        {
            // Check if a warehouse with IsMainWarehouse = true already exists
            var mainExists = _unitOfWork.Warehouses.GetAll()
                .Any(w => w.IsMainWarehouse);

            if (!mainExists)
            {
                var mainWarehouse = new Warehouse
                {
                    Name = "Main Virtual Warehouse",
                    Location = "System (Virtual)",
                    IsMainWarehouse = true
                };

                _unitOfWork.Warehouses.Create(mainWarehouse);
                _unitOfWork.SaveChanges();
            }
        }

        public Warehouse AddWarehouse(WarehouseInsertDto dto)
        {
            var warehouse = new Warehouse
            {
                Name = dto.Name,
                Location = dto.Location,
                IsMainWarehouse = false // User created warehouses are always physical (not Main)
            };

            _unitOfWork.Warehouses.Create(warehouse);
            _unitOfWork.SaveChanges();

            return warehouse;
        }

        public IEnumerable<WarehouseItemDto> GetAllWarehouses()
        {
            return _unitOfWork.Warehouses.GetAll()
                .Select(w => new WarehouseItemDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    Location = w.Location,
                    IsMainWarehouse = w.IsMainWarehouse
                });
        }

        public bool TransferStock(StockTransferDto dto)
        {
            // 1. Validate: Ensure warehouses exist (Optional but good practice)
            // In Mock data we trust the IDs, in Real DB you might check existence.

            // 2. Find Source Stock (FROM)
            var sourceStock = _unitOfWork.WarehouseStocks.GetAll()
                .FirstOrDefault(ws => ws.WarehouseId == dto.FromWarehouseId
                                   && ws.ProductPackageId == dto.ProductPackageId);

            // CHECK: Do we have enough?
            if (sourceStock == null || sourceStock.Quantity < dto.Quantity)
            {
                // In a real app, you might throw a custom exception like "InsufficientStockException"
                // For now, we return false to indicate failure.
                return false;
            }

            // 3. Find Destination Stock (TO)
            var destStock = _unitOfWork.WarehouseStocks.GetAll()
                .FirstOrDefault(ws => ws.WarehouseId == dto.ToWarehouseId
                                   && ws.ProductPackageId == dto.ProductPackageId);

            // 4. EXECUTE TRANSFER

            // A. Decrease Source
            sourceStock.Quantity -= dto.Quantity;
            _unitOfWork.WarehouseStocks.Update(sourceStock); // Update Logic

            // B. Increase Destination
            if (destStock != null)
            {
                // Case: Stock record already exists -> Just add qty
                destStock.Quantity += dto.Quantity;
                _unitOfWork.WarehouseStocks.Update(destStock);
            }
            else
            {
                // Case: New item for this warehouse -> Create record
                var newStock = new WarehouseStock
                {
                    WarehouseId = dto.ToWarehouseId,
                    ProductPackageId = dto.ProductPackageId,
                    Quantity = dto.Quantity,
                    MinStockLevel = 0 // Default
                };
                _unitOfWork.WarehouseStocks.Create(newStock);
            }

            var transferLog = new StockTransferLog
            {
                TransferDate = DateTime.UtcNow, // Always use UTC for server timestamps
                FromWarehouseId = dto.FromWarehouseId,
                ToWarehouseId = dto.ToWarehouseId,
                ProductPackageId = dto.ProductPackageId,
                Quantity = dto.Quantity
            };

            _unitOfWork.StockTransferLogs.Create(transferLog);

            // 5. Commit Transaction (Saves Updates + The Log at the same time)
            _unitOfWork.SaveChanges();

            // 5. Commit Transaction
            _unitOfWork.SaveChanges();

            return true;
        }

        public IEnumerable<WarehouseStockDto> GetWarehouseStock(int warehouseId)
        {
            var stockItems = _unitOfWork.WarehouseStocks.GetAllQueryable()
                .Where(w => w.WarehouseId == warehouseId);

            var query = from stock in stockItems
                        join pkg in _unitOfWork.ProductPackages.GetAllQueryable()
                            on stock.ProductPackageId equals pkg.Id
                        join pt in _unitOfWork.PackageTypes.GetAllQueryable()
                            on pkg.PackageTypeId equals pt.Id
                        join var in _unitOfWork.ProductVariations.GetAllQueryable()
                            on pkg.ProductVariationId equals var.Id
                        join prod in _unitOfWork.Products.GetAllQueryable()
                            on var.ProductId equals prod.Id
                        select new WarehouseStockDto
                        {
                            StockId = stock.Id,

                            ProductPackageId = pkg.Id,

                            ProductName = prod.Name,
                            VariationName = var.Name,
                            PackageName = pt.Name,
                            Quantity = stock.Quantity
                        };

            return query.ToList();
        }

        public IEnumerable<StockTransferLogDto> GetTransferLogs()
        {
            var logs = _unitOfWork.StockTransferLogs.GetAllQueryable();
            var warehouses = _unitOfWork.Warehouses.GetAllQueryable();
            var packages = _unitOfWork.ProductPackages.GetAllQueryable();
            var variations = _unitOfWork.ProductVariations.GetAllQueryable();
            var products = _unitOfWork.Products.GetAllQueryable();
            var packageTypes = _unitOfWork.PackageTypes.GetAllQueryable();

            var query = from log in logs
                            // 1. Join Source Warehouse
                        join whFrom in warehouses on log.FromWarehouseId equals whFrom.Id
                        // 2. Join Destination Warehouse
                        join whTo in warehouses on log.ToWarehouseId equals whTo.Id
                        // 3. Join Product Hierarchy
                        join pkg in packages on log.ProductPackageId equals pkg.Id
                        join pt in packageTypes on pkg.PackageTypeId equals pt.Id
                        join var in variations on pkg.ProductVariationId equals var.Id
                        join prod in products on var.ProductId equals prod.Id

                        // 4. Order by latest first
                        orderby log.TransferDate descending

                        select new StockTransferLogDto
                        {
                            Id = log.Id,
                            Date = log.TransferDate,
                            FromWarehouse = whFrom.Name,
                            ToWarehouse = whTo.Name,
                            ProductName = prod.Name,
                            VariationName = var.Name,
                            PackageType = pt.Name,
                            Quantity = log.Quantity
                        };

            return query.ToList();
        }

    }
}
