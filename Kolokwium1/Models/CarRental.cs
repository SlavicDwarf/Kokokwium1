namespace Kolokwium1.Models;

public class CarRental
{
    public int Id { get; set; }
    public string ClientId { get; set; }
    public string CarId { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int TotalPrice { get; set; }
    public int Discount { get; set; }
}