using ERP_MVC.Models.DTOs.Purchasing;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace ERP_MVC.Services.Purchasing
{
    public class PurchaseReturnService
    {
        private readonly HttpClient _api;

        public PurchaseReturnService(IHttpClientFactory http)
        {
            _api = http.CreateClient("AuthorizedApiClient");
        }

        public async Task<List<PurchaseReturnListItemDto>> GetAllReturnsAsync()
        {
            return await _api.GetFromJsonAsync<List<PurchaseReturnListItemDto>>("api/PurchaseReturns")
                ?? new List<PurchaseReturnListItemDto>();
        }

        public async Task<PurchaseReturnResponseDto?> GetReturnByIdAsync(int id)
        {
            return await _api.GetFromJsonAsync<PurchaseReturnResponseDto>($"api/PurchaseReturns/{id}");
        }

        public async Task<(bool Success, string Message)> CreateReturnAsync(CreatePurchaseReturnDto dto)
        {
            var res = await _api.PostAsJsonAsync("api/PurchaseReturns", dto);
            if (res.IsSuccessStatusCode) return (true, "Return created successfully");
            return (false, await res.Content.ReadAsStringAsync());
        }

        public async Task<bool> DeleteReturnAsync(int id)
        {
            var res = await _api.DeleteAsync($"api/PurchaseReturns/{id}");
            return res.IsSuccessStatusCode;
        }
    }
}