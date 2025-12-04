using ERP_API.Application.DTOs.Customers;
using ERP_API.Application.Interfaces.Customers;
using ERP_API.DataAccess.Entities.Customers;
using ERP_API.DataAccess.Enums;
using ERP_API.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_API.Application.Services.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly IErpUnitOfWork _uow;

        public CustomerService(IErpUnitOfWork uow)
        {
            _uow = uow;
        }


        public IEnumerable<CustomerDto> GetAll()
        {
            return _uow.Customers.GetAll().Select(c => new CustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                Phone = c.Phone,
                Email = c.Email,
                Address = c.Address,
                TaxNum = c.TaxNum,
                InitialBalance = c.InitialBalance,
                BalanceType = c.BalanceType
            });
        }
        public CustomerDto? GetById(Guid id)
        {
            var c = _uow.Customers.FindById(id);
            if (c == null) return null;

            return new CustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                Phone = c.Phone,
                Email = c.Email,
                Address = c.Address,
                TaxNum = c.TaxNum,
                InitialBalance = c.InitialBalance,
                BalanceType = c.BalanceType
            };
        }
        public CustomerDto Create(CreateCustomer dto)
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address,
                TaxNum = dto.TaxNumber,
                InitialBalance = dto.InitialBalance ?? 0,
                BalanceType = dto.BalanceType ?? CustomerBalanceType.CustomerIsDebtor
            };

            _uow.Customers.Create(customer);
            _uow.SaveChanges();

            return new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                Email = customer.Email,
                Address = customer.Address,
                TaxNum = customer.TaxNum,
                InitialBalance = customer.InitialBalance,
                BalanceType = customer.BalanceType
            };
        }

        public CustomerDto Update(Guid id, UpdateCustomerDto updated)
        {
            var customer = _uow.Customers.FindById(id);
            if (customer == null) return null;

            customer.Name = updated.Name;
            customer.Phone = updated.Phone;
            customer.Email = updated.Email;
            customer.Address = updated.Address;
            customer.TaxNum = updated.TaxNumber;

            _uow.Customers.Update(customer);
            _uow.SaveChanges();

            return new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                Email = customer.Email,
                Address = customer.Address,
                TaxNum = customer.TaxNum,
                InitialBalance = customer.InitialBalance,
                BalanceType = customer.BalanceType
            };
        }

        public bool Delete(Guid id)
        {
            var deleted = _uow.Customers.Delete(id);
            if (deleted == null) return false;
            _uow.SaveChanges();
            return true;
        }

     
    }
}
