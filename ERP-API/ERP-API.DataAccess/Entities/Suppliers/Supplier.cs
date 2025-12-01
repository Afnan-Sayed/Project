using ERP_API.DataAccess.Entities.Finance;
using ERP_API.DataAccess.Entities.Purchasing;
using ERP_API.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_API.DataAccess.Entities.Suppliers
{
    public class Supplier
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Email { get; set; }
        public required string Phone { get; set; } = default!;
        public string? Address { get; set; }
        public string? TaxNumber { get; set; }

        public decimal? InitialBalance { get; set; }
        public SupplierBalanceType? BalanceType { get; set; }

        public ICollection<PurchaseInvoice> PurchaseInvoices { get; set; } = new List<PurchaseInvoice>();
        public ICollection<PurchaseReturn> PurchaseReturns { get; set; } = new List<PurchaseReturn>();
        public ICollection<PaymentPermission> Payments { get; set; } = new List<PaymentPermission>();
        public ICollection<ReceiptPermission> Receipts { get; set; } = new List<ReceiptPermission>();

    }
}
