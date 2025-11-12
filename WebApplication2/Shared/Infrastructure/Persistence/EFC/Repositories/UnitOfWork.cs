using WebApplication2.Shared.Domain.Repositories;
using WebApplication2.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace WebApplication2.Shared.Infrastructure.Persistence.EFC.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public async Task CompleteAsync() => await context.SaveChangesAsync();
}