using ERP_Application.Contracts;
using ERP_Application.DTOs.Inventory.Packages;
using Microsoft.AspNetCore.Mvc;

namespace G3_API.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageTypesController : ControllerBase
    {
        private readonly IPackageTypeService _packageTypeService;

        // Constructor Injection: Ask the container for the Service we created earlier
        public PackageTypesController(IPackageTypeService packageTypeService)
        {
            _packageTypeService = packageTypeService;
        }

        // GET: api/PackageTypes
        // Used to fill the "Package Type" Dropdown in your UI
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _packageTypeService.GetAllPackageTypes();
            return Ok(result);
        }

        // POST: api/PackageTypes
        // Used to define a new type (e.g. "Barrel - Liter")
        [HttpPost]
        public IActionResult Create(PackageTypeInsertDto dto)
        {
            var createdPackageType = _packageTypeService.AddPackageType(dto);
            return Ok(createdPackageType);
        }

        // GET: api/PackageTypes/1/Products
        // Returns the package info AND the list of products using it
        [HttpGet("{id}/Products")]
        public IActionResult GetPackageDetails(int id)
        {
            var result = _packageTypeService.GetPackageDetailsWithProducts(id);

            if (result == null)
            {
                return NotFound($"Package Type with ID {id} not found.");
            }

            return Ok(result);
        }
    }
}