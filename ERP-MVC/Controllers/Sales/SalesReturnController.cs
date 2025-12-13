using ERP_MVC.Models.DTOs.Common;
using ERP_MVC.Models.DTOs.Sales;
using ERP_MVC.Services.Sales;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ERP_MVC.Controllers.Sales
{
    [Authorize]
    public class SalesReturnController : Controller
    {
        private readonly SalesReturnService _service;
        private readonly HttpClient _api;

        public SalesReturnController(SalesReturnService service, IHttpClientFactory http)
        {
            _service = service;
            _api = http.CreateClient("AuthorizedApiClient");
        }

        public async Task<IActionResult> Index()
        {
            var list = await _service.GetAllReturnsAsync();
            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            await LoadCustomers();
            return View(new CreateSalesReturnDto
            {
                Items = new List<SalesReturnItemDto> { new SalesReturnItemDto() }
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSalesReturnDto model)
        {
            model.Items = model.Items.Where(x => x.ProductPackageId > 0).ToList();

            if (!ModelState.IsValid || model.Items.Count == 0)
            {
                await LoadCustomers();
                return View(model);
            }

            var result = await _service.CreateReturnAsync(model);

            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = result.Message;
            await LoadCustomers();
            return View(model);
        }

        // AJAX
        public async Task<IActionResult> GetProducts()
        {
            return Json(await _api.GetFromJsonAsync<List<ProductSimpleDto>>("api/Products"));
        }

        public async Task<IActionResult> GetVariations(int productId)
        {
            return Json(await _api.GetFromJsonAsync<List<VariationSimpleDto>>($"api/Products/{productId}/Variations"));
        }

        public async Task<IActionResult> GetPackages(int variationId)
        {
            return Json(await _api.GetFromJsonAsync<ProductPackageDto>($"api/Products/Variations/{variationId}/Packages"));
        }

        public async Task<IActionResult> GetProductPackages()
        {
            var data = await _api.GetFromJsonAsync<List<ProductPackageSimpleDto>>("api/Products/ProductPackages");
            return Json(data);
        }

        private async Task LoadCustomers()
        {
            var response = await _api.GetFromJsonAsync<CustomerResponse>("api/Customer");
            var customers = response?.Data ?? new List<CustomerSimpleDto>();

            ViewBag.CustomerList = customers.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CustomerName
            }).ToList();
        }
    }
}