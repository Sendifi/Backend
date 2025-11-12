using WebApplication2.Courier.Domain.Model.Queries;
// alias
using CourierEntity = WebApplication2.Courier.Domain.Model.Aggregates.Courier;

namespace WebApplication2.Courier.Domain.Services;

public interface ICourierQueryService
{
    Task<IEnumerable<CourierEntity>> Handle(GetAllCouriersQuery q);
    Task<CourierEntity?> Handle(GetCourierByIdQuery q);
}