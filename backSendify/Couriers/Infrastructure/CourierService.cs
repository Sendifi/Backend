using backSendify.Couriers.Application.Abstractions;
using backSendify.Couriers.Application.Couriers.Dtos;
using backSendify.Couriers.Application.Couriers.Requests;
using backSendify.Couriers.Domain.Couriers.Entities;
using backSendify.Shared.Application.Common.Dtos;
using backSendify.Shared.Application.Mapping;
using backSendify.Shared.Domain.Common.ValueObjects;
using backSendify.Shared.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backSendify.Couriers.Infrastructure;

public class CourierService : ICourierService
{
    private readonly BackSendifyDbContext _context;

    public CourierService(BackSendifyDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CourierDto>> GetAsync(CancellationToken cancellationToken)
    {
        var couriers = await _context.Couriers.AsNoTracking().OrderBy(c => c.Name).ToListAsync(cancellationToken);
        return couriers.Select<Courier, CourierDto>(c => c.ToDto());
    }

    public async Task<CourierDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var courier = await _context.Couriers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        return courier?.ToDto();
    }

    public async Task<CourierDto> CreateAsync(CourierCreateRequest request, CancellationToken cancellationToken)
    {
        EnsurePricing(request);
        var entity = new Courier
        {
            Name = request.Name,
            Contact = request.Contact?.ToValueObject(),
            Pricing = request.Pricing?.ToValueObject(),
            CostPerKg = request.CostPerKg,
            EstimatedDays = request.EstimatedDays,
            IsActive = request.IsActive,
            Services = request.Services?.Select(s => s).Distinct(StringComparer.OrdinalIgnoreCase).ToList() ?? new List<string>(),
            Coverage = request.Coverage?.Select(s => s).Distinct(StringComparer.OrdinalIgnoreCase).ToList() ?? new List<string>(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Couriers.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.ToDto();
    }

    public async Task<CourierDto?> UpdateAsync(Guid id, CourierUpdateRequest request, CancellationToken cancellationToken)
    {
        var entity = await _context.Couriers.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (entity == null)
        {
            return null;
        }

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            entity.Name = request.Name;
        }

        if (request.Contact is not null)
        {
            entity.Contact = request.Contact.ToValueObject();
        }

        if (request.Pricing is not null)
        {
            entity.Pricing = request.Pricing.ToValueObject();
        }

        if (request.CostPerKg.HasValue)
        {
            entity.CostPerKg = request.CostPerKg;
        }

        if (request.EstimatedDays.HasValue)
        {
            entity.EstimatedDays = request.EstimatedDays;
        }

        if (request.IsActive.HasValue)
        {
            entity.IsActive = request.IsActive.Value;
        }

        if (request.Services is not null)
        {
            entity.Services = request.Services.Select(s => s).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        }

        if (request.Coverage is not null)
        {
            entity.Coverage = request.Coverage.Select(s => s).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        }

        entity.UpdatedAt = DateTime.UtcNow;
        EnsurePricing(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.ToDto();
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _context.Couriers.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (entity == null)
        {
            return false;
        }

        _context.Couriers.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static void EnsurePricing(CourierCreateRequest request)
    {
        if (request.Pricing == null && !request.CostPerKg.HasValue)
        {
            throw new ArgumentException("Either pricing or costPerKg must be provided");
        }
    }

    private static void EnsurePricing(Courier entity)
    {
        if (entity.Pricing == null && !entity.CostPerKg.HasValue)
        {
            throw new ArgumentException("Couriers must define pricing or cost per kg");
        }
    }
}

internal static class CourierMappings
{
    public static ContactInfo ToValueObject(this ContactDto dto) => new(dto.Email, dto.Phone, dto.Website);
    public static PricingInfo ToValueObject(this PricingDto dto) => new(dto.BaseCost, dto.PerKg);
}
