namespace backSendify.Shared.Application.Common.Dtos;

public record AddressDto(
    string Street,
    string City,
    string State,
    string ZipCode,
    string Country);
