using Microsoft.EntityFrameworkCore;
using ERP_API.DataAccess.DataContext;
using ERP_API.DataAccess.Interfaces;
using ERP_API.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//Database Configuration
builder.Services.AddDbContext<ErpDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IErpUnitOfWork, ErpUnitOfWork>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();