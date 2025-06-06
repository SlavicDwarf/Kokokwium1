namespace Kolokwium1.Models;

public class car_rentals
{
    public int ID { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int TotalPrice { get; set; }
    public int Discount { get; set; }
}