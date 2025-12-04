using ERP_API.Application.Interfaces.Customers;
using ERP_API.Application.Interfaces.Suppliers;
using ERP_API.Application.Services.Customers;
using ERP_API.Application.Services.Suppliers;
using ERP_API.DataAccess.DataContext;
using ERP_API.DataAccess.Interfaces;
using ERP_API.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();

builder.Services.AddSwaggerGen();
//Database Configuration
builder.Services.AddDbContext<ErpDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IErpUnitOfWork, ErpUnitOfWork>();

var app = builder.Build();
//app.UseAuthentication();
//app.UseAuthorization();
app.UseHttpsRedirection();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();

app.Run();