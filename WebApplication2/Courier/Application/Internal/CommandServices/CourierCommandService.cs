using WebApplication2.Courier.Domain.Model.Commands;
using WebApplication2.Courier.Domain.Repositories;
using WebApplication2.Courier.Domain.Services;
using WebApplication2.Shared.Domain.Repositories;
// alias
using CourierEntity = WebApplication2.Courier.Domain.Model.Aggregates.Courier;

namespace WebApplication2.Courier.Application.Internal.CommandServices;

public class CourierCommandService(ICourierRepository repo, IUnitOfWork uow) : ICourierCommandService
{
    public async Task<CourierEntity?> Handle(CreateCourierCommand command)
    {
        var exists = await repo.FindByNameAsync(command.Name);
        if (exists is not null) throw new Exception("Courier name must be unique");

        var entity = new CourierEntity(command);
        try
        {
            await repo.AddAsync(entity);
            await uow.CompleteAsync();
        }
        catch { return null; }
        return entity;
    }

    public async Task<CourierEntity?> Update(int id, string? name, decimal? costPerKg, int? estimatedDays, bool? isActive)
    {
        var entity = await repo.FindByIdAsync(id);
        if (entity is null) return null;

        entity.Update(name, costPerKg, estimatedDays, isActive);
        repo.Update(entity);
        await uow.CompleteAsync();
        return entity;
    }

    public async Task<bool> Delete(int id)
    {
        var entity = await repo.FindByIdAsync(id);
        if (entity is null) return false;
        repo.Remove(entity);
        await uow.CompleteAsync();
        return true;
    }
}