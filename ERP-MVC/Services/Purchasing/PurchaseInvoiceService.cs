using ERP_MVC.Models.DTOs.Purchasing;
using System.Net.Http.Json;


namespace ERP_MVC.Services.Purchasing{ 
public class PurchaseInvoiceService
{

        private readonly HttpClient _api;

        public PurchaseInvoiceService(IHttpClientFactory http)
        {
            _api = http.CreateClient("AuthorizedApiClient");
        }

    public async Task<List<PurchaseInvoiceListItemDto>> GetAllInvoicesAsync()
    {
        return await _api.GetFromJsonAsync<List<PurchaseInvoiceListItemDto>>("api/PurchaseInvoices")
            ?? new List<PurchaseInvoiceListItemDto>();
    }

    public async Task<PurchaseInvoiceResponseDto?> GetInvoiceByIdAsync(int id)
    {
        return await _api.GetFromJsonAsync<PurchaseInvoiceResponseDto>($"api/PurchaseInvoices/{id}");
    }

    public async Task<(bool Success, string Message)> CreateInvoiceAsync(CreatePurchaseInvoiceDto dto)
    {
        var res = await _api.PostAsJsonAsync("api/PurchaseInvoices", dto);
        if (res.IsSuccessStatusCode) return (true, "Invoice created successfully");
        return (false, await res.Content.ReadAsStringAsync());
    }

    public async Task<bool> DeleteInvoiceAsync(int id)
    {
        var res = await _api.DeleteAsync($"api/PurchaseInvoices/{id}");
        return res.IsSuccessStatusCode;
    }
}
}