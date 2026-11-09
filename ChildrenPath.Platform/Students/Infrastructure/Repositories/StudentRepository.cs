using ChildrenPath.Platform.Shared.Domain.Repositories;
using ChildrenPath.Platform.Shared.Infrastructure;
using ChildrenPath.Platform.Students.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Students.Infrastructure.Repositories;

public interface IStudentRepository : IBaseRepository<Student> { }

public class StudentRepository(AppDbContext context) : IStudentRepository
{
    public async Task AddAsync(Student entity, CancellationToken cancellationToken = default)
        => await context.Students.AddAsync(entity, cancellationToken);

    public async Task<Student?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        => await context.Students.FindAsync([id], cancellationToken);

    public void Update(Student entity) => context.Students.Update(entity);

    public void Remove(Student entity) => context.Students.Remove(entity);

    public async Task<IEnumerable<Student>> ListAsync(CancellationToken cancellationToken = default)
        => await context.Students.ToListAsync(cancellationToken);
}