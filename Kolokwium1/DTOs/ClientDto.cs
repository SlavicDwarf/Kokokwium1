using System.ComponentModel.DataAnnotations;

namespace Kolokwium1.DTOs;

public class ClientDto
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Address { get; set; }
}