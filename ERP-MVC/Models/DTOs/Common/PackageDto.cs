namespace ERP_MVC.Models.DTOs.Common
{
    public class PackageDto
    {
        public int Id { get; set; }
        public string PackageTypeName { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalesPrice { get; set; }
    }
}
