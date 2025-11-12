using ShippingApi.Shipment.Models;

namespace ShippingApi.Shipment.Services;

public class ShipmentService
{
    private readonly List<Models.Shipment> _shipments = new();
    private int _nextId = 1;
    private readonly Random _random = new();

    public ShipmentService()
    {
        SeedData();
    }

    private void SeedData()
    {
        var shipment1 = new Models.Shipment
        {
            Id = "1",
            TrackingCode = "SFY123456789",
            Sender = new Person
            {
                Name = "Juan Pérez",
                Email = "juan@email.com",
                Phone = "+1234567890"
            },
            Recipient = new Person
            {
                Name = "María García",
                Email = "maria@email.com",
                Phone = "+0987654321"
            },
            OriginAddress = new Address
            {
                Street = "Calle 123",
                City = "Bogotá",
                State = "Cundinamarca",
                ZipCode = "110111",
                Country = "Colombia"
            },
            DestinationAddress = new Address
            {
                Street = "Avenida 456",
                City = "Medellín",
                State = "Antioquia",
                ZipCode = "050001",
                Country = "Colombia"
            },
            Weight = 2.5,
            Cost = 15000,
            Status = ShipmentStatus.PENDING,
            CourierId = 1,
            DeliveryPersonId = "e07a",
            EstimatedDelivery = "2024-01-15",
            CreatedAt = "2024-01-10T10:00:00Z",
            UpdatedAt = "2024-01-10T10:00:00Z"
        };

        var shipment2 = new Models.Shipment
        {
            Id = "2",
            TrackingCode = "SFY987654321",
            Sender = new Person
            {
                Name = "Carlos Rodríguez",
                Email = "carlos@email.com",
                Phone = "+1122334455"
            },
            Recipient = new Person
            {
                Name = "Ana Martínez",
                Email = "ana@email.com",
                Phone = "+5566778899"
            },
            OriginAddress = new Address
            {
                Street = "Carrera 10",
                City = "Cali",
                State = "Valle del Cauca",
                ZipCode = "760001",
                Country = "Colombia"
            },
            DestinationAddress = new Address
            {
                Street = "Calle 50",
                City = "Barranquilla",
                State = "Atlántico",
                ZipCode = "080001",
                Country = "Colombia"
            },
            Weight = 1.8,
            Cost = 12000,
            Status = ShipmentStatus.IN_TRANSIT,
            CourierId = 2,
            DeliveryPersonId = "e07b",
            EstimatedDelivery = "2024-01-18",
            CreatedAt = "2024-01-11T14:30:00Z",
            UpdatedAt = "2024-01-12T09:15:00Z"
        };

        _shipments.Add(shipment1);
        _shipments.Add(shipment2);
        _nextId = 3;
    }

    public List<Models.Shipment> GetAllShipments(string? status = null, string? trackingCode = null, string? deliveryPersonId = null)
    {
        var query = _shipments.AsEnumerable();

        if (!string.IsNullOrEmpty(status) && Enum.TryParse<ShipmentStatus>(status, true, out var statusEnum))
        {
            query = query.Where(s => s.Status == statusEnum);
        }

        if (!string.IsNullOrEmpty(trackingCode))
        {
            query = query.Where(s => s.TrackingCode.Equals(trackingCode, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(deliveryPersonId))
        {
            query = query.Where(s => s.DeliveryPersonId == deliveryPersonId);
        }

        return query.ToList();
    }

    public Models.Shipment? GetShipmentById(string id)
    {
        return _shipments.FirstOrDefault(s => s.Id == id);
    }

    public Models.Shipment CreateShipment(CreateShipmentRequest request)
    {
        var now = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
        var trackingCode = GenerateTrackingCode();

        var shipment = new Models.Shipment
        {
            Id = _nextId.ToString(),
            TrackingCode = trackingCode,
            Sender = request.Sender,
            Recipient = request.Recipient,
            OriginAddress = request.OriginAddress,
            DestinationAddress = request.DestinationAddress,
            Weight = request.Weight,
            Cost = 0,
            Status = request.Status,
            CourierId = request.CourierId,
            DeliveryPersonId = request.DeliveryPersonId,
            EstimatedDelivery = null,
            CreatedAt = now,
            UpdatedAt = now
        };

        _shipments.Add(shipment);
        _nextId++;

        return shipment;
    }

    public Models.Shipment? UpdateShipment(string id, UpdateShipmentRequest request)
    {
        var shipment = _shipments.FirstOrDefault(s => s.Id == id);
        if (shipment == null)
        {
            return null;
        }

        if (request.Status.HasValue)
        {
            shipment.Status = request.Status.Value;
        }

        if (request.DeliveryPersonId != null)
        {
            shipment.DeliveryPersonId = request.DeliveryPersonId;
        }

        if (request.EstimatedDelivery != null)
        {
            shipment.EstimatedDelivery = request.EstimatedDelivery;
        }

        if (request.Cost.HasValue)
        {
            shipment.Cost = request.Cost.Value;
        }

        if (request.CourierId.HasValue)
        {
            shipment.CourierId = request.CourierId.Value;
        }

        shipment.UpdatedAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

        return shipment;
    }

    public bool DeleteShipment(string id)
    {
        var shipment = _shipments.FirstOrDefault(s => s.Id == id);
        if (shipment == null)
        {
            return false;
        }

        _shipments.Remove(shipment);
        return true;
    }

    private string GenerateTrackingCode()
    {
        string code;
        do
        {
            var randomNumber = _random.Next(100000000, 999999999);
            code = $"SFY{randomNumber}";
        } while (_shipments.Any(s => s.TrackingCode == code));

        return code;
    }

    public (bool isValid, List<string> errors) ValidateCreateRequest(CreateShipmentRequest request)
    {
        var errors = new List<string>();

        if (request.Weight <= 0)
        {
            errors.Add("weight must be a positive number");
        }

        if (string.IsNullOrWhiteSpace(request.Sender?.Name))
        {
            errors.Add("sender.name is required");
        }

        if (string.IsNullOrWhiteSpace(request.Sender?.Email))
        {
            errors.Add("sender.email is required");
        }

        if (string.IsNullOrWhiteSpace(request.Sender?.Phone))
        {
            errors.Add("sender.phone is required");
        }

        if (string.IsNullOrWhiteSpace(request.Recipient?.Name))
        {
            errors.Add("recipient.name is required");
        }

        if (string.IsNullOrWhiteSpace(request.Recipient?.Email))
        {
            errors.Add("recipient.email is required");
        }

        if (string.IsNullOrWhiteSpace(request.Recipient?.Phone))
        {
            errors.Add("recipient.phone is required");
        }

        if (string.IsNullOrWhiteSpace(request.OriginAddress?.Street))
        {
            errors.Add("originAddress.street is required");
        }

        if (string.IsNullOrWhiteSpace(request.OriginAddress?.City))
        {
            errors.Add("originAddress.city is required");
        }

        if (string.IsNullOrWhiteSpace(request.DestinationAddress?.Street))
        {
            errors.Add("destinationAddress.street is required");
        }

        if (string.IsNullOrWhiteSpace(request.DestinationAddress?.City))
        {
            errors.Add("destinationAddress.city is required");
        }

        return (errors.Count == 0, errors);
    }
}
