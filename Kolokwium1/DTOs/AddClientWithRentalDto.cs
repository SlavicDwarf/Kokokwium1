using System.ComponentModel.DataAnnotations;

namespace Kolokwium1.DTOs;

public class AddClientWithRentalDto
{
    [Required]
    public ClientDto Client { get; set; }
    [Required]
    public int CarId { get; set; }
    [Required]
    public DateTime DateFrom { get; set; }
    [Required]
    public DateTime DateTo { get; set; }
}