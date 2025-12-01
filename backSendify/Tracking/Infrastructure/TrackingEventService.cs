using backSendify.Shared.Application.Mapping;
using backSendify.Shared.Infrastructure.Persistence;
using backSendify.Tracking.Application.Abstractions;
using backSendify.Tracking.Application.Tracking.Dtos;
using backSendify.Tracking.Application.Tracking.Requests;
using backSendify.Tracking.Domain.Tracking.Entities;
using Microsoft.EntityFrameworkCore;

namespace backSendify.Tracking.Infrastructure;

public class TrackingEventService : ITrackingEventService
{
    private readonly BackSendifyDbContext _context;

    public TrackingEventService(BackSendifyDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TrackingEventDto>> GetAsync(TrackingEventQueryParameters query, CancellationToken cancellationToken)
    {
        IQueryable<TrackingEvent> eventsQuery = _context.TrackingEvents.AsNoTracking();

        if (query.ShipmentId.HasValue)
        {
            eventsQuery = eventsQuery.Where(e => e.ShipmentId == query.ShipmentId);
        }

        var list = await eventsQuery.OrderByDescending(e => e.Timestamp).ToListAsync(cancellationToken);
        return list.Select(e => e.ToDto());
    }

    public async Task<TrackingEventDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _context.TrackingEvents.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        return entity?.ToDto();
    }

    public async Task<TrackingEventDto> CreateAsync(TrackingEventCreateRequest request, CancellationToken cancellationToken)
    {
        await EnsureShipmentExists(request.ShipmentId, cancellationToken);

        var entity = new TrackingEvent
        {
            ShipmentId = request.ShipmentId,
            Status = request.Status,
            Description = request.Description,
            Location = request.Location,
            Timestamp = request.Timestamp ?? DateTime.UtcNow,
            CourierReference = request.CourierReference
        };

        _context.TrackingEvents.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.ToDto();
    }

    public async Task<TrackingEventDto?> UpdateAsync(Guid id, TrackingEventUpdateRequest request, CancellationToken cancellationToken)
    {
        var entity = await _context.TrackingEvents.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (entity == null)
        {
            return null;
        }

        if (request.Status.HasValue)
        {
            entity.Status = request.Status.Value;
        }

        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            entity.Description = request.Description;
        }

        if (!string.IsNullOrWhiteSpace(request.Location))
        {
            entity.Location = request.Location;
        }

        if (request.Timestamp.HasValue)
        {
            entity.Timestamp = request.Timestamp.Value;
        }

        if (request.CourierReference != null)
        {
            entity.CourierReference = request.CourierReference;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return entity.ToDto();
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _context.TrackingEvents.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (entity == null)
        {
            return false;
        }

        _context.TrackingEvents.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    private async Task EnsureShipmentExists(Guid shipmentId, CancellationToken cancellationToken)
    {
        var exists = await _context.Shipments.AnyAsync(s => s.Id == shipmentId, cancellationToken);
        if (!exists)
        {
            throw new ArgumentException("Shipment does not exist", nameof(shipmentId));
        }
    }
}
