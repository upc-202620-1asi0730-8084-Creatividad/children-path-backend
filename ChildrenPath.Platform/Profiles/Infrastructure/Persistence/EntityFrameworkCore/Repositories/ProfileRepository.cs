using ChildrenPath.Platform.Profiles.Domain.Model.Aggregates;
using ChildrenPath.Platform.Profiles.Domain.Repositories;
using ChildrenPath.Platform.Shared.Infrastructure;
using ChildrenPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace ChildrenPath.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ProfileRepository(AppDbContext context)
    : BaseRepository<Profile>(context), IProfileRepository;