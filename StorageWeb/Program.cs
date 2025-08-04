var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddScoped<CarDataContext>();

//builder.Services.AddScoped<OrderDataContext>();

//builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));

var app = builder.Build();

app.UseRouting();

app.MapControllers();

app.Run();
