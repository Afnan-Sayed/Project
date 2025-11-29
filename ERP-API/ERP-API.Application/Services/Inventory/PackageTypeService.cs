using ERP_Application.Contracts;
using ERP_Application.DTOs.Inventory.Packages;
using ERP_DataLayer.Contracts;
using ERP_DataLayer.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Application.Services
{
    public class PackageTypeService : IPackageTypeService
    {
        private readonly IErpUnitOfWork _unitOfWork;

        public PackageTypeService(IErpUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<PackageTypeItemDto> GetAllPackageTypes()
        {
            return _unitOfWork.PackageTypes.GetAll()
                .Select(pt => new PackageTypeItemDto
                {
                    Id = pt.Id,
                    Name = pt.Name,
                    UnitOfMeasurement = pt.UnitOfMeasurement
                });
        }

        public PackageType AddPackageType(PackageTypeInsertDto dto)
        {
            var entity = new PackageType
            {
                Name = dto.Name,
                Description = dto.Description,
                UnitOfMeasurement = dto.UnitOfMeasurement
            };

            _unitOfWork.PackageTypes.Create(entity);
            _unitOfWork.SaveChanges();

            return entity;
        }

        public PackageDetailsDto GetPackageDetailsWithProducts(int packageTypeId)
        {
            // 1. Get the Package Info
            var packageType = _unitOfWork.PackageTypes.FindById(packageTypeId);
            if (packageType == null) return null;

            // 2. Get Products using this package
            // We start with the Generic Queryable and filter it
            var productNames = _unitOfWork.ProductPackages.GetAllQueryable() // Start Query
                .Where(pkg => pkg.PackageTypeId == packageTypeId)            // Filter by Package
                .Join(_unitOfWork.ProductVariations.GetAllQueryable(),       // Join Variation
                      pkg => pkg.ProductVariationId,
                      var => var.Id,
                      (pkg, var) => var)
                .Join(_unitOfWork.Products.GetAllQueryable(),                // Join Product
                      var => var.ProductId,
                      prod => prod.Id,
                      (var, prod) => prod.Name + " - " + var.Name)           // Select Name
                .ToList(); // EXECUTE QUERY HERE

            return new PackageDetailsDto
            {
                PackageTypeName = packageType.Name,
                UnitOfMeasurement = packageType.UnitOfMeasurement,
                ProductsUsingThis = productNames
            };
        }
    }
}
