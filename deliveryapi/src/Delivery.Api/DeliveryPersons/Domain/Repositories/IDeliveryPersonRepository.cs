using Delivery.Api.DeliveryPersons.Domain.Models;

namespace Delivery.Api.DeliveryPersons.Domain.Repositories;

public interface IDeliveryPersonRepository
{
    Task<IEnumerable<DeliveryPerson>> ListAsync(string? code = null);
    Task<DeliveryPerson?> FindByIdAsync(string id);
    Task<DeliveryPerson?> FindByCodeAsync(string code);
    Task AddAsync(DeliveryPerson entity);
    void Update(DeliveryPerson entity);
    void Remove(DeliveryPerson entity);
}