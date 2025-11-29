using ERP_Application.Contracts;
using ERP_Application.DTOs.Inventory.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        // Constructor Injection
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // POST: api/Products
        [HttpPost]
        public IActionResult Create(ProductInsertDto productDto)
        {
            // Note: Because we use [ApiController], we don't need to write:
            // if (!ModelState.IsValid) return BadRequest();
            // The controller does it automatically based on the [Required] tags in your DTO.

            var createdProduct = _productService.AddProduct(productDto);

            // Returns status 200 OK with the full product object (including the new ID)
            return Ok(createdProduct);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_productService.GetAllProducts());
        }


        // POST: api/Products/{id}/Variations
        // Example: Add "Cheese Flavor" to Product ID 1
        [HttpPost("{productId}/Variations")]
        public IActionResult AddVariation(int productId, VariationInsertDto dto)
        {
            var result = _productService.AddVariation(productId, dto);
            return Ok(result);
        }

        // POST: api/Products/Variations/{id}/Packages
        // Example: Add "Carton" option to Variation ID 5
        [HttpPost("Variations/{variationId}/Packages")]
        public IActionResult AddPackage(int variationId, PackageLinkInsertDto dto)
        {
            var result = _productService.AddPackage(variationId, dto);
            return Ok(result);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }

            return Ok(product);
        }
    }
}
