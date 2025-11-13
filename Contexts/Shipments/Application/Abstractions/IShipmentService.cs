using backSendify.Shipments.Application.Shipments.Dtos;
using backSendify.Shipments.Application.Shipments.Requests;

namespace backSendify.Shipments.Application.Abstractions;

public interface IShipmentService
{
    Task<IEnumerable<ShipmentDto>> GetShipmentsAsync(ShipmentQueryParameters query, CancellationToken cancellationToken);
    Task<ShipmentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<ShipmentDto> CreateAsync(ShipmentCreateRequest request, CancellationToken cancellationToken);
    Task<ShipmentDto?> UpdateAsync(Guid id, ShipmentUpdateRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
