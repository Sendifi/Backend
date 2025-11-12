using WebApplication2.Courier.Domain.Model.Commands;
// alias
using CourierEntity = WebApplication2.Courier.Domain.Model.Aggregates.Courier;

namespace WebApplication2.Courier.Domain.Services;

public interface ICourierCommandService
{
    Task<CourierEntity?> Handle(CreateCourierCommand command);
    Task<CourierEntity?> Update(int id, string? name, decimal? costPerKg, int? estimatedDays, bool? isActive);
    Task<bool> Delete(int id);
}