using Kolokwium1.DTOs;

public interface IClientService
{
    Task<ClientDetailsDto?> GetClientDetails(int clientId);
    Task<int> AddClientWithRental(AddClientWithRentalDto dto);
}
