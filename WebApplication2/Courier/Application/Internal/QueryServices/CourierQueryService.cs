using WebApplication2.Courier.Domain.Model.Queries;
using WebApplication2.Courier.Domain.Repositories;
using WebApplication2.Courier.Domain.Services;
// alias
using CourierEntity = WebApplication2.Courier.Domain.Model.Aggregates.Courier;

namespace WebApplication2.Courier.Application.Internal.QueryServices;

public class CourierQueryService(ICourierRepository repo) : ICourierQueryService
{
    public async Task<IEnumerable<CourierEntity>> Handle(GetAllCouriersQuery q) => await repo.ListAsync();
    public async Task<CourierEntity?> Handle(GetCourierByIdQuery q) => await repo.FindByIdAsync(q.Id);
}