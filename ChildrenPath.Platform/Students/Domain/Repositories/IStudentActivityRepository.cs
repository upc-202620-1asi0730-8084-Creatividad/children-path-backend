using ChildrenPath.Platform.Shared.Domain.Repositories;
using ChildrenPath.Platform.Students.Domain.Model.Entities;

namespace ChildrenPath.Platform.Students.Domain.Repositories;

public interface IStudentActivityRepository : IBaseRepository<StudentActivity>
{
    Task<IEnumerable<StudentActivity>> ListRecentAsync(int limit, CancellationToken cancellationToken = default);
}