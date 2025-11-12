namespace WebApplication2.Shared.Domain.Repositories;

public interface IBaseRepository<T> where T : class
{
    Task AddAsync(T entity);
    Task<T?> FindByIdAsync(int id);
    Task<IEnumerable<T>> ListAsync();
    void Update(T entity);
    void Remove(T entity);
}