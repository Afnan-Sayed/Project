using ERP_API.Application.DTOs.Suppliers;
using ERP_API.Application.Interfaces.Suppliers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP_API.API.Controllers.Suppliers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "SystemManager")]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _service;

        public SuppliersController(ISupplierService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var supplier = _service.GetById(id);
            if (supplier == null) return NotFound();
            return Ok(supplier);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateSupplierDto dto)
        {
            var created = _service.Create(dto);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] UpdateSupplierDto dto)
        {
            var result = _service.Update(id, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var deleted = _service.Delete(id);
            if (!deleted) return NotFound();
            return Ok();
        }
    }
}
