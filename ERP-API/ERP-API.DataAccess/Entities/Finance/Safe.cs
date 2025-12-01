using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_API.DataAccess.Entities.Finance
{
    public class Safe
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public decimal OpeningBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public bool IsMainSafe { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public ICollection<PaymentPermission> Payments { get; set; } = new List<PaymentPermission>();
        public ICollection<ReceiptPermission> Receipts { get; set; } = new List<ReceiptPermission>();
    }
}
