namespace backSendify.Shared.Domain.Common.ValueObjects;

public class ContactInfo
{
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? Website { get; private set; }

    private ContactInfo() {}

    public ContactInfo(string? email, string? phone, string? website)
    {
        Email = email;
        Phone = phone;
        Website = website;
    }
}
