using ERP_Application.Contracts;
using ERP_Application.DTOs.Inventory.Product;
using ERP_Application.DTOs.Inventory.Product.Responses;
using ERP_DataLayer.Contracts;
using ERP_DataLayer.Entities.Inventory;
using ERP_DataLayer.Entities.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ERP_Application.Services
{
    internal class ProductService : IProductService
    {
        private readonly IErpUnitOfWork _unitOfWork;

        public ProductService(IErpUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ==========================================================
        // 1. ADD PRODUCT (Returns DTO)
        // ==========================================================
        public ProductResponseDto AddProduct(ProductInsertDto dto)
        {
            // 1. Create Product Entity
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                CategoryId = dto.CategoryId
            };

            productRepo.Create(product);
            _unitOfWork.SaveChanges();

            // 2. Generate Smart SKU
            string smartSku = GenerateSmartSKU(product.Id);

            // 3. Create Variation Entity
            var variation = new ProductVariation
            {
                ProductId = product.Id,
                Name = dto.VariationName,
                Flavor = dto.Flavor,
                Size = dto.Size,
                SKU = smartSku
            };

            variationRepo.Create(variation);
            _unitOfWork.SaveChanges();

            // 4. Create Package Entity
            var package = new ProductPackage
            {
                ProductVariationId = variation.Id,
                PackageTypeId = dto.PackageTypeId,
                QinP = dto.QinP,
                PurchasePrice = dto.PurchasePrice,
                SalesPrice = dto.SalesPrice,
                Barcode = GenerateBarcode(variation.Id)
            };

            packageRepo.Create(package);
            _unitOfWork.SaveChanges();

            // 5. Add Initial Stock
            AddInitialStockToMain(package.Id, dto.InitialQuantity);

            // 6. Fetch Names for Response
            var packageTypeEntity = _unitOfWork.PackageTypes.FindById(dto.PackageTypeId);
            string pkgName = packageTypeEntity != null ? packageTypeEntity.Name : "Unknown";

            // Return DTO
            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CategoryName = "General", // Placeholder or fetch real name
                Variations = new List<VariationResponseDto>
                {
                    new VariationResponseDto
                    {
                        Id = variation.Id,
                        Name = variation.Name,
                        Flavor = variation.Flavor,
                        SKU = variation.SKU,
                        Packages = new List<PackageResponseDto>
                        {
                            new PackageResponseDto
                            {
                                Id = package.Id,
                                PackageTypeName = pkgName,
                                QinP = package.QinP,
                                SalesPrice = package.SalesPrice,
                                Barcode = package.Barcode
                            }
                        }
                    }
                }
            };
        }

        public ProductResponseDto? GetProductById(int id)
        {
            // We start from the Products table
            var query = _unitOfWork.Products.GetAllQueryable()
                .Where(p => p.Id == id)
                .Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    // Check for null category safely
                    CategoryName = p.Category != null ? p.Category.Name : "Uncategorized",

                    // Map Variations
                    Variations = p.Variations.Select(v => new VariationResponseDto
                    {
                        Id = v.Id,
                        Name = v.Name,
                        Flavor = v.Flavor,
                        SKU = v.SKU,

                        // Map Packages inside Variation
                        Packages = v.ProductPackages.Select(pkg => new PackageResponseDto
                        {
                            Id = pkg.Id,
                            PackageTypeName = pkg.PackageType.Name, // Navigation Property
                            QinP = pkg.QinP,
                            SalesPrice = pkg.SalesPrice,
                            Barcode = pkg.Barcode
                        }).ToList()
                    }).ToList()
                });

            // Execute the query
            return query.FirstOrDefault();
        }

        // ==========================================================
        // 2. ADD VARIATION (Returns DTO)
        // ==========================================================
        public VariationResponseDto AddVariation(int productId, VariationInsertDto dto)
        {
            string smartSku = GenerateSmartSKU(productId);

            var variation = new ProductVariation
            {
                ProductId = productId,
                Name = dto.VariationName,
                Flavor = dto.Flavor,
                Size = dto.Size,
                SKU = smartSku
            };

            variationRepo.Create(variation);
            _unitOfWork.SaveChanges();

            var package = new ProductPackage
            {
                ProductVariationId = variation.Id,
                PackageTypeId = dto.PackageTypeId,
                QinP = dto.QinP,
                PurchasePrice = dto.PurchasePrice,
                SalesPrice = dto.SalesPrice,
                Barcode = GenerateBarcode(variation.Id)
            };

            packageRepo.Create(package);
            _unitOfWork.SaveChanges();

            AddInitialStockToMain(package.Id, dto.InitialQuantity);

            // Fetch Package Name for DTO
            var packageTypeEntity = _unitOfWork.PackageTypes.FindById(dto.PackageTypeId);
            string pkgName = packageTypeEntity != null ? packageTypeEntity.Name : "Unknown";

            return new VariationResponseDto
            {
                Id = variation.Id,
                Name = variation.Name,
                Flavor = variation.Flavor,
                SKU = variation.SKU,
                Packages = new List<PackageResponseDto>
                {
                    new PackageResponseDto
                    {
                        Id = package.Id,
                        PackageTypeName = pkgName,
                        QinP = package.QinP,
                        SalesPrice = package.SalesPrice,
                        Barcode = package.Barcode
                    }
                }
            };
        }

        // ==========================================================
        // 3. ADD PACKAGE (Returns DTO)
        // ==========================================================
        public PackageResponseDto AddPackage(int variationId, PackageLinkInsertDto dto)
        {
            var package = new ProductPackage
            {
                ProductVariationId = variationId,
                PackageTypeId = dto.PackageTypeId,
                QinP = dto.QinP,
                PurchasePrice = dto.PurchasePrice,
                SalesPrice = dto.SalesPrice,
                Barcode = GenerateBarcode(variationId)
            };

            packageRepo.Create(package);
            _unitOfWork.SaveChanges();

            AddInitialStockToMain(package.Id, dto.InitialQuantity);

            // Fetch Package Name for DTO
            var packageTypeEntity = _unitOfWork.PackageTypes.FindById(dto.PackageTypeId);
            string pkgName = packageTypeEntity != null ? packageTypeEntity.Name : "Unknown";

            return new PackageResponseDto
            {
                Id = package.Id,
                PackageTypeName = pkgName,
                QinP = package.QinP,
                SalesPrice = package.SalesPrice,
                Barcode = package.Barcode
            };
        }

        // ==========================================================
        // 4. GET ALL PRODUCTS
        // ==========================================================
        public IEnumerable<ProductSummaryDto> GetAllProducts()
        {
            return _unitOfWork.Products.GetAllQueryable()
                .Select(p => new ProductSummaryDto
                {
                    Id = p.Id,
                    Name = p.Name,

                    // ✅ Efficient SQL Count
                    VariationCount = p.Variations.Count()
                })
                .ToList();
        }

        // ==========================================================
        // 🛠️ HELPERS
        // ==========================================================

        private void AddInitialStockToMain(int packageId, decimal quantity)
        {
            if (quantity > 0)
            {
                // 1. Find Main Warehouse
                var mainWarehouse = warehouseRepo.GetAll().FirstOrDefault(w => w.IsMainWarehouse);

                // 2. Safety: Auto-create if missing (Defensive Coding)
                if (mainWarehouse == null)
                {
                    mainWarehouse = new Warehouse
                    {
                        Name = "Main Virtual Warehouse",
                        Location = "System (Virtual)",
                        IsMainWarehouse = true
                    };
                    warehouseRepo.Create(mainWarehouse);
                    _unitOfWork.SaveChanges();
                }

                // 3. Add Stock
                var stock = new WarehouseStock
                {
                    WarehouseId = mainWarehouse.Id,
                    ProductPackageId = packageId,
                    Quantity = quantity,
                    MinStockLevel = 0
                };
                stockRepo.Create(stock);
                _unitOfWork.SaveChanges();
            }
        }

        private string GenerateSmartSKU(int productId)
        {
            var count = variationRepo.GetAll().Count(v => v.ProductId == productId);
            var nextNumber = count + 1;
            return $"PROD{productId}-VAR{nextNumber.ToString("D3")}";
        }

        private string GenerateBarcode(int variationId)
        {
            return $"800{variationId.ToString("D5")}";
        }

        // --- Repository Accessors ---
        private IBaseRepository<Product, int> productRepo => _unitOfWork.Products;
        private IBaseRepository<ProductVariation, int> variationRepo => _unitOfWork.ProductVariations;
        private IBaseRepository<ProductPackage, int> packageRepo => _unitOfWork.ProductPackages;
        private IBaseRepository<Warehouse, int> warehouseRepo => _unitOfWork.Warehouses;
        private IBaseRepository<WarehouseStock, int> stockRepo => _unitOfWork.WarehouseStocks;
    }
}