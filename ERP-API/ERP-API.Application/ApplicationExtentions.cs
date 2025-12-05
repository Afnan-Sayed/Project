using ERP_API.Application.Interfaces;
using ERP_API.Application.Interfaces.Customers;
using ERP_API.Application.Interfaces.Finance;
using ERP_API.Application.Interfaces.Inventory;
using ERP_API.Application.Interfaces.Purchasing;
using ERP_API.Application.Interfaces.Sales;
using ERP_API.Application.Interfaces.Suppliers;
using ERP_API.Application.Interfaces.User;
using ERP_API.Application.Services;
using ERP_API.Application.Services.Purchasing;
using ERP_API.Application.Services.Sales;
using ERP_API.Application.Services.User;
using Microsoft.Extensions.DependencyInjection;
using ERP_API.Application.Services.Customers;
using ERP_API.Application.Services.Finance;
using ERP_API.Application.Services.Suppliers;
using ERP_API.DataAccess.Entities.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_API.Application
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register your Services here
            services.AddScoped<IProductService, ProductService>();



            services.AddScoped<IPackageTypeService, PackageTypeService>(); 
            services.AddScoped<IWarehouseService, WarehouseService>();
            services.AddScoped<IInventoryAdjustmentService, InventoryAdjustmentService>();
            services.AddScoped<IAccountService, AccountService>();


            services.AddScoped<IMainSafeService, MainSafeService>();
            services.AddScoped<IPaymentOrderService, PaymentOrderService>();
            services.AddScoped<IReceiptOrderService, ReceiptOrderService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ISupplierService, SupplierService>();

            services.AddScoped<IUserManagementService, UserManagementService>();


            services.AddScoped<IPurchaseInvoiceService, PurchaseInvoiceService>();
            services.AddScoped<IPurchaseReturnService, PurchaseReturnService>();
            services.AddScoped<ISalesInvoiceService, SalesInvoiceService>();
            services.AddScoped<ISalesReturnService, SalesReturnService>();

            return services;
        }
    }
}
