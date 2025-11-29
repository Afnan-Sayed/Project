using ERP_Application.DTOs.Inventory.Product;
using ERP_Application.DTOs.Inventory.Product.Responses;
using ERP_DataLayer.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Application.Contracts
{
    public interface IProductService
    {
        ProductResponseDto AddProduct(ProductInsertDto dto);
        IEnumerable<ProductSummaryDto> GetAllProducts();

        ProductResponseDto? GetProductById(int id);

        // Updated Return Types:
        VariationResponseDto AddVariation(int productId, VariationInsertDto dto);
        PackageResponseDto AddPackage(int variationId, PackageLinkInsertDto dto);
    }
}
