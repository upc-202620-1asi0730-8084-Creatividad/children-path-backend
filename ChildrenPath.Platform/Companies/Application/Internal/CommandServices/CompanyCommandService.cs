using ChildrenPath.Platform.Companies.Application.CommandServices;
using ChildrenPath.Platform.Companies.Domain.Model.Aggregates;
using ChildrenPath.Platform.Companies.Domain.Model.Commands;
using ChildrenPath.Platform.Companies.Domain.Model.Entities;
using ChildrenPath.Platform.Companies.Domain.Repositories;
using ChildrenPath.Platform.Shared.Domain;
using ChildrenPath.Platform.Shared.Domain.Model;
using ChildrenPath.Platform.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Companies.Application.Internal.CommandServices;

public class CompanyCommandService(
    ICompanyRepository companyRepository,
    ISchoolContractRepository schoolContractRepository,
    ICompanyMemberRepository companyMemberRepository,
    IComplianceItemRepository complianceItemRepository,
    ICompanyActivityRepository companyActivityRepository,
    IUnitOfWork unitOfWork) : ICompanyCommandService
{
    public async Task<Result<Company>> Handle(CreateCompanyCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            if (await companyRepository.ExistsByRucAsync(command.Ruc, cancellationToken))
            {
                return Result<Company>.Failure(Error.Conflict("Companies.DuplicateRuc", $"Una empresa con el RUC {command.Ruc} ya se encuentra registrada."));
            }

            var company = new Company(command);
            await companyRepository.AddAsync(company, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            var activity = new CompanyActivity(new CreateCompanyActivityCommand(
                company.Id,
                DateTime.UtcNow,
                "Registro de empresa completado",
                $"La empresa '{company.CommercialName}' fue dada de alta en el sistema.",
                "completed"
            ));
            await companyActivityRepository.AddAsync(activity, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<Company>.Success(company);
        }
        catch (InvalidOperationException ex)
        {
            return Result<Company>.Failure(Error.Validation("Companies.InvalidData", ex.Message));
        }
        catch (DbUpdateException)
        {
            return Result<Company>.Failure(Error.Unexpected("Companies.DatabaseError", "A database error occurred."));
        }
    }

    public async Task<Result<Company>> Handle(UpdateCompanyCommand command, CancellationToken cancellationToken = default)
    {
        var company = await companyRepository.FindByIdAsync(command.Id, cancellationToken);
        if (company == null) 
            return Result<Company>.Failure(Error.NotFound("Companies.NotFound", "The specified company was not found."));

        try
        {
            company.Update(command);
            companyRepository.Update(company);

            var activity = new CompanyActivity(new CreateCompanyActivityCommand(
                company.Id,
                DateTime.UtcNow,
                "Perfil corporativo actualizado",
                "Se modificaron los datos comerciales y de contacto de la empresa.",
                "completed"
            ));
            await companyActivityRepository.AddAsync(activity, cancellationToken);

            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Company>.Success(company);
        }
        catch (InvalidOperationException ex)
        {
            return Result<Company>.Failure(Error.Validation("Companies.InvalidData", ex.Message));
        }
        catch (DbUpdateException)
        {
            return Result<Company>.Failure(Error.Unexpected("Companies.DatabaseError", "A database error occurred."));
        }
    }

    public async Task<Result> HandleDelete(int id, CancellationToken cancellationToken = default)
    {
        var company = await companyRepository.FindByIdAsync(id, cancellationToken);
        if (company == null) 
            return Result.Failure(Error.NotFound("Companies.NotFound", "The specified company was not found."));

        try
        {
            companyRepository.Remove(company);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }
        catch (DbUpdateException)
        {
            return Result.Failure(Error.Unexpected("Companies.DatabaseError", "A database error occurred."));
        }
    }

    public async Task<Result<ComplianceItem>> Handle(UpdateComplianceItemStatusCommand command, CancellationToken cancellationToken = default)
    {
        var item = await complianceItemRepository.FindByIdAsync(command.Id, cancellationToken);
        if (item == null) 
            return Result<ComplianceItem>.Failure(Error.NotFound("Companies.ComplianceItemNotFound", "Compliance item not found."));

        try
        {
            item.UpdateStatus(command.Status);
            complianceItemRepository.Update(item);

            var activity = new CompanyActivity(new CreateCompanyActivityCommand(
                item.CompanyId,
                DateTime.UtcNow,
                $"Auditoría actualizada: {item.Title}",
                $"El estado del ítem fue cambiado a '{command.Status}'.",
                "completed"
            ));
            await companyActivityRepository.AddAsync(activity, cancellationToken);

            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<ComplianceItem>.Success(item);
        }
        catch (InvalidOperationException ex)
        {
            return Result<ComplianceItem>.Failure(Error.Validation("Companies.InvalidData", ex.Message));
        }
        catch (DbUpdateException)
        {
            return Result<ComplianceItem>.Failure(Error.Unexpected("Companies.DatabaseError", "A database error occurred."));
        }
    }

    // ==========================================
    // LOS SIGUIENTES MÉTODOS SE MANTIENEN IGUAL 
    // (Devuelven entidades, no usan Result)
    // ==========================================

    public async Task<Result<SchoolContract>> Handle(CreateSchoolContractCommand command, CancellationToken cancellationToken = default)
    {
        var contract = new SchoolContract(command);
        await schoolContractRepository.AddAsync(contract, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<SchoolContract>.Success(contract);
    }

    public async Task<Result<CompanyMember>> Handle(CreateCompanyMemberCommand command, CancellationToken cancellationToken = default)
    {
        var member = new CompanyMember(command);
        await companyMemberRepository.AddAsync(member, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<CompanyMember>.Success(member);
    }

    public async Task<Result<ComplianceItem>> Handle(CreateComplianceItemCommand command, CancellationToken cancellationToken = default)
    {
        var item = new ComplianceItem(command);
        await complianceItemRepository.AddAsync(item, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<ComplianceItem>.Success(item);
    }

    public async Task<Result<CompanyActivity>> Handle(CreateCompanyActivityCommand command, CancellationToken cancellationToken = default)
    {
        var activity = new CompanyActivity(command);
        await companyActivityRepository.AddAsync(activity, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<CompanyActivity>.Success(activity);
    }

    public async Task SeedInitialDataAsync(CancellationToken cancellationToken = default)
    {
        var existing = await companyRepository.GetDefaultCompanyAsync(cancellationToken);
        if (existing != null) return;

        var defaultCompany = new Company(new CreateCompanyCommand(
            LegalName: "Transportes Escolares del Perú S.A.C.",
            CommercialName: "TransEscolar S.A.C.",
            Ruc: "20601234567",
            Status: "active",
            Plan: "Enterprise Fleet",
            PlanLimitVehicles: 30,
            PlanLimitStudents: 1800,
            AdminName: "Carlos Mendoza",
            AdminEmail: "cmendoza@transescolar.pe",
            Phone: "+51 987 654 321",
            Address: "Av. Javier Prado Este 2450, San Borja, Lima",
            OperatingDistricts: "San Borja, Surco, La Molina, Miraflores, San Isidro",
            Verified: true,
            LicenseExpiration: DateTime.UtcNow.AddYears(2)
        ));
        await companyRepository.AddAsync(defaultCompany, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        var contracts = new List<SchoolContract>
        {
            new(new CreateSchoolContractCommand(defaultCompany.Id, "Colegio San Agustín", "San Isidro", 4, 180, "Lic. Patricia Romero", DateTime.UtcNow.AddMonths(2), "renewal", 98, "Renovación anticipada solicitada")),
            new(new CreateSchoolContractCommand(defaultCompany.Id, "Inmaculado Corazón", "Miraflores", 3, 135, "Ing. Roberto Salas", DateTime.UtcNow.AddMonths(5), "active", 95, "Flota 100% eléctrica asignada")),
            new(new CreateSchoolContractCommand(defaultCompany.Id, "Santa María Marianistas", "Surco", 5, 220, "Dra. Carmen Vega", DateTime.UtcNow.AddMonths(1), "review", 88, "Revisión de rutas en curso")),
            new(new CreateSchoolContractCommand(defaultCompany.Id, "Villa Caritas & San Pedro", "La Molina", 4, 160, "Lic. Fernando Ruiz", DateTime.UtcNow.AddMonths(8), "active", 97, "Excelente puntualidad"))
        };

        foreach (var c in contracts)
        {
            await schoolContractRepository.AddAsync(c, cancellationToken);
        }

        var members = new List<CompanyMember>
        {
            new(new CreateCompanyMemberCommand(defaultCompany.Id, "Carlos Mendoza", "cmendoza@transescolar.pe", "Administrador General", "active", DateTime.UtcNow.AddHours(-1), "Nivel Corporativo")),
            new(new CreateCompanyMemberCommand(defaultCompany.Id, "Lucía Paredes", "lparedes@transescolar.pe", "Coordinadora de Operaciones", "active", DateTime.UtcNow.AddHours(-3), "Rutas y Despacho")),
            new(new CreateCompanyMemberCommand(defaultCompany.Id, "Miguel Ángel Torres", "mtorres@transescolar.pe", "Supervisor de Seguridad Vial", "active", DateTime.UtcNow.AddHours(-5), "Auditorías y Flota")),
            new(new CreateCompanyMemberCommand(defaultCompany.Id, "Ana Belén Quispe", "aquispe@transescolar.pe", "Atención al Cliente y Colegios", "active", DateTime.UtcNow.AddDays(-1), "Soporte Institucional")),
            new(new CreateCompanyMemberCommand(defaultCompany.Id, "Jorge Ramírez", "jramirez@transescolar.pe", "Jefe de Mantenimiento", "invited", null, "Taller y Mecánica"))
        };

        foreach (var m in members)
        {
            await companyMemberRepository.AddAsync(m, cancellationToken);
        }

        var compliance = new List<ComplianceItem>
        {
            new(new CreateComplianceItemCommand(defaultCompany.Id, "SOAT de Flota Completo", "Seguros", "Miguel Torres", DateTime.UtcNow.AddDays(15), "completed", "high", "30 pólizas vigentes registradas")),
            new(new CreateComplianceItemCommand(defaultCompany.Id, "Revisión Técnica Vehicular", "Técnico", "Jorge Ramírez", DateTime.UtcNow.AddDays(5), "pending", "high", "4 unidades pendientes de inspección")),
            new(new CreateComplianceItemCommand(defaultCompany.Id, "Capacitación de Primeros Auxilios", "Personal", "Lucía Paredes", DateTime.UtcNow.AddDays(20), "completed", "medium", "22 conductores certificados")),
            new(new CreateComplianceItemCommand(defaultCompany.Id, "Licencia Municipal de Operación", "Legal", "Carlos Mendoza", DateTime.UtcNow.AddDays(45), "completed", "high", "Autorización ATU vigente 2026")),
            new(new CreateComplianceItemCommand(defaultCompany.Id, "Pruebas Toxicológicas Semestrales", "Salud", "Miguel Torres", DateTime.UtcNow.AddDays(-3), "pending", "medium", "Reprogramado para próxima semana"))
        };

        foreach (var ci in compliance)
        {
            await complianceItemRepository.AddAsync(ci, cancellationToken);
        }

        var activities = new List<CompanyActivity>
        {
            new(new CreateCompanyActivityCommand(defaultCompany.Id, DateTime.UtcNow.AddHours(-2), "Contrato Renovado", "Se extendió el contrato con Colegio San Agustín por 1 año más.", "completed")),
            new(new CreateCompanyActivityCommand(defaultCompany.Id, DateTime.UtcNow.AddHours(-5), "Auditoría de Seguridad", "Revisión de botiquines y extintores al 100% de la flota.", "completed")),
            new(new CreateCompanyActivityCommand(defaultCompany.Id, DateTime.UtcNow.AddDays(-1), "Nuevo Conductor Registrado", "Se completó la inducción y validación de antecedentes de Jorge Ramírez.", "completed")),
            new(new CreateCompanyActivityCommand(defaultCompany.Id, DateTime.UtcNow.AddDays(-2), "Mantenimiento Preventivo", "8 vehículos completaron cambio de frenos y fluidos.", "completed"))
        };

        foreach (var a in activities)
        {
            await companyActivityRepository.AddAsync(a, cancellationToken);
        }

        await unitOfWork.CompleteAsync(cancellationToken);
    }
}