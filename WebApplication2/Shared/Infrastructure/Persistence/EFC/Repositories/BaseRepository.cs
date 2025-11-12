using Microsoft.EntityFrameworkCore;
using WebApplication2.Shared.Domain.Repositories;
using WebApplication2.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace WebApplication2.Shared.Infrastructure.Persistence.EFC.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext Context;
    protected BaseRepository(AppDbContext context) => Context = context;

    public async Task AddAsync(TEntity entity) => await Context.Set<TEntity>().AddAsync(entity);
    public async Task<TEntity?> FindByIdAsync(int id) => await Context.Set<TEntity>().FindAsync(id);
    public async Task<IEnumerable<TEntity>> ListAsync() => await Context.Set<TEntity>().AsNoTracking().ToListAsync();
    public void Update(TEntity entity) => Context.Set<TEntity>().Update(entity);
    public void Remove(TEntity entity) => Context.Set<TEntity>().Remove(entity);
}