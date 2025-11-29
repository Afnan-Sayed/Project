using ERP_Application.Contracts;
using ERP_Application.DTOs.Warehouse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;

        public WarehousesController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        // GET: api/Warehouses
        // This will list all warehouses (including the Main Virtual one)
        [HttpGet]
        public IActionResult GetAll()
        {
            var warehouses = _warehouseService.GetAllWarehouses();
            return Ok(warehouses);
        }

        // POST: api/Warehouses
        // Create a new physical location (e.g. "Alex Branch")
        [HttpPost]
        public IActionResult Create(WarehouseInsertDto dto)
        {
            var createdWarehouse = _warehouseService.AddWarehouse(dto);
            return Ok(createdWarehouse);
        }

        // POST: api/Warehouses/Transfer
        [HttpPost("Transfer")]
        public IActionResult TransferStock(StockTransferDto dto)
        {
            // 1. Validate inputs (Warehouses shouldn't be the same)
            if (dto.FromWarehouseId == dto.ToWarehouseId)
            {
                return BadRequest("Source and Destination warehouses cannot be the same.");
            }

            // 2. Call Service
            bool success = _warehouseService.TransferStock(dto);

            if (!success)
            {
                return BadRequest("Transfer Failed: Insufficient stock or invalid item.");
            }

            return Ok("Transfer Successful");
        }


        

        // GET: api/Warehouses/1/Stock
        [HttpGet("{id}/Stock")]
        public IActionResult GetStock(int id)
        {
            var result = _warehouseService.GetWarehouseStock(id);
            // We return Ok even if empty list (it just means empty warehouse)
            return Ok(result);
        }


        // GET: api/Warehouses/Logs
        [HttpGet("Logs")]
        public IActionResult GetLogs()
        {
            var logs = _warehouseService.GetTransferLogs();
            return Ok(logs);
        }

    }
}
