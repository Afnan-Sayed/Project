using ERP_Application.DTOs.Inventory.Packages;
using ERP_DataLayer.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Application.Contracts
{
    public interface IPackageTypeService
    {
        IEnumerable<PackageTypeItemDto> GetAllPackageTypes();
        PackageType AddPackageType(PackageTypeInsertDto dto);

        // Add this line inside the interface
        PackageDetailsDto? GetPackageDetailsWithProducts(int id);
    }
}
