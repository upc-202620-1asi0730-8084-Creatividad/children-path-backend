using ChildrenPath.Platform.Students.Domain.Model.Aggregates;
using ChildrenPath.Platform.Students.Domain.Model.Commands;

namespace ChildrenPath.Platform.Students.Application.CommandServices;

public interface IStudentCommandService
{
    Task<Student> Handle(CreateStudentCommand command, CancellationToken cancellationToken = default);
    Task<Student?> Handle(UpdateStudentCommand command, CancellationToken cancellationToken = default);
    Task<bool> Handle(DeleteStudentCommand command, CancellationToken cancellationToken = default);
}