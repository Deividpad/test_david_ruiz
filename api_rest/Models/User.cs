namespace api_rest.Models;

public class User
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string? Phone { get; set; }
    public int? AddressId { get; set; }
    public Address? Address { get; set; }
}