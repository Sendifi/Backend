namespace Delivery.Api.Shared.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly DeliveryDbContext _context;

    public UnitOfWork(DeliveryDbContext context)
    {
        _context = context;
    }

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }
}