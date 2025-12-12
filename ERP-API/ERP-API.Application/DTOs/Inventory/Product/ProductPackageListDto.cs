using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_API.Application.DTOs.Inventory.Product
{
    public class ProductPackageListDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string PackageTypeName { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalesPrice { get; set; }
        public string Barcode { get; set; }
    }
}
