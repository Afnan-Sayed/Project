using ERP_MVC.Models.DTOs.Common;
using ERP_MVC.Models.DTOs.Purchasing;
using ERP_MVC.Services.Purchasing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ERP_MVC.Controllers.Purchasing
{
    [Authorize]
    public class PurchaseReturnController : Controller
    {
        private readonly PurchaseReturnService _service;
        private readonly HttpClient _api;

        public PurchaseReturnController(PurchaseReturnService service, IHttpClientFactory http)
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
            await LoadSuppliers();
            return View(new CreatePurchaseReturnDto
            {
                Items = new List<PurchaseReturnItemDto> { new PurchaseReturnItemDto() }
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePurchaseReturnDto model)
        {
            model.Items = model.Items.Where(x => x.ProductPackageId > 0).ToList();

            if (!ModelState.IsValid || model.Items.Count == 0)
            {
                await LoadSuppliers();
                return View();
            }

            var result = await _service.CreateReturnAsync(model);

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

        // AJAX for dropdowns
        public async Task<IActionResult> GetProducts()
        {
            var result = await _api.GetFromJsonAsync<List<ProductSimpleDto>>("api/Products");
            return Json(result);
        }

        public async Task<IActionResult> GetVariations(int productId)
        {
            var result = await _api.GetFromJsonAsync<List<VariationSimpleDto>>($"api/Products/{productId}/Variations");
            return Json(result);
        }

        public async Task<IActionResult> GetPackages(int variationId)
        {
            var result = await _api.GetFromJsonAsync<ProductPackageDto>($"api/Products/Variations/{variationId}/Packages");
            return Json(result);
        }

        public async Task<IActionResult> GetProductPackages()
        {
            var data = await _api.GetFromJsonAsync<List<ProductPackageSimpleDto>>("api/Products/ProductPackages");
            return Json(data);
        }
    }
}