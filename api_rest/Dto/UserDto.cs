using System.ComponentModel.DataAnnotations;

namespace api_rest.Dto;

public class UserDTO
{
    public int IdUser { get; set; }
    
    [Required(ErrorMessage = "User name is required.")]
    [StringLength(60, ErrorMessage = "User name can't be longer than 60 characters.")]
    public string UserName { get; set; }
    
    [Required(ErrorMessage = "Phone is required.")]
    [StringLength(20, ErrorMessage = "Phone can't be longer than 20 characters.")]
    public string Phone { get; set; }
    
    [Required(ErrorMessage = "Address[] is required.")]
    public AddressDto? AddressDto { get; set; }
}