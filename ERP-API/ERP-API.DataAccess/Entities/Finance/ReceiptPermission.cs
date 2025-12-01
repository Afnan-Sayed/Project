using ERP_API.DataAccess.Entities.Customers;
using ERP_API.DataAccess.Entities.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP_API.DataAccess.Entities.User;

namespace ERP_API.DataAccess.Entities.Finance
{
    public class ReceiptPermission
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string? Description { get; set; }
        public ReceiptReferenceType ReferenceType { get; set; }

        // Safe Relationship
        public int SafeId { get; set; }
        public Safe Safe { get; set; } = default!;

        // User Relationship
        public Guid UserId { get; set; }
        public ERP_API.DataAccess.Entities.User.User User { get; set; } = default!;

        // Party Relationship (Supplier or Customer)
        public Guid? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        public Guid? CustomerId { get; set; }
        public Customer? Customer { get; set; }

        // Expense or Revenue Source
        public int? ExpenseTypeId { get; set; }
        public ExpenseType? ExpenseType { get; set; }

        public int? RevenueSourceId { get; set; }
        public RevenueSource? RevenueSource { get; set; }
    }
}
