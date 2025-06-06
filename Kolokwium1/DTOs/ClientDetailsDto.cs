using System.ComponentModel.DataAnnotations;

namespace Kolokwium1.DTOs;

public class ClientDetailsDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Address { get; set; }
    public List<RentalDto> Rentals { get; set; }
}