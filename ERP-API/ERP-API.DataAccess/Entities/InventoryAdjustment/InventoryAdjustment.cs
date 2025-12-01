using ERP_API.DataAccess.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP_API.DataAccess.Entities.User;

namespace ERP_API.DataAccess.Entities.InventoryAdjustment
{
    public class InventoryAdjustment
    {
        public int Id { get; set; }
        public DateTime AdjustmentDate { get; set; }

        // Where did it happen?
        public int WarehouseId { get; set; }
        public Warehouse.Warehouse Warehouse { get; set; }

        // What item?
        public int ProductPackageId { get; set; }
        public ProductPackage ProductPackage { get; set; }

        // The Math (Crucial for Audit)
        public decimal OldQuantity { get; set; } // What was it before? (e.g. 10)
        public decimal NewQuantity { get; set; } // What is it now? (e.g. 8)
        public decimal Difference { get; set; }  // The effect (e.g. -2)

        // Metadata
        public string AdjustmentType { get; set; } // "Increase" or "Decrease" (Calculated)
        public string Reason { get; set; } // e.g. "Stolen", "Expired", "Found", "Gift"

        // User (Nullable for now)
        public Guid? UserId { get; set; }
        public ERP_API.DataAccess.Entities.User.User User { get; set; } = default!;

    }
}