using ERP_Application.Contracts;
using ERP_Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Application
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register your Services here
            services.AddScoped<IProductService, ProductService>();

            // Add Category/Package services here later if you create them
            // services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<IPackageTypeService, PackageTypeService>();

            services.AddScoped<IWarehouseService, WarehouseService>();

            services.AddScoped<IInventoryAdjustmentService, InventoryAdjustmentService>();

            return services;
        }
    }
}
