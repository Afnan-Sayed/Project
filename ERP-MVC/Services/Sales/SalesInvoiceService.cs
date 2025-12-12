using ERP_MVC.Models.DTOs.Sales;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace ERP_MVC.Services.Sales
{
    public class SalesInvoiceService
    {
        private readonly HttpClient _api;

        public SalesInvoiceService(IHttpClientFactory http)
        {
            _api = http.CreateClient("AuthorizedApiClient");
        }

        

        public async Task<List<SalesInvoiceListItemDto>> GetAllInvoicesAsync()
        {
            return await _api.GetFromJsonAsync<List<SalesInvoiceListItemDto>>("api/SalesInvoices")
                ?? new List<SalesInvoiceListItemDto>();
        }

        public async Task<SalesInvoiceResponseDto?> GetInvoiceByIdAsync(int id)
        {
            return await _api.GetFromJsonAsync<SalesInvoiceResponseDto>($"api/SalesInvoices/{id}");
        }

        public async Task<(bool Success, string Message)> CreateInvoiceAsync(CreateSalesInvoiceDto dto)
        {
            var res = await _api.PostAsJsonAsync("api/SalesInvoices", dto);
            if (res.IsSuccessStatusCode) return (true, "Invoice created successfully");
            return (false, await res.Content.ReadAsStringAsync());
        }

        public async Task<bool> DeleteInvoiceAsync(int id)
        {
            var res = await _api.DeleteAsync($"api/SalesInvoices/{id}");
            return res.IsSuccessStatusCode;
        }
    }
}