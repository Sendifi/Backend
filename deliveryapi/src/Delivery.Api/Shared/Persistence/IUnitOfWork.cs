namespace Delivery.Api.Shared.Persistence;

public interface IUnitOfWork
{
    Task CompleteAsync();
}