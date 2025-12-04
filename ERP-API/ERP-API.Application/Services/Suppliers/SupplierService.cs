using ERP_API.Application.DTOs.Suppliers;
using ERP_API.Application.Interfaces.Suppliers;
using ERP_API.DataAccess.Entities.Suppliers;
using ERP_API.DataAccess.Enums;
using ERP_API.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_API.Application.Services.Suppliers
{
    public class SupplierService : ISupplierService
    {
        private readonly IErpUnitOfWork _uow;

        public SupplierService(IErpUnitOfWork uow)
        {
            _uow = uow;
        }

        public IEnumerable<SupplierDto> GetAll()
        {
            return _uow.Suppliers.GetAll().Select(s => new SupplierDto
            {
                Id = s.Id,
                Name = s.Name,
                Phone = s.Phone,
                Email = s.Email,
                Address = s.Address,
                TaxNumber = s.TaxNumber,
                InitialBalance = s.InitialBalance,
                BalanceType = s.BalanceType
            });
        }

        public SupplierDto? GetById(Guid id)
        {
            var s = _uow.Suppliers.FindById(id);
            if (s == null) return null;

            return new SupplierDto
            {
                Id = s.Id,
                Name = s.Name,
                Phone = s.Phone,
                Email = s.Email,
                Address = s.Address,
                TaxNumber = s.TaxNumber,
                InitialBalance = s.InitialBalance,
                BalanceType = s.BalanceType
            };
        }

        public SupplierDto Create(CreateSupplierDto dto)
        {
            var supplier = new Supplier
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address,
                TaxNumber = dto.TaxNumber,
                InitialBalance = dto.InitialBalance ?? 0,
                BalanceType = dto.BalanceType ?? SupplierBalanceType.SupplierIsDebtor
            };

            _uow.Suppliers.Create(supplier);
            _uow.SaveChanges();

            return new SupplierDto
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Phone = supplier.Phone,
                Email = supplier.Email,
                Address = supplier.Address,
                TaxNumber = supplier.TaxNumber,
                InitialBalance = supplier.InitialBalance,
                BalanceType = supplier.BalanceType
            };
        }

        public SupplierDto? Update(Guid id, UpdateSupplierDto dto)
        {
            var supplier = _uow.Suppliers.FindById(id);
            if (supplier == null) return null;

            supplier.Name = dto.Name;
            supplier.Phone = dto.Phone;
            supplier.Email = dto.Email;
            supplier.Address = dto.Address;
            supplier.TaxNumber = dto.TaxNumber;

            if (dto.InitialBalance != null && dto.BalanceType != null)
            {
                supplier.InitialBalance = dto.InitialBalance.Value;
                supplier.BalanceType = dto.BalanceType.Value;
            }

            _uow.Suppliers.Update(supplier);
            _uow.SaveChanges();

            return new SupplierDto
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Phone = supplier.Phone,
                Email = supplier.Email,
                Address = supplier.Address,
                TaxNumber = supplier.TaxNumber,
                InitialBalance = supplier.InitialBalance,
                BalanceType = supplier.BalanceType
            };
        }

        public bool Delete(Guid id)
        {
            var deleted = _uow.Suppliers.Delete(id);
            if (deleted == null) return false;
            _uow.SaveChanges();
            return true;
        }
    }
}
