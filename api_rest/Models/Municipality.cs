namespace api_rest.Models;

public class Municipality
{
    public int Id { get; set; }
    public string? MunicipalityName { get; set; }
    public int DepartmentId { get; set; }
    public Department Department { get; set; } //only to navigate between entities
}