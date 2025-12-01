using ERP_API.DataAccess.Entities.Finance;
using ERP_API.DataAccess.Entities.Sales;
using ERP_API.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_API.DataAccess.Entities.Customers
{
    public class Customer
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Email { get; set; }
        public required string Phone { get; set; } = default!;
        public string? Address { get; set; }
        public string? TaxNum { get; set; }
        public int? InitialBalance { get; set; }
        public CustomerBalanceType? BalanceType { get; set; }

        public ICollection<SalesInvoice> SalesInvoices { get; set; } = new List<SalesInvoice>();
        public ICollection<SalesReturn> SalesReturns { get; set; } = new List<SalesReturn>();
        public ICollection<PaymentPermission> Payments { get; set; } = new List<PaymentPermission>();
        public ICollection<ReceiptPermission> Receipts { get; set; } = new List<ReceiptPermission>();
    }
}
