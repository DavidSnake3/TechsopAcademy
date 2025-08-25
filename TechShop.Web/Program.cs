using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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


// vaina para autenticar y proteger url
builder.Services
  .AddAuthentication("MyCookieAuth")
  .AddCookie("MyCookieAuth", options =>
  {
      options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
      options.SlidingExpiration = true;

      options.LoginPath = "/Login/Index";
      options.AccessDeniedPath = "/Login/AccessDenied";

      options.Events.OnRedirectToLogin = ctx =>
      {
          ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
          return Task.CompletedTask;
      };
      options.Events.OnRedirectToAccessDenied = ctx =>
      {
          ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
          return Task.CompletedTask;
      };

      options.Events.OnValidatePrincipal = context =>
      {
          var issuedUtc = context.Properties.IssuedUtc;
          if (issuedUtc.HasValue && issuedUtc.Value.AddHours(2) < DateTimeOffset.UtcNow)
          {
              context.RejectPrincipal();
              return context.HttpContext.SignOutAsync("MyCookieAuth");
          }
          return Task.CompletedTask;
      };
  });

builder.Services.AddControllersWithViews();

// al reiniciar se mantiene y gestiona cosas en memoria
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<AvisosService>();

//inyectamos
builder.Services.AddScoped<ICursoService, CursoService>();



var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();

app.UseExceptionHandler("/Error/500");
app.UseStatusCodePagesWithReExecute("/Error/{0}");
app.UseAuthentication();
app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");
app.Run();