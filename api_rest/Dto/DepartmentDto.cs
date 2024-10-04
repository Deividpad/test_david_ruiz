namespace api_rest.Dto;

public class DepartmentDto
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public List<MunicipalityDto> Municipalities { get; set; }
}