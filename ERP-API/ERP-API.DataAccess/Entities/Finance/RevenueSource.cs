using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_API.DataAccess.Entities.Finance
{
    public class RevenueSource
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }

        public ICollection<ReceiptPermission> Receipts { get; set; } = new List<ReceiptPermission>();
    }
}
