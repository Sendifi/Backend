using backSendify.Delivery.Application.DeliveryPersons.Dtos;
using backSendify.Delivery.Application.DeliveryPersons.Requests;

namespace backSendify.Delivery.Application.Abstractions;

public interface IDeliveryPersonService
{
    Task<IEnumerable<DeliveryPersonDto>> GetAsync(DeliveryPersonQueryParameters query, CancellationToken cancellationToken);
    Task<DeliveryPersonDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<DeliveryPersonDto> CreateAsync(DeliveryPersonCreateRequest request, CancellationToken cancellationToken);
    Task<DeliveryPersonDto?> UpdateAsync(Guid id, DeliveryPersonUpdateRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
