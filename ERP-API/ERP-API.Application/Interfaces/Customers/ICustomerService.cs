using ERP_API.Application.DTOs.Customers;
using ERP_API.DataAccess.Entities.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_API.Application.Interfaces.Customers
{
    public interface ICustomerService
    {
        IEnumerable<CustomerDto> GetAll();
        CustomerDto? GetById(Guid id);
        CustomerDto Create(CreateCustomer customer);
        CustomerDto? Update(Guid id, UpdateCustomerDto updated);
        bool Delete(Guid id);
    }
}
