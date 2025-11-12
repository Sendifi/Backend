using WebApplication2.Shared.Domain.Repositories;
// alias
using CourierEntity = WebApplication2.Courier.Domain.Model.Aggregates.Courier;

namespace WebApplication2.Courier.Domain.Repositories;

public interface ICourierRepository : IBaseRepository<CourierEntity>
{
    Task<CourierEntity?> FindByNameAsync(string name);
    Task<IEnumerable<CourierEntity>> ListAsync();
}