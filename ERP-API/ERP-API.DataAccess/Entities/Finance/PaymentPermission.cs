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
    public class PaymentPermission
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string? Description { get; set; }
        public ReferenceType ReferenceType { get; set; }

        // Safe Relationship
        public int SafeId { get; set; }
        public Safe Safe { get; set; } = default!;

        // User Relationship
        public Guid UserId { get; set; }
        public ERP_API.DataAccess.Entities.User.User User { get; set; } = default!; // Fully qualify User type

        // Party Relationship (Supplier or Customer)
        public Guid? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        public Guid? CustomerId { get; set; }
        public Customer? Customer { get; set; }
    }
}
