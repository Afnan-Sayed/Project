using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_API.DataAccess.Entities.Finance
{
    public enum ReceiptReferenceType
    {
        WithParty,  //مع طرف (supp or cust)
        Expense,       //مصروف
        RevenueSource //مصدر ربح
    }
}
