using ERP_DataLayer.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_DataLayer.Entities.Warehouse
{
    public class WarehouseStock
    {
        public int Id { get; set; }

        // Relationship: Which Warehouse?
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }

        // Relationship: Which specific Item? (Linked to ProductPackage, not Product)
        public int ProductPackageId { get; set; }
        public ProductPackage ProductPackage { get; set; }

        // The Inventory Data
        public decimal Quantity { get; set; }

        // Optional: Alert me if quantity drops below this number
        public decimal MinStockLevel { get; set; }
    }
}
