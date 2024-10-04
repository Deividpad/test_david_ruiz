namespace api_rest.Models;

public class Department
{
    public int Id { get; set; }
    public string DepartmentName { get; set; }
    public int CountryId { get; set; }
    public Country? Country { get; set; } //only to navigate between entities
    public List<Municipality>? Municipalities { get; set; }

}