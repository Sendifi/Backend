namespace backSendify.Shared.Domain.Common.ValueObjects;

public class PersonInfo
{
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Phone { get; private set; } = null!;

    private PersonInfo() {}

    public PersonInfo(string name, string email, string phone)
    {
        Name = name;
        Email = email;
        Phone = phone;
    }
}
