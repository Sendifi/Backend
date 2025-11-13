using backSendify.Delivery.Application.Abstractions;
using backSendify.Delivery.Application.DeliveryPersons.Dtos;
using backSendify.Delivery.Application.DeliveryPersons.Requests;
using backSendify.Delivery.Domain.DeliveryPersons.Entities;
using backSendify.Shared.Application.Mapping;
using backSendify.Shared.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backSendify.Delivery.Infrastructure;

public class DeliveryPersonService : IDeliveryPersonService
{
    private readonly BackSendifyDbContext _context;
    private readonly Random _random = new();

    public DeliveryPersonService(BackSendifyDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DeliveryPersonDto>> GetAsync(DeliveryPersonQueryParameters query, CancellationToken cancellationToken)
    {
        IQueryable<DeliveryPerson> deliveryPeople = _context.DeliveryPersons.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Code))
        {
            deliveryPeople = deliveryPeople.Where(d => d.Code == query.Code);
        }

        if (query.IsActive.HasValue)
        {
            deliveryPeople = deliveryPeople.Where(d => d.IsActive == query.IsActive);
        }

        var list = await deliveryPeople.OrderBy(d => d.Name).ToListAsync(cancellationToken);
        return list.Select(d => d.ToDto());
    }

    public async Task<DeliveryPersonDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _context.DeliveryPersons.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        return entity?.ToDto();
    }

    public async Task<DeliveryPersonDto> CreateAsync(DeliveryPersonCreateRequest request, CancellationToken cancellationToken)
    {
        var code = await GetCodeAsync(request.Code, cancellationToken);
        var entity = new DeliveryPerson
        {
            Name = request.Name,
            Phone = request.Phone,
            Code = code,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.DeliveryPersons.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.ToDto();
    }

    public async Task<DeliveryPersonDto?> UpdateAsync(Guid id, DeliveryPersonUpdateRequest request, CancellationToken cancellationToken)
    {
        var entity = await _context.DeliveryPersons.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        if (entity == null)
        {
            return null;
        }

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            entity.Name = request.Name;
        }

        if (!string.IsNullOrWhiteSpace(request.Phone))
        {
            entity.Phone = request.Phone;
        }

        if (request.IsActive.HasValue)
        {
            entity.IsActive = request.IsActive.Value;
        }

        if (request.AssignedShipments is not null)
        {
            entity.AssignedShipments = request.AssignedShipments.Select(id => id).Distinct().ToList();
        }

        entity.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
        return entity.ToDto();
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _context.DeliveryPersons.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        if (entity == null)
        {
            return false;
        }

        _context.DeliveryPersons.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    private async Task<string> GetCodeAsync(string? requested, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(requested))
        {
            var exists = await _context.DeliveryPersons.AnyAsync(d => d.Code == requested, cancellationToken);
            if (exists)
            {
                throw new ArgumentException("Delivery person code already exists", nameof(requested));
            }

            return requested.ToUpperInvariant();
        }

        string code;
        do
        {
            code = $"DEL{_random.Next(100, 999):000}";
        } while (await _context.DeliveryPersons.AnyAsync(d => d.Code == code, cancellationToken));

        return code;
    }
}
