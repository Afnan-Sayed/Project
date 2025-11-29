using ERP_Application.DTOs.InventoryAdjustment;
using ERP_DataLayer.Entities.InventoryAdjustment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Application.Contracts
{
    public interface IInventoryAdjustmentService
    {
        InventoryAdjustment CreateAdjustment(CreateAdjustmentDto dto);
        IEnumerable<AdjustmentLogDto> GetAdjustmentLogs();
    }
}
