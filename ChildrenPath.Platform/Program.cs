using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using ChildrenPath.Platform.Shared.Infrastructure;
using ChildrenPath.Platform.Shared.Domain.Repositories;

// Repositorios
using ChildrenPath.Platform.Fleet.Infrastructure.Repositories;
using ChildrenPath.Platform.Companies.Domain.Repositories;
using ChildrenPath.Platform.Companies.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using ChildrenPath.Platform.Companies.Application.CommandServices;
using ChildrenPath.Platform.Companies.Application.QueryServices;
using ChildrenPath.Platform.Companies.Application.Internal.CommandServices;
using ChildrenPath.Platform.Companies.Application.Internal.QueryServices;
using ChildrenPath.Platform.Students.Infrastructure.Repositories;

using ChildrenPath.Platform.Alerts.Domain.Repositories;
using ChildrenPath.Platform.Alerts.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using ChildrenPath.Platform.Alerts.Application.CommandServices;
using ChildrenPath.Platform.Alerts.Application.QueryServices;
using ChildrenPath.Platform.Alerts.Application.Internal.CommandServices;
using ChildrenPath.Platform.Alerts.Application.Internal.QueryServices;
using ChildrenPath.Platform.Infrastructure;
using ChildrenPath.Platform.Trips.Domain.Repositories;
using ChildrenPath.Platform.Trips.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using ChildrenPath.Platform.Trips.Application.CommandServices;
using ChildrenPath.Platform.Trips.Application.QueryServices;
using ChildrenPath.Platform.Trips.Application.Internal.CommandServices;
using ChildrenPath.Platform.Trips.Application.Internal.QueryServices;

using ChildrenPath.Platform.Analytics.Domain.Repositories;
using ChildrenPath.Platform.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using ChildrenPath.Platform.Analytics.Application.CommandServices;
using ChildrenPath.Platform.Analytics.Application.QueryServices;
using ChildrenPath.Platform.Analytics.Application.Internal.CommandServices;
using ChildrenPath.Platform.Analytics.Application.Internal.QueryServices;

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
// builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));

// AHORA (Temporal para desarrollo):
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("ChildrenPathDb_Dev")); 

// --- INYECCIÓN DE DEPENDENCIAS (BOUNDED CONTEXTS) ---

// Shared Context
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Fleet Bounded Context
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();

// Companies Bounded Context
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ISchoolContractRepository, SchoolContractRepository>();
builder.Services.AddScoped<ICompanyMemberRepository, CompanyMemberRepository>();
builder.Services.AddScoped<IComplianceItemRepository, ComplianceItemRepository>();
builder.Services.AddScoped<ICompanyActivityRepository, CompanyActivityRepository>();
builder.Services.AddScoped<ICompanyCommandService, CompanyCommandService>();
builder.Services.AddScoped<ICompanyQueryService, CompanyQueryService>();

// (Aquí irás agregando los repositorios de Students, Routes, etc.)
// Students Bounded Context
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

// Repositorios
builder.Services.AddScoped<IAlertRepository, AlertRepository>();
// builder.Services.AddScoped<INotificationTemplateRepository, NotificationTemplateRepository>();

// Servicios de Aplicación (Commands & Queries)
builder.Services.AddScoped<IAlertCommandService, AlertCommandService>();
builder.Services.AddScoped<IAlertQueryService, AlertQueryService>();
// Trips Bounded Context
builder.Services.AddScoped<ITripRepository, TripRepository>();

builder.Services.AddScoped<ITripCommandService, TripCommandService>();
builder.Services.AddScoped<ITripQueryService, TripQueryService>();

// Analytics Bounded Context
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IReportCommandService, ReportCommandService>();
builder.Services.AddScoped<IReportQueryService, ReportQueryService>();

// (Compañeros, aquí iran agregando los repositorios de Students, Routes, etc.)

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

//app.UseHttpsRedirection();
app.UseCors("AllowVueApp");
app.UseAuthorization();
app.MapControllers();

app.Run();