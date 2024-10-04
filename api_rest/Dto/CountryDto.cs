namespace api_rest.Dto;

public class CountryDto
{
    public int CountryId { get; set; }
    public string CountryName { get; set; }
    public List<DepartmentDto> Departments { get; set; }
}