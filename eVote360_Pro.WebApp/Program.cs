using FluentValidation;
using eVote360_Pro.Core.Application.Validators.User;
using eVote360_Pro.Core.Application;
using eVote360_Pro.Core.Application.Profiles;
using eVote360_Pro.WebApp.Profiles;
using eVote360_Pro.Persistence;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Registrar AutoMapper en el Composition Root escaneando los ensamblados de la capa de aplicación y presentación
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(UserProfile).Assembly, typeof(WebMappingProfile).Assembly);
});

// Registrar las capas de Aplicación y Persistencia
builder.Services.AddApplicationLayer();
builder.Services.AddPersistenceInfrastructure(builder.Configuration);

// Registrar validadores de FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<UserCreateValidator>();

// AutoMapper acá en Web
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(Program).Assembly);
});

// Configurar Autenticación por Cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(2); // Duración de la sesión
    });

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// Habilitar Autenticación y Autorización en el orden correcto
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Ejecutar migraciones automáticas y sembrar datos de prueba
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await eVote360_Pro.Persistence.DatabaseSeeder.SeedDatabaseAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al aplicar las migraciones o sembrar la base de datos.");
    }
}

app.Run();
