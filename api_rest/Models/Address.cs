using System.ComponentModel.DataAnnotations;

namespace api_rest.Models;

public class Address
{
    public int Id { get; set; }
    public string Neighborhood { get; set; }
    public string AddressDescription { get; set; }
    public int MunicipalityId { get; set; }
    public Municipality? Municipality { get; set; } //only to navigate between entities

    
}