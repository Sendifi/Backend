using Delivery.Api.DeliveryPersons.Domain.Commands;
using Delivery.Api.DeliveryPersons.Domain.Models;
using Delivery.Api.DeliveryPersons.Domain.Repositories;
using Delivery.Api.DeliveryPersons.Domain.Services;
using Delivery.Api.Shared.Persistence;

namespace Delivery.Api.DeliveryPersons.Infrastructure.Services;

public class DeliveryPersonCommandService : IDeliveryPersonCommandService
{
    private readonly IDeliveryPersonRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeliveryPersonCommandService(
        IDeliveryPersonRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DeliveryPerson> Handle(CreateDeliveryPersonCommand command)
    {
        // Validar unicidad de code si viene explícito
        if (!string.IsNullOrWhiteSpace(command.Code))
        {
            var existing = await _repository.FindByCodeAsync(command.Code);
            if (existing is not null)
                throw new ArgumentException("Code must be unique");
        }

        var person = new DeliveryPerson(command.Name, command.Code, command.Phone);

        await _repository.AddAsync(person);
        await _unitOfWork.CompleteAsync();

        return person;
    }

    public async Task<DeliveryPerson?> Handle(UpdateDeliveryPersonCommand command)
    {
        var existing = await _repository.FindByIdAsync(command.Id);
        if (existing is null) return null;

        existing.Update(command.Name, command.Phone);
        _repository.Update(existing);
        await _unitOfWork.CompleteAsync();

        return existing;
    }

    public async Task<bool> Handle(DeleteDeliveryPersonCommand command)
    {
        var existing = await _repository.FindByIdAsync(command.Id);
        if (existing is null) return false;

        _repository.Remove(existing);
        await _unitOfWork.CompleteAsync();

        return true;
    }
}