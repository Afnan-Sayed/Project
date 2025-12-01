using ERP_API.DataAccess.Entities.Finance;
using ERP_API.DataAccess.Entities.Purchasing;
using ERP_API.DataAccess.Entities.Sales;
using ERP_API.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ERP_API.DataAccess.Entities.User
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public UserRole Role { get; set; }
        public List<Permission> Permissions { get; set; } = new();
        public bool IsActive { get; set; } = true;

        public ICollection<PaymentPermission> PaymentPermissions { get; set; } = new List<PaymentPermission>();
        public ICollection<ReceiptPermission> ReceiptPermissions { get; set; } = new List<ReceiptPermission>();
        public ICollection<SalesInvoice> SalesInvoices { get; set; } = new List<SalesInvoice>();
        public ICollection<PurchaseInvoice> PurchaseInvoices { get; set; } = new List<PurchaseInvoice>();
    }
}
