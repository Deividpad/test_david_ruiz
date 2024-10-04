namespace api_rest.Models;

public class Country
{
    public int Id { get; set; }
    public string? CountryName { get; set; }
    public List<Department>? Departments { get; set; }

}