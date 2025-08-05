using StorageWeb.Repository;
using StorageWeb.Repository.Receipt.Models;
using StorageWeb.Repository.Resource.Models;
using StorageWeb.Repository.Unit.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ReceiptDataContext>();

builder.Services.AddScoped<ResourceDataContext>();

builder.Services.AddScoped<UnitDataContext>();

builder.Services.Configure<ConnectionString>(builder.Configuration.GetSection("ConnectionString"));

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
