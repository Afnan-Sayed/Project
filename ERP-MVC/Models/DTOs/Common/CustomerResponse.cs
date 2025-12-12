namespace ERP_MVC.Models.DTOs.Common
{
    public class CustomerResponse
    {
        public bool Success { get; set; }
        public List<CustomerSimpleDto> Data { get; set; }
    }
}
