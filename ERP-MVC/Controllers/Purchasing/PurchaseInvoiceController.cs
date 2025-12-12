using ERP_MVC.Models.DTOs.Common;
using ERP_MVC.Models.DTOs.Purchasing;
using ERP_MVC.Services.Purchasing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ERP_MVC.Controllers.Purchasing
{
    [Authorize]
    public class PurchaseInvoiceController : Controller
    {
        private readonly PurchaseInvoiceService _service;
        private readonly HttpClient _api;

        public PurchaseInvoiceController(PurchaseInvoiceService service, IHttpClientFactory http)
        {
            _service = service;
            _api = http.CreateClient("AuthorizedApiClient");
        }

        public async Task<IActionResult> Index()
        {
            var list = await _service.GetAllInvoicesAsync();
            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            await LoadSuppliers();
            return View(new CreatePurchaseInvoiceDto
            {
                Items = new List<PurchaseInvoiceItemDto> { new PurchaseInvoiceItemDto() }
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePurchaseInvoiceDto model)
        {
            model.Items = model.Items.Where(x => x.ProductPackageId > 0).ToList();

            if (!ModelState.IsValid || model.Items.Count == 0)
            {
                await LoadSuppliers();
                return View();
            }

            var result = await _service.CreateInvoiceAsync(model);

            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = result.Message;
            await LoadSuppliers();
            return View();
        }

        private async Task LoadSuppliers()
        {
            var response = await _api.GetFromJsonAsync<SupplierResponse>("api/Supplier");
            var suppliers = response?.Data ?? new List<SupplierSimpleDto>();

            ViewBag.SupplierList = suppliers.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.SupplierName
            }).ToList();
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
    }
}