using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using TechShop.Application.Interfaces;
using TechShop.Infraestructure.Data;
using TechShop.Infraestructure.Services;


var builder = WebApplication.CreateBuilder(args);

// configuración previa del fking dataverse
var endpoint = builder.Configuration["Dataverse:Endpoint"];
if (string.IsNullOrWhiteSpace(endpoint))
{
    throw new InvalidOperationException("No se encontró la configuración 'Dataverse:Endpoint' en appsettings.json");
}
builder.Services.AddHttpClient<IDataverseService, DataverseService>();

// configuracion previa del fking sql server
builder.Services.AddDbContext<TechAcademyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerDataBase")));

// inyección del servicio
//builder.Services
//    .AddScoped<IDatosPersonalesService, DatosPersonalesService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.MapDefaultControllerRoute();
app.Run();