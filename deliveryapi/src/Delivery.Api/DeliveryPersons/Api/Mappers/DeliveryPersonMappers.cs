using Delivery.Api.DeliveryPersons.Api.Dtos;
using Delivery.Api.DeliveryPersons.Domain.Commands;
using Delivery.Api.DeliveryPersons.Domain.Models;

namespace Delivery.Api.DeliveryPersons.Api.Mappers;

public static class DeliveryPersonMappers
{
    // Model -> Resource
    public static DeliveryPersonResource ToResource(this DeliveryPerson model) =>
        new(
            Id: model.Id,
            Name: model.Name,
            Code: model.Code,
            Phone: model.Phone,
            AssignedShipments: model.AssignedShipments.ToList(),
            IsActive: model.IsActive);

    // Create DTO -> Command
    public static CreateDeliveryPersonCommand ToCreateCommand(this CreateDeliveryPersonResource resource) =>
        new(
            Name: resource.Name,
            Code: resource.Code,
            Phone: resource.Phone);

    // Update DTO -> Command
    public static UpdateDeliveryPersonCommand ToUpdateCommand(
        this UpdateDeliveryPersonResource resource,
        string id) =>
        new(
            Id: id,
            Name: resource.Name,
            Phone: resource.Phone);
}