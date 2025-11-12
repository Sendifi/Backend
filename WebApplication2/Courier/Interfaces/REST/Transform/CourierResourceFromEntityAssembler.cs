using WebApplication2.Courier.Interfaces.REST.Resources;
// alias
using CourierEntity = WebApplication2.Courier.Domain.Model.Aggregates.Courier;

namespace WebApplication2.Courier.Interfaces.REST.Transform;

public static class CourierResourceFromEntityAssembler
{
    public static CourierResource ToResourceFromEntity(CourierEntity e) =>
        new(e.Id, e.Name, e.CostPerKg, e.EstimatedDays, e.IsActive);
}