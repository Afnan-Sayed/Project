using ERP_MVC.Models.DTOs.Sales;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace ERP_MVC.Services.Sales
{
    public class SalesReturnService
    {
        private readonly HttpClient _api;

        public SalesReturnService(IHttpClientFactory http)
        {
            _api = http.CreateClient("AuthorizedApiClient");
        }

        public async Task<List<SalesReturnListItemDto>> GetAllReturnsAsync()
        {
            return await _api.GetFromJsonAsync<List<SalesReturnListItemDto>>("api/SalesReturns")
                ?? new List<SalesReturnListItemDto>();
        }

        public async Task<SalesReturnResponseDto?> GetReturnByIdAsync(int id)
        {
            return await _api.GetFromJsonAsync<SalesReturnResponseDto>($"SalesReturns/{id}");
        }

        public async Task<(bool Success, string Message)> CreateReturnAsync(CreateSalesReturnDto dto)
        {
            var res = await _api.PostAsJsonAsync("api/SalesReturns", dto);
            if (res.IsSuccessStatusCode) return (true, "Return created successfully");
            return (false, await res.Content.ReadAsStringAsync());
        }

        public async Task<bool> DeleteReturnAsync(int id)
        {
            var res = await _api.DeleteAsync($"api/SalesReturns/{id}");
            return res.IsSuccessStatusCode;
        }
    }
}