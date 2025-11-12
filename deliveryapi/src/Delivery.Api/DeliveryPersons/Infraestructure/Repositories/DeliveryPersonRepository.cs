using Delivery.Api.DeliveryPersons.Domain.Models;
using Delivery.Api.DeliveryPersons.Domain.Repositories;
using Delivery.Api.Shared.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Api.DeliveryPersons.Infrastructure.Repositories;

public class DeliveryPersonRepository : IDeliveryPersonRepository
{
    private readonly DeliveryDbContext _context;

    public DeliveryPersonRepository(DeliveryDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DeliveryPerson>> ListAsync(string? code = null)
    {
        IQueryable<DeliveryPerson> query = _context.DeliveryPersons.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(code))
            query = query.Where(x => x.Code == code);

        return await query.ToListAsync();
    }

    public async Task<DeliveryPerson?> FindByIdAsync(string id)
    {
        return await _context.DeliveryPersons
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<DeliveryPerson?> FindByCodeAsync(string code)
    {
        return await _context.DeliveryPersons
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == code);
    }

    public async Task AddAsync(DeliveryPerson entity)
    {
        await _context.DeliveryPersons.AddAsync(entity);
    }

    public void Update(DeliveryPerson entity)
    {
        _context.DeliveryPersons.Update(entity);
    }

    public void Remove(DeliveryPerson entity)
    {
        _context.DeliveryPersons.Remove(entity);
    }
}