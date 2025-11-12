using Microsoft.EntityFrameworkCore;
using WebApplication2.Courier.Domain.Repositories;
using WebApplication2.Shared.Infrastructure.Persistence.EFC.Configuration;
using WebApplication2.Shared.Infrastructure.Persistence.EFC.Repositories;
// alias
using CourierEntity = WebApplication2.Courier.Domain.Model.Aggregates.Courier;

namespace WebApplication2.Courier.Infrastructure.Repositories;

public class CourierRepository(AppDbContext context)
    : BaseRepository<CourierEntity>(context), ICourierRepository
{
    public async Task<CourierEntity?> FindByNameAsync(string name) =>
        await Context.Set<CourierEntity>().FirstOrDefaultAsync(x => x.Name == name);

    public new async Task<IEnumerable<CourierEntity>> ListAsync() =>
        await Context.Set<CourierEntity>().AsNoTracking().OrderBy(x => x.Name).ToListAsync();
}