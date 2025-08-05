using StorageWeb.Repository;
using StorageWeb.Repository.Receipt;
using StorageWeb.Repository.Resource;
using StorageWeb.Repository.Unit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<ReceiptDataContext>();

builder.Services.AddScoped<ResourceDataContext>();

builder.Services.AddScoped<UnitDataContext>();

builder.Services.Configure<ConnectionString>(builder.Configuration.GetSection("ConnectionString"));

var app = builder.Build();

app.UseRouting();

app.MapControllers();

app.Run();
