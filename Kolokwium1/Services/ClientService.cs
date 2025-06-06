using System.Data;
using Kolokwium1.DTOs;
// using CarRentalAPI.DTOs;
// using CarRentalAPI.Models;
using Microsoft.Data.SqlClient;


namespace Kolokwium1.Services;

public class ClientService : IClientService
{
    private readonly  IConfiguration _configuration;
    private readonly string _connectionString;

    public ClientService(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<ClientDetailsDto?> GetClientWithRentals(int clientId)
    {
        ClientDetailsDto? result = null;
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var clientQuery = "SELECT Id, FirstName, LastName, Address FROM clients WHERE ID = @id";
            using (var command = new SqlCommand(clientQuery, connection))
            {
                command.Parameters.AddWithValue("@id", clientId);
                
                using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new ClientDetailsDto
                            {
                                Id = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                Address = reader.GetString(3),
                                Rentals = new List<RentalDto>()
                            };
                        }
                    }
                
            }

            if (result == null)
            {
                var rentalQuery = @"
                    SELECT c.VIN, col.Name, m.Name, r.DateFrom, r.DateTo, r.TotalPrice
                    FROM car_rentals r
                    JOIN cars c ON r.CarId = c.Id
                    JOIN colors col ON c.ColorId = col.Id
                    JOIN models m ON c.ModelId = m.Id
                    WHERE r.ClientId = @clientid";
            }
        }
    }
    
}