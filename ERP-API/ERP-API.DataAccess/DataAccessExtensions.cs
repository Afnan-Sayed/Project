using ERP_API.DataAccess.DataContext;
using ERP_API.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_API.DataAccess
{
    public static class DataAccessExtensions
    {
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Register DbContext with SQL Server
            services.AddDbContext<ErpDBContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            // 2. Register UnitOfWork
            services.AddScoped<IErpUnitOfWork, ErpUnitOfWork>();

            return services;
        }
    }
}
