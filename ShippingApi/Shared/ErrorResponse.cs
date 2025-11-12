namespace ShippingApi.Shared;

public class ErrorResponse
{
    public required string Message { get; set; }
    public List<string>? Errors { get; set; }
}
