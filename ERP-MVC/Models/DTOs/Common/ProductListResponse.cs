namespace ERP_MVC.Models.DTOs.Common
{
    public class ProductListResponse
    {
        public bool Success { get; set; }
        public List<ProductSimpleDto> Data { get; set; }
    }
}
