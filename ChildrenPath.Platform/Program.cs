using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using ChildrenPath.Platform.Shared.Infrastructure;
using ChildrenPath.Platform.Shared.Domain.Repositories;
using ChildrenPath.Platform.Fleet.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. CONFIGURACIÓN DE SERVICIOS (DI)
// ==========================================

builder.Services.AddControllers();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "Children Path API";
        document.Info.Version = "v1";
        document.Info.Description = "RESTful API for Children Path School Transport Platform";
        return Task.CompletedTask;
    });
});

// Configurar CORS para permitir peticiones del Frontend (Vue)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Configurar Base de Datos (MySQL)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- INYECCIÓN DE DEPENDENCIAS (BOUNDED CONTEXTS) ---

// Shared Context
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Fleet Bounded Context
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();

// (Aquí irás agregando los repositorios de Students, Routes, etc.)

// ==========================================
// 2. CONSTRUCCIÓN DE LA APP
// ==========================================
var app = builder.Build();

// ==========================================
// 3. PIPELINE DE HTTP (MIDDLEWARES)
// ==========================================
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Children Path API Reference")
            .WithTheme(ScalarTheme.Moon);
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowVueApp");
app.UseAuthorization();
app.MapControllers();

app.Run();