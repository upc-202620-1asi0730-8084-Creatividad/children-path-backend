using ChildrenPath.Platform.Shared.Domain;
using ChildrenPath.Platform.Shared.Domain.Model;
using ChildrenPath.Platform.Shared.Domain.Repositories;
using ChildrenPath.Platform.Students.Application.CommandServices;
using ChildrenPath.Platform.Students.Domain.Model.Aggregates;
using ChildrenPath.Platform.Students.Domain.Model.Commands;
using ChildrenPath.Platform.Students.Domain.Model.Entities;
using ChildrenPath.Platform.Students.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Students.Application.Internal.CommandServices;

public class StudentCommandService(
    IStudentRepository studentRepository,
    IStudentActivityRepository activityRepository,
    IUnitOfWork unitOfWork) : IStudentCommandService
{
    private static readonly string[] AllowedStatuses = ["active", "unassigned", "review", "inactive"];

    public async Task<Result<Student>> Handle(CreateStudentCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var normalized = Normalize(command);
            ValidateStatus(normalized.Status);

            var codeNumber = await studentRepository.GetNextCodeNumberAsync(cancellationToken);
            var student = new Student(normalized) { Code = $"ST-{codeNumber:D3}" };
            
            await studentRepository.AddAsync(student, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            await activityRepository.AddAsync(CreateActivity(student, "Student registered", "The student record was created."), cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            
            return Result<Student>.Success(student);
        }
        catch (InvalidOperationException ex)
        {
            // Captura tus validaciones de Required() y ValidateStatus()
            return Result<Student>.Failure(Error.Validation("Students.InvalidData", ex.Message));
        }
        catch (DbUpdateException)
        {
            return Result<Student>.Failure(Error.Unexpected("Students.DatabaseError", "A database error occurred."));
        }
    }

    public async Task<Result<Student>> Handle(UpdateStudentCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var normalized = Normalize(command);
            ValidateStatus(normalized.Status);

            var student = await studentRepository.FindWithRelationsByIdAsync(normalized.Id, cancellationToken);
            if (student == null) 
                return Result<Student>.Failure(Error.NotFound("Students.NotFound", "The specified student was not found."));

            student.Update(normalized);
            studentRepository.Update(student);
            
            await activityRepository.AddAsync(
                CreateActivity(student, "Student record updated", "The student profile was updated."),
                cancellationToken);
                
            await unitOfWork.CompleteAsync(cancellationToken);
            
            return Result<Student>.Success(student);
        }
        catch (InvalidOperationException ex)
        {
            return Result<Student>.Failure(Error.Validation("Students.InvalidData", ex.Message));
        }
        catch (DbUpdateException)
        {
            return Result<Student>.Failure(Error.Unexpected("Students.DatabaseError", "A database error occurred."));
        }
    }

    public async Task<Result> Handle(DeleteStudentCommand command, CancellationToken cancellationToken = default)
    {
        var student = await studentRepository.FindWithRelationsByIdAsync(command.Id, cancellationToken);
        if (student == null) 
            return Result.Failure(Error.NotFound("Students.NotFound", "The specified student was not found."));

        try
        {
            await activityRepository.AddAsync(
                CreateActivity(student, "Student record deleted", $"The record for {FullName(student)} was deleted."),
                cancellationToken);
                
            studentRepository.Remove(student);
            await unitOfWork.CompleteAsync(cancellationToken);
            
            return Result.Success();
        }
        catch (DbUpdateException)
        {
            return Result.Failure(Error.Unexpected("Students.DatabaseError", "A database error occurred."));
        }
    }

    // ==========================================
    // A PARTIR DE AQUÍ, TUS MÉTODOS PRIVADOS SE QUEDAN EXACTAMENTE IGUAL
    // ==========================================

    private static CreateStudentCommand Normalize(CreateStudentCommand command)
    {
        return command with
        {
            FirstName = Required(command.FirstName, "First name"),
            LastName = Required(command.LastName, "Last name"),
            Grade = Required(command.Grade, "Grade"),
            School = Required(command.School, "School"),
            GuardianName = Required(command.GuardianName, "Guardian name"),
            GuardianPhone = Required(command.GuardianPhone, "Guardian phone"),
            GuardianEmail = Optional(command.GuardianEmail),
            EmergencyContact = Optional(command.EmergencyContact),
            RouteName = Optional(command.RouteName),
            PickupPoint = Optional(command.PickupPoint),
            PickupWindow = Optional(command.PickupWindow),
            Status = Required(command.Status, "Status").ToLowerInvariant()
        };
    }

    private static UpdateStudentCommand Normalize(UpdateStudentCommand command)
    {
        return command with
        {
            FirstName = Required(command.FirstName, "First name"),
            LastName = Required(command.LastName, "Last name"),
            Grade = Required(command.Grade, "Grade"),
            School = Required(command.School, "School"),
            GuardianName = Required(command.GuardianName, "Guardian name"),
            GuardianPhone = Required(command.GuardianPhone, "Guardian phone"),
            GuardianEmail = Optional(command.GuardianEmail),
            EmergencyContact = Optional(command.EmergencyContact),
            RouteName = Optional(command.RouteName),
            PickupPoint = Optional(command.PickupPoint),
            PickupWindow = Optional(command.PickupWindow),
            Status = Required(command.Status, "Status").ToLowerInvariant()
        };
    }

    private static string Required(string? value, string field)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException($"{field} is required.");
        return value.Trim();
    }

    private static string? Optional(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static void ValidateStatus(string status)
    {
        if (!AllowedStatuses.Contains(status, StringComparer.Ordinal))
            throw new InvalidOperationException("Status must be one of: active, unassigned, review, inactive.");
    }

    private static StudentActivity CreateActivity(Student student, string title, string description)
    {
        return new StudentActivity
        {
            StudentId = student.Id,
            StudentCode = student.Code,
            StudentName = FullName(student),
            OccurredAt = DateTime.UtcNow,
            Title = title,
            Description = description,
            Status = "completed"
        };
    }

    private static string FullName(Student student) => $"{student.FirstName} {student.LastName}".Trim();
}