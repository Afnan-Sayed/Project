using ERP_API.Application.DTOs.Customers;
using ERP_API.Application.Interfaces.Customers;
using ERP_API.DataAccess.Entities.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP_API.API.Controllers.Customers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "SystemManager")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;

        public CustomerController(ICustomerService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var customer = _service.GetById(id);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateCustomer customer)
        {
            var created = _service.Create(customer);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] UpdateCustomerDto updated)
        {
            var result = _service.Update(id, updated);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var ok = _service.Delete(id);
            if (!ok) return NotFound();
            return Ok();
        }
    }
}
