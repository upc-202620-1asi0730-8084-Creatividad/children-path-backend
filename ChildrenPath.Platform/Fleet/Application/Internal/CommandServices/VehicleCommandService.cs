using ChildrenPath.Platform.Fleet.Application.CommandServices;
using ChildrenPath.Platform.Fleet.Domain.Model.Aggregates;
using ChildrenPath.Platform.Fleet.Domain.Model.Commands;
using ChildrenPath.Platform.Fleet.Domain.Model.Entities;
using ChildrenPath.Platform.Fleet.Domain.Repositories;
using ChildrenPath.Platform.Shared.Domain.Repositories;

namespace ChildrenPath.Platform.Fleet.Application.Internal.CommandServices;

public class VehicleCommandService(
    IVehicleRepository vehicleRepository,
    IMaintenanceAlertRepository maintenanceAlertRepository,
    IFleetRepository fleetRepository,
    IUnitOfWork unitOfWork) : IVehicleCommandService
{
    public async Task<Vehicle?> Handle(CreateVehicleCommand command, CancellationToken cancellationToken = default)
    {
        if (await vehicleRepository.ExistsByPlateAsync(command.Plate, cancellationToken))
        {
            throw new InvalidOperationException($"Ya existe un vehículo con la placa '{command.Plate}'.");
        }

        if (await vehicleRepository.ExistsByCodeAsync(command.Code, cancellationToken))
        {
            throw new InvalidOperationException($"Ya existe un vehículo con el código '{command.Code}'.");
        }

        var vehicle = new Vehicle(command);
        await vehicleRepository.AddAsync(vehicle, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return vehicle;
    }

    public async Task<Vehicle?> Handle(UpdateVehicleCommand command, CancellationToken cancellationToken = default)
    {
        var vehicle = await vehicleRepository.FindByIdAsync(command.Id, cancellationToken);
        if (vehicle == null) return null;

        vehicle.Update(command);
        vehicleRepository.Update(vehicle);
        await unitOfWork.CompleteAsync(cancellationToken);
        return vehicle;
    }

    public async Task<bool> Handle(DeleteVehicleCommand command, CancellationToken cancellationToken = default)
    {
        var vehicle = await vehicleRepository.FindByIdAsync(command.Id, cancellationToken);
        if (vehicle == null) return false;

        vehicleRepository.Remove(vehicle);
        await unitOfWork.CompleteAsync(cancellationToken);
        return true;
    }

    public async Task SeedInitialDataAsync(CancellationToken cancellationToken = default)
    {
        var existingVehicles = await vehicleRepository.ListAsync(cancellationToken);
        if (existingVehicles.Any()) return;

        var defaultFleet = new TransportFleet(new CreateFleetCommand(
            "Flota Escolar Central Lima",
            "Unidades asignadas a los principales colegios de Lima Metropolitana",
            1
        ));
        await fleetRepository.AddAsync(defaultFleet, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        var vehicles = new List<Vehicle>
        {
            new(new CreateVehicleCommand(
                Plate: "KW-204",
                Code: "VAN-01",
                Brand: "Hyundai",
                Model: "H1 School Van",
                Year: 2022,
                Capacity: 18,
                AssignedStudents: 16,
                DriverName: "Carlos Pérez",
                RouteName: "Miraflores - San Isidro",
                Status: "available",
                EnergyType: "gasoline",
                OwnershipType: "company",
                NextMaintenanceDate: DateTime.UtcNow.AddMonths(2),
                LastInspectionDate: DateTime.UtcNow.AddMonths(-4),
                MileageKm: 45000,
                AvailabilityScore: 98,
                DocumentsValid: true,
                FleetId: defaultFleet.Id
            )),
            new(new CreateVehicleCommand(
                Plate: "KW-076",
                Code: "VAN-02",
                Brand: "Mercedes-Benz",
                Model: "Sprinter 516 CDI",
                Year: 2021,
                Capacity: 22,
                AssignedStudents: 20,
                DriverName: "Luis Torres",
                RouteName: "Surco - La Molina",
                Status: "on_route",
                EnergyType: "diesel",
                OwnershipType: "company",
                NextMaintenanceDate: DateTime.UtcNow.AddMonths(1),
                LastInspectionDate: DateTime.UtcNow.AddMonths(-2),
                MileageKm: 62000,
                AvailabilityScore: 95,
                DocumentsValid: true,
                FleetId: defaultFleet.Id
            )),
            new(new CreateVehicleCommand(
                Plate: "KW-310",
                Code: "BUS-01",
                Brand: "Toyota",
                Model: "HiAce Commuter",
                Year: 2023,
                Capacity: 15,
                AssignedStudents: 14,
                DriverName: "Roberto Gómez",
                RouteName: "San Borja - Salamanca",
                Status: "available",
                EnergyType: "hybrid",
                OwnershipType: "company",
                NextMaintenanceDate: DateTime.UtcNow.AddMonths(3),
                LastInspectionDate: DateTime.UtcNow.AddMonths(-1),
                MileageKm: 28000,
                AvailabilityScore: 99,
                DocumentsValid: true,
                FleetId: defaultFleet.Id
            )),
            new(new CreateVehicleCommand(
                Plate: "KW-412",
                Code: "BUS-02",
                Brand: "Volkswagen",
                Model: "Crafter Minibus",
                Year: 2020,
                Capacity: 24,
                AssignedStudents: 0,
                DriverName: "Jorge Ramírez",
                RouteName: "Taller Central",
                Status: "maintenance",
                EnergyType: "diesel",
                OwnershipType: "third_party",
                NextMaintenanceDate: DateTime.UtcNow.AddDays(5),
                LastInspectionDate: DateTime.UtcNow.AddMonths(-6),
                MileageKm: 88000,
                AvailabilityScore: 82,
                DocumentsValid: true,
                FleetId: defaultFleet.Id
            ))
        };

        foreach (var v in vehicles)
        {
            await vehicleRepository.AddAsync(v, cancellationToken);
        }

        var alerts = new List<MaintenanceAlert>
        {
            new(new CreateMaintenanceAlertCommand(
                VehicleCode: "VAN-02",
                Plate: "KW-076",
                Title: "Cambio de pastillas de freno",
                DueDate: DateTime.UtcNow.AddDays(4).ToString("yyyy-MM-dd"),
                Priority: "high",
                Description: "Desgaste del 80% en frenos delanteros reportado por el sistema de telemetría.",
                VehicleId: null
            )),
            new(new CreateMaintenanceAlertCommand(
                VehicleCode: "BUS-02",
                Plate: "KW-412",
                Title: "Revisión técnica vehicular anual",
                DueDate: DateTime.UtcNow.AddDays(11).ToString("yyyy-MM-dd"),
                Priority: "high",
                Description: "Vencimiento obligatorio de certificado de inspección técnica vehicular (CITV).",
                VehicleId: null
            )),
            new(new CreateMaintenanceAlertCommand(
                VehicleCode: "VAN-01",
                Plate: "KW-204",
                Title: "Mantenimiento preventivo 50,000 km",
                DueDate: DateTime.UtcNow.AddDays(21).ToString("yyyy-MM-dd"),
                Priority: "medium",
                Description: "Cambio de aceite sintético, filtros de aire y revisión de suspensión.",
                VehicleId: null
            ))
        };

        foreach (var a in alerts)
        {
            await maintenanceAlertRepository.AddAsync(a, cancellationToken);
        }

        await unitOfWork.CompleteAsync(cancellationToken);
    }
}
