namespace ERP_MVC.Models.DTOs.Common
{
    public class ProductPackageSimpleDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string PackageTypeName { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalesPrice { get; set; }
        public string Barcode { get; set; }
    }
}
