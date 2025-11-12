using Delivery.Api.DeliveryPersons.Domain.Models;
using Delivery.Api.DeliveryPersons.Domain.Queries;
using Delivery.Api.DeliveryPersons.Domain.Repositories;
using Delivery.Api.DeliveryPersons.Domain.Services;

namespace Delivery.Api.DeliveryPersons.Infrastructure.Services;

public class DeliveryPersonQueryService : IDeliveryPersonQueryService
{
    private readonly IDeliveryPersonRepository _repository;

    public DeliveryPersonQueryService(IDeliveryPersonRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DeliveryPerson>> Handle(GetAllDeliveryPersonsQuery query)
    {
        return await _repository.ListAsync();
    }

    public async Task<DeliveryPerson?> Handle(GetDeliveryPersonByIdQuery query)
    {
        return await _repository.FindByIdAsync(query.Id);
    }

    public async Task<DeliveryPerson?> Handle(GetDeliveryPersonByCodeQuery query)
    {
        return await _repository.FindByCodeAsync(query.Code);
    }
}