namespace ERP_MVC.Models.DTOs.Common
{
    public class SupplierResponse
    {
        public bool Success { get; set; }
        public List<SupplierSimpleDto> Data { get; set; }
    }
}
