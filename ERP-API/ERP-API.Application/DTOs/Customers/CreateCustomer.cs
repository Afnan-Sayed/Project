using ERP_API.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_API.Application.DTOs.Customers
{
    public class CreateCustomer
    {
        public required string Name { get; set; }
        public required string Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? TaxNumber { get; set; }
        public int? InitialBalance { get; set; }
        public CustomerBalanceType? BalanceType { get; set; }
    }
}
