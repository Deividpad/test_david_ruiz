using System.ComponentModel.DataAnnotations;

namespace api_rest.Dto;

public class AddressDto
{
    public int? IdAddress { get; set; }
    
    [Required(ErrorMessage = "Neighborhood is required.")]
    [StringLength(50, ErrorMessage = "Neighborhood can't be longer than 50 characters.")]
    public string? Neighborhood { get; set; }
    
    [Required(ErrorMessage = "AddressDescription is required.")]
    [StringLength(150, ErrorMessage = "AddressDescription can't be longer than 150 characters.")]
    public string? AddressDescription { get; set; }
    
    [Required(ErrorMessage = "MunicipalityId is required.")]
    public int? MunicipalityId { get; set; }
    public string? MunicipalityName { get; set; }
    public int? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public int? CountryId { get; set; }
    public string? CountryName { get; set; }
}