using System.ComponentModel.DataAnnotations;
using ChildrenPath.Platform.Drivers.Application.CommandServices;
using ChildrenPath.Platform.Drivers.Domain.Model.Aggregates;
using ChildrenPath.Platform.Drivers.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Drivers.Domain.Model.Commands;
using ChildrenPath.Platform.Drivers.Domain.Model.Entities;
using ChildrenPath.Platform.Drivers.Domain.Repositories;
using ChildrenPath.Platform.Shared.Domain;
using ChildrenPath.Platform.Shared.Domain.Model;
using ChildrenPath.Platform.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Drivers.Application.Internal.CommandServices;

public class DriverCommandService(
    IDriverRepository driverRepository,
    IDriverActivityRepository activityRepository,
    IUnitOfWork unitOfWork) : IDriverCommandService
{
    public async Task<Result<Driver>> Handle(CreateDriverCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var normalized = Normalize(command);
            Validate(normalized);

            if (await driverRepository.ExistsByEmailAsync(normalized.Email, cancellationToken: cancellationToken))
                return Result<Driver>.Failure(Error.Conflict("Drivers.DuplicateEmail", "A driver with this email already exists."));
                
            if (await driverRepository.ExistsByLicenseNumberAsync(normalized.LicenseNumber, cancellationToken: cancellationToken))
                return Result<Driver>.Failure(Error.Conflict("Drivers.DuplicateLicense", "A driver with this license number already exists."));

            var driver = new Driver(normalized);
            await driverRepository.AddAsync(driver, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            driver.Code = $"DRV-{driver.Id:D3}";
            await activityRepository.AddAsync(
                CreateActivity(driver, "Driver registered", "The driver profile was created."), cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            
            return Result<Driver>.Success(driver);
        }
        catch (InvalidOperationException ex)
        {
            return Result<Driver>.Failure(Error.Validation("Drivers.InvalidData", ex.Message));
        }
        catch (DbUpdateException)
        {
            return Result<Driver>.Failure(Error.Unexpected("Drivers.DatabaseError", "A database error occurred."));
        }
    }

    public async Task<Result<Driver>> Handle(UpdateDriverCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var normalized = Normalize(command);
            Validate(normalized);

            var driver = await driverRepository.FindWithRelationsByIdAsync(normalized.Id, cancellationToken);
            if (driver == null) 
                return Result<Driver>.Failure(Error.NotFound("Drivers.NotFound", "The specified driver was not found."));

            if (await driverRepository.ExistsByEmailAsync(normalized.Email, normalized.Id, cancellationToken))
                return Result<Driver>.Failure(Error.Conflict("Drivers.DuplicateEmail", "A driver with this email already exists."));
                
            if (await driverRepository.ExistsByLicenseNumberAsync(normalized.LicenseNumber, normalized.Id, cancellationToken))
                return Result<Driver>.Failure(Error.Conflict("Drivers.DuplicateLicense", "A driver with this license number already exists."));

            var previousStatus = driver.Status;
            driver.Update(normalized);
            driverRepository.Update(driver);

            var activity = previousStatus == driver.Status
                ? CreateActivity(driver, "Driver profile updated", "The driver profile was updated.")
                : CreateActivity(driver, "Driver status updated",
                    $"Driver status changed from {ToDisplayStatus(previousStatus)} to {ToDisplayStatus(driver.Status)}.");
                    
            await activityRepository.AddAsync(activity, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            
            return Result<Driver>.Success(driver);
        }
        catch (InvalidOperationException ex)
        {
            return Result<Driver>.Failure(Error.Validation("Drivers.InvalidData", ex.Message));
        }
        catch (DbUpdateException)
        {
            return Result<Driver>.Failure(Error.Unexpected("Drivers.DatabaseError", "A database error occurred."));
        }
    }

    public async Task<Result> Handle(DeleteDriverCommand command, CancellationToken cancellationToken = default)
    {
        var driver = await driverRepository.FindWithRelationsByIdAsync(command.Id, cancellationToken);
        if (driver == null) 
            return Result.Failure(Error.NotFound("Drivers.NotFound", "The specified driver was not found."));

        try
        {
            await activityRepository.AddAsync(
                CreateActivity(driver, "Driver removed", $"The driver record for {driver.FullName} was removed."),
                cancellationToken);
            driverRepository.Remove(driver);
            await unitOfWork.CompleteAsync(cancellationToken);
            
            return Result.Success();
        }
        catch (DbUpdateException)
        {
            return Result.Failure(Error.Unexpected("Drivers.DatabaseError", "A database error occurred."));
        }
    }

    // ==========================================
    // A PARTIR DE AQUÍ, TUS MÉTODOS PRIVADOS SE MANTIENEN EXACTAMENTE IGUAL
    // ==========================================

    private static CreateDriverCommand Normalize(CreateDriverCommand command)
    {
        return command with
        {
            FullName = Required(command.FullName, "Full name"),
            Phone = Required(command.Phone, "Phone"),
            Email = Required(command.Email, "Email").ToLowerInvariant(),
            LicenseNumber = Required(command.LicenseNumber, "License number").ToUpperInvariant(),
            LicenseClass = Required(command.LicenseClass, "License class"),
            AssignedVehicle = Optional(command.AssignedVehicle),
            AssignedRoute = Optional(command.AssignedRoute),
            PhotoInitials = Optional(command.PhotoInitials),
            AvailabilityLabel = Optional(command.AvailabilityLabel)
        };
    }

    private static UpdateDriverCommand Normalize(UpdateDriverCommand command)
    {
        return command with
        {
            FullName = Required(command.FullName, "Full name"),
            Phone = Required(command.Phone, "Phone"),
            Email = Required(command.Email, "Email").ToLowerInvariant(),
            LicenseNumber = Required(command.LicenseNumber, "License number").ToUpperInvariant(),
            LicenseClass = Required(command.LicenseClass, "License class"),
            AssignedVehicle = Optional(command.AssignedVehicle),
            AssignedRoute = Optional(command.AssignedRoute),
            PhotoInitials = Optional(command.PhotoInitials),
            AvailabilityLabel = Optional(command.AvailabilityLabel)
        };
    }

    private static void Validate(CreateDriverCommand command)
    {
        ValidateDetails(command.Email, command.SafetyScore, command.PunctualityScore,
            command.TripsToday, command.StudentsAssigned, command.YearsOfExperience, command.Status);
    }

    private static void Validate(UpdateDriverCommand command)
    {
        ValidateDetails(command.Email, command.SafetyScore, command.PunctualityScore,
            command.TripsToday, command.StudentsAssigned, command.YearsOfExperience, command.Status);
    }

    private static void ValidateDetails(
        string email,
        int safetyScore,
        int punctualityScore,
        int tripsToday,
        int studentsAssigned,
        int yearsOfExperience,
        DriverStatus status)
    {
        if (!new EmailAddressAttribute().IsValid(email))
            throw new InvalidOperationException("Email must be a valid email address.");
        if (safetyScore is < 0 or > 100 || punctualityScore is < 0 or > 100)
            throw new InvalidOperationException("Safety and punctuality scores must be between 0 and 100.");
        if (tripsToday < 0 || studentsAssigned < 0 || yearsOfExperience < 0)
            throw new InvalidOperationException("Trips, assigned students, and years of experience cannot be negative.");
        if (!Enum.IsDefined(status))
            throw new InvalidOperationException("Status must be available, onRoute, offDuty, or review.");
    }

    private static string Required(string? value, string field)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException($"{field} is required.");
        return value.Trim();
    }

    private static string? Optional(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static DriverActivity CreateActivity(Driver driver, string title, string description)
    {
        return new DriverActivity
        {
            DriverId = driver.Id,
            DriverName = driver.FullName,
            OccurredAt = DateTime.UtcNow,
            Title = title,
            Description = description,
            Status = driver.Status switch
            {
                DriverStatus.OnRoute => "active",
                DriverStatus.Review or DriverStatus.Suspended => "pending",
                _ => "completed"
            }
        };
    }

    private static string ToDisplayStatus(DriverStatus status)
    {
        return status switch
        {
            DriverStatus.OnRoute => "onRoute",
            DriverStatus.OffDuty => "offDuty",
            DriverStatus.Review or DriverStatus.Suspended => "review",
            _ => "available"
        };
    }
}