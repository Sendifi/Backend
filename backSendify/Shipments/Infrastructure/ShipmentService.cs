using System.Text.RegularExpressions;
using backSendify.Shared.Application.Common.Dtos;
using backSendify.Shared.Application.Mapping;
using backSendify.Shared.Domain.Common.ValueObjects;
using backSendify.Shared.Infrastructure.Persistence;
using backSendify.Shipments.Application.Abstractions;
using backSendify.Shipments.Application.Shipments.Dtos;
using backSendify.Shipments.Application.Shipments.Requests;
using backSendify.Shipments.Domain.Shipments.Entities;
using Microsoft.EntityFrameworkCore;

namespace backSendify.Shipments.Infrastructure;

public class ShipmentService : IShipmentService
{
    private static readonly Regex TrackingRegex = new("^SFY\\d{9}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private readonly BackSendifyDbContext _context;
    private readonly Random _random = new();

    public ShipmentService(BackSendifyDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ShipmentDto>> GetShipmentsAsync(ShipmentQueryParameters query, CancellationToken cancellationToken)
    {
        IQueryable<Shipment> shipments = _context.Shipments.AsNoTracking();

        if (query.Status.HasValue)
        {
            shipments = shipments.Where(s => s.Status == query.Status);
        }

        if (!string.IsNullOrWhiteSpace(query.TrackingCode))
        {
            shipments = shipments.Where(s => s.TrackingCode == query.TrackingCode);
        }

        if (query.DeliveryPersonId.HasValue)
        {
            shipments = shipments.Where(s => s.DeliveryPersonId == query.DeliveryPersonId);
        }

        var list = await shipments.OrderByDescending(s => s.CreatedAt).ToListAsync(cancellationToken);
        return list.Select(s => s.ToDto());
    }

    public async Task<ShipmentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _context.Shipments.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        return entity?.ToDto();
    }

    public async Task<ShipmentDto> CreateAsync(ShipmentCreateRequest request, CancellationToken cancellationToken)
    {
        ValidateParticipants(request.Sender, request.Recipient);
        var trackingCode = await GetTrackingCodeAsync(request.TrackingCode, cancellationToken);

        var shipment = new Shipment
        {
            TrackingCode = trackingCode,
            Sender = request.Sender.ToValueObject(),
            Recipient = request.Recipient.ToValueObject(),
            OriginAddress = request.OriginAddress.ToValueObject(),
            DestinationAddress = request.DestinationAddress.ToValueObject(),
            Weight = request.Weight,
            Cost = request.Cost ?? 0,
            Status = request.Status,
            CourierId = request.CourierId,
            DeliveryPersonId = request.DeliveryPersonId,
            EstimatedDelivery = request.EstimatedDelivery,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Shipments.Add(shipment);
        await _context.SaveChangesAsync(cancellationToken);
        return shipment.ToDto();
    }

    public async Task<ShipmentDto?> UpdateAsync(Guid id, ShipmentUpdateRequest request, CancellationToken cancellationToken)
    {
        var shipment = await _context.Shipments.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        if (shipment == null)
        {
            return null;
        }

        if (request.Sender is not null)
        {
            ValidatePerson(request.Sender, nameof(request.Sender));
            shipment.Sender = request.Sender.ToValueObject();
        }

        if (request.Recipient is not null)
        {
            ValidatePerson(request.Recipient, nameof(request.Recipient));
            shipment.Recipient = request.Recipient.ToValueObject();
        }

        if (request.OriginAddress is not null)
        {
            shipment.OriginAddress = request.OriginAddress.ToValueObject();
        }

        if (request.DestinationAddress is not null)
        {
            shipment.DestinationAddress = request.DestinationAddress.ToValueObject();
        }

        if (request.Weight.HasValue)
        {
            if (request.Weight <= 0)
            {
                throw new ArgumentException("Weight must be positive", nameof(request.Weight));
            }
            shipment.Weight = request.Weight.Value;
        }

        if (request.Cost.HasValue)
        {
            shipment.Cost = request.Cost.Value;
        }

        if (request.Status.HasValue)
        {
            shipment.Status = request.Status.Value;
        }

        if (request.CourierId.HasValue)
        {
            shipment.CourierId = request.CourierId;
        }

        if (request.DeliveryPersonId.HasValue)
        {
            shipment.DeliveryPersonId = request.DeliveryPersonId;
        }

        if (request.EstimatedDelivery.HasValue)
        {
            shipment.EstimatedDelivery = request.EstimatedDelivery;
        }

        shipment.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
        return shipment.ToDto();
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var shipment = await _context.Shipments.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        if (shipment == null)
        {
            return false;
        }

        _context.Shipments.Remove(shipment);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    private async Task<string> GetTrackingCodeAsync(string? requestedCode, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(requestedCode))
        {
            if (!TrackingRegex.IsMatch(requestedCode))
            {
                throw new ArgumentException("Tracking code must match SFY######### format", nameof(requestedCode));
            }

            var exists = await _context.Shipments.AnyAsync(s => s.TrackingCode == requestedCode, cancellationToken);
            if (exists)
            {
                throw new ArgumentException("Tracking code already exists", nameof(requestedCode));
            }

            return requestedCode.ToUpperInvariant();
        }

        string generated;
        do
        {
            generated = $"SFY{_random.Next(100_000_000, 999_999_999)}";
        } while (await _context.Shipments.AnyAsync(s => s.TrackingCode == generated, cancellationToken));

        return generated;
    }

    private static void ValidateParticipants(PersonDto sender, PersonDto recipient)
    {
        ValidatePerson(sender, nameof(sender));
        ValidatePerson(recipient, nameof(recipient));
    }

    private static void ValidatePerson(PersonDto dto, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Phone))
        {
            throw new ArgumentException($"{fieldName} requires name, email and phone");
        }
    }
}

internal static class ShipmentMappings
{
    public static PersonInfo ToValueObject(this PersonDto dto) => new(dto.Name, dto.Email, dto.Phone);
    public static Address ToValueObject(this AddressDto dto) => new(dto.Street, dto.City, dto.State, dto.ZipCode, dto.Country);
}
