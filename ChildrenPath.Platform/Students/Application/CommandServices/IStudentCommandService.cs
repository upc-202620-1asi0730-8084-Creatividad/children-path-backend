using ChildrenPath.Platform.Shared.Domain;
using ChildrenPath.Platform.Students.Domain.Model.Aggregates;
using ChildrenPath.Platform.Students.Domain.Model.Commands;

namespace ChildrenPath.Platform.Students.Application.CommandServices;

public interface IStudentCommandService
{
    Task<Result<Student>> Handle(CreateStudentCommand command, CancellationToken cancellationToken = default);
    Task<Result<Student>> Handle(UpdateStudentCommand command, CancellationToken cancellationToken = default);
    Task<Result> Handle(DeleteStudentCommand command, CancellationToken cancellationToken = default);
}