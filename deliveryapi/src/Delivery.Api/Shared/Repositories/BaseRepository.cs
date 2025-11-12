namespace Delivery.Api.Shared.Repositories;

public class BaseRepository<T> where T : class
{
    // Lista estática en memoria
    protected static readonly List<T> Items = new();

    public virtual Task<IEnumerable<T>> ListAsync()
        => Task.FromResult<IEnumerable<T>>(Items);

    public virtual Task AddAsync(T entity)
    {
        Items.Add(entity);
        return Task.CompletedTask;
    }

    public virtual void Remove(T entity) => Items.Remove(entity);
}