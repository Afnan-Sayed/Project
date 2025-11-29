using ERP_Application.DTOs.Warehouse;
using ERP_DataLayer.Entities.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Application.Contracts
{
    public interface IWarehouseService
    {
        Warehouse AddWarehouse(WarehouseInsertDto dto);
        IEnumerable<WarehouseItemDto> GetAllWarehouses();

        // Returns true if successful, false (or throws exception) if failed
        bool TransferStock(StockTransferDto dto);

        IEnumerable<WarehouseStockDto> GetWarehouseStock(int warehouseId);

        // Returns a list of all transfers that happened in history
        IEnumerable<StockTransferLogDto> GetTransferLogs();

    }
}
