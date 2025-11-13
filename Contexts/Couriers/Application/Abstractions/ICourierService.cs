using backSendify.Couriers.Application.Couriers.Dtos;
using backSendify.Couriers.Application.Couriers.Requests;

namespace backSendify.Couriers.Application.Abstractions;

public interface ICourierService
{
    Task<IEnumerable<CourierDto>> GetAsync(CancellationToken cancellationToken);
    Task<CourierDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<CourierDto> CreateAsync(CourierCreateRequest request, CancellationToken cancellationToken);
    Task<CourierDto?> UpdateAsync(Guid id, CourierUpdateRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
