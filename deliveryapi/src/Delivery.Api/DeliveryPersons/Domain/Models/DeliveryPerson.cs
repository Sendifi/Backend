using System.Text.RegularExpressions;

namespace Delivery.Api.DeliveryPersons.Domain.Models;

public class DeliveryPerson
{
    public DeliveryPerson(string name, string? code, string phone)
    {
        Id = Guid.NewGuid().ToString("N")[..4]; // ejemplo: "e07a"
        Name = name.Trim();
        Code = string.IsNullOrWhiteSpace(code) ? GenerateCode() : code.Trim();
        Phone = phone.Trim();
        AssignedShipments = new List<string>();
        IsActive = true;

        Validate();
    }

    // Para actualizar
    public void Update(string? name, string? phone)
    {
        if (!string.IsNullOrWhiteSpace(name))
            Name = name.Trim();

        if (!string.IsNullOrWhiteSpace(phone))
            Phone = phone.Trim();

        Validate();
    }

    public string Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Code { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public IList<string> AssignedShipments { get; private set; } = new List<string>();
    public bool IsActive { get; private set; } = true;

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentException("Name is required");

        var regex = new Regex(@"^\+?\d{7,15}$");
        if (!regex.IsMatch(Phone))
            throw new ArgumentException("Phone is not valid");
    }

    private static string GenerateCode()
    {
        var random = new Random();
        return $"DEL{random.Next(0, 999):D3}"; // DEL000 - DEL999
    }
}