using ChildrenPath.Platform.Shared.Domain.Repositories;
using ChildrenPath.Platform.Students.Domain.Model.Aggregates;

namespace ChildrenPath.Platform.Students.Domain.Repositories;

public interface IStudentRepository : IBaseRepository<Student>
{
    Task<Student?> FindWithRelationsByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Student>> ListWithRelationsAsync(CancellationToken cancellationToken = default);
    Task<int> GetNextCodeNumberAsync(CancellationToken cancellationToken = default);
}