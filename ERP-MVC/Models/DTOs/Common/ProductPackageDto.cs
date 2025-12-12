namespace ERP_MVC.Models.DTOs.Common
{
    public class ProductPackageDto
    {
        public int Id { get; set; }
        public decimal SalesPrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public string PackageTypeName { get; set; }
    }
}
