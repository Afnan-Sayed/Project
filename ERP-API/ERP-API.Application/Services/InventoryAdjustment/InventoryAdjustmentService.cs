using ERP_Application.Contracts;
using ERP_Application.DTOs.InventoryAdjustment;
using ERP_DataLayer.Contracts;
using ERP_DataLayer.Entities.InventoryAdjustment;
using ERP_DataLayer.Entities.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Application.Services
{
    public class InventoryAdjustmentService : IInventoryAdjustmentService
    {
        private readonly IErpUnitOfWork _unitOfWork;

        public InventoryAdjustmentService(IErpUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public InventoryAdjustment CreateAdjustment(CreateAdjustmentDto dto)
        {
            // 1. Find the current stock record
            var stock = _unitOfWork.WarehouseStocks.GetAll()
                .FirstOrDefault(ws => ws.WarehouseId == dto.WarehouseId
                                   && ws.ProductPackageId == dto.ProductPackageId);

            // Handle case where stock doesn't exist yet (e.g. found new item in empty warehouse)
            if (stock == null)
            {
                stock = new WarehouseStock
                {
                    WarehouseId = dto.WarehouseId,
                    ProductPackageId = dto.ProductPackageId,
                    Quantity = 0, // Start at 0
                    MinStockLevel = 0
                };
                _unitOfWork.WarehouseStocks.Create(stock);
            }

            // 2. Calculate the Math
            decimal oldQty = stock.Quantity;
            decimal newQty = dto.NewQuantity;
            decimal diff = newQty - oldQty;

            // If no change, return null or throw error (optional)
            if (diff == 0) return null;

            // 3. Update the Stock
            stock.Quantity = newQty;
            _unitOfWork.WarehouseStocks.Update(stock);

            // 4. Create the Log Record
            var adjustment = new InventoryAdjustment
            {
                AdjustmentDate = DateTime.UtcNow,
                WarehouseId = dto.WarehouseId,
                ProductPackageId = dto.ProductPackageId,
                OldQuantity = oldQty,
                NewQuantity = newQty,
                Difference = diff,
                AdjustmentType = diff > 0 ? "Increase" : "Decrease", // Auto-detect type
                Reason = dto.Reason,
                UserId = null // For now
            };

            _unitOfWork.InventoryAdjustments.Create(adjustment);
            _unitOfWork.SaveChanges();

            return adjustment;
        }

        public IEnumerable<AdjustmentLogDto> GetAdjustmentLogs()
        {
            var adjs = _unitOfWork.InventoryAdjustments.GetAllQueryable();
            var warehouses = _unitOfWork.Warehouses.GetAllQueryable();
            var packages = _unitOfWork.ProductPackages.GetAllQueryable();
            var variations = _unitOfWork.ProductVariations.GetAllQueryable();
            var products = _unitOfWork.Products.GetAllQueryable();
            var packageTypes = _unitOfWork.PackageTypes.GetAllQueryable();

            var query = from a in adjs
                        join w in warehouses on a.WarehouseId equals w.Id
                        join pkg in packages on a.ProductPackageId equals pkg.Id
                        join pt in packageTypes on pkg.PackageTypeId equals pt.Id
                        join var in variations on pkg.ProductVariationId equals var.Id
                        join prod in products on var.ProductId equals prod.Id
                        orderby a.AdjustmentDate descending
                        select new AdjustmentLogDto
                        {
                            Id = a.Id,
                            Date = a.AdjustmentDate,
                            WarehouseName = w.Name,
                            ProductName = $"{prod.Name} - {var.Name} ({pt.Name})",
                            Type = a.AdjustmentType,
                            Reason = a.Reason,
                            Difference = a.Difference
                        };

            return query.ToList();
        }
    }
}
