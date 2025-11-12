using Delivery.Api.DeliveryPersons.Domain.Commands;
using Delivery.Api.DeliveryPersons.Domain.Models;

namespace Delivery.Api.DeliveryPersons.Domain.Services;

public interface IDeliveryPersonCommandService
{
    Task<DeliveryPerson> Handle(CreateDeliveryPersonCommand command);
    Task<DeliveryPerson?> Handle(UpdateDeliveryPersonCommand command);
    Task<bool> Handle(DeleteDeliveryPersonCommand command);
}