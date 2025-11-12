using Delivery.Api.DeliveryPersons.Domain.Models;
using Delivery.Api.DeliveryPersons.Domain.Queries;

namespace Delivery.Api.DeliveryPersons.Domain.Services;

public interface IDeliveryPersonQueryService
{
    Task<IEnumerable<DeliveryPerson>> Handle(GetAllDeliveryPersonsQuery query);
    Task<DeliveryPerson?> Handle(GetDeliveryPersonByIdQuery query);
    Task<DeliveryPerson?> Handle(GetDeliveryPersonByCodeQuery query);
}