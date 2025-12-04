using ERP_API.Application.DTOs.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_API.Application.Interfaces.Suppliers
{
    public interface ISupplierService
    {
        IEnumerable<SupplierDto> GetAll();
        SupplierDto? GetById(Guid id);
        SupplierDto Create(CreateSupplierDto dto);
        SupplierDto? Update(Guid id, UpdateSupplierDto dto);
        bool Delete(Guid id);
    }
}
