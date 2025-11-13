using System.ComponentModel.DataAnnotations;

namespace backSendify.Delivery.Application.DeliveryPersons.Requests;

public class DeliveryPersonCreateRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Code { get; set; }

    [Required]
    [Phone]
    public string Phone { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
