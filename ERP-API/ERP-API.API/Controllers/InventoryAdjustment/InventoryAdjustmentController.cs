using ERP_Application.Contracts;
using ERP_Application.DTOs.InventoryAdjustment;
using ERP_Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryAdjustmentController : ControllerBase
    {
        private readonly IInventoryAdjustmentService _inventoryAdjustmentService;

        public InventoryAdjustmentController(IInventoryAdjustmentService inventoryAdjustmentService)
        {
            _inventoryAdjustmentService = inventoryAdjustmentService;
        }

        // POST: api/Warehouses/Adjustments
        [HttpPost("Adjustments")]
        public IActionResult CreateAdjustment(CreateAdjustmentDto dto)
        {
            var result = _inventoryAdjustmentService.CreateAdjustment(dto);
            return Ok(new { Message = "Adjustment recorded successfully.", AdjustmentId = result.Id });
        }

        // GET: api/Warehouses/Adjustments
        [HttpGet("Adjustments")]
        public IActionResult GetAdjustmentLogs()
        {
            var logs = _inventoryAdjustmentService.GetAdjustmentLogs();
            return Ok(logs);
        }
    }
}
