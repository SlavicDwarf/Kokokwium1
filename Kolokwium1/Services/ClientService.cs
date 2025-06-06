using System.Data;
using Kolokwium1.DTOs;
using Kolokwium1.Models;
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
                using (var command = new SqlCommand(rentalQuery, connection))
                {
                    command.Parameters.AddWithValue("@clientid", clientId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Rentals.Add(new RentalDto
                            {
                                Vin = reader.GetString(0),
                                Color = reader.GetString(1),
                                Model = reader.GetString(2),
                                DateFrom = reader.GetDateTime(3),
                                DateTo = reader.GetDateTime(4),
                                TotalPrice = reader.GetInt32(5)
                            });
                        }
                    }
                }
            }
        }
        
        return result;
    }

    public async Task<int> AddClientWithRentals(AddClientWithRentalDto dto)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var CheckCarQuery = "Select PricePerDay FROM cars Where ID = @CarId";
            int pricePerDay = 0;
            using (var command = new SqlCommand(CheckCarQuery, connection))
            {
                command.Parameters.AddWithValue("@CarId", dto.CarId);
                var result = await command.ExecuteScalarAsync();
                if (result != null)
                {
                    throw new Exception($"Car with ID {dto.CarId} doesn't exists");
                }
                pricePerDay = (int)result;
                
            }
            var days = (dto.DateTo - dto.DateFrom).TotalDays;
            var totalPrice = days*pricePerDay;
            var insertClientQuery = @"
                INSERT INTO clients(FirstName,LastName,Address)
                VALUES (@FirstName,@LastName,@Address);
                SELECT CAST(SCOPE_IDENTITY() as int)";
            int newClientId;
            using (var command = new SqlCommand(insertClientQuery, connection))
            {
                command.Parameters.AddWithValue("@firstName",dto.Client.FirstName);
                command.Parameters.AddWithValue("@lastName", dto.Client.LastName);
                command.Parameters.AddWithValue("@address", dto.Client.Address);
                newClientId = (int)await command.ExecuteScalarAsync();
            }
            var insertRentalQuery = @"
                INSERT INTO carrentals(ClientId,CarId,DateFrom,DateTo,TotalPrice,Discount)
                VALUES (@clientId,@carId,@datefrom,@dateto,@totalprice,NULL)";
            using (var command = new SqlCommand(insertRentalQuery, connection))
            {
                command.Parameters.AddWithValue("@clientId", newClientId);
                command.Parameters.AddWithValue("@carId", dto.CarId);
                command.Parameters.AddWithValue("@datefrom", dto.DateFrom);
                command.Parameters.AddWithValue("@dateto", dto.DateTo);
                command.Parameters.AddWithValue("@totalprice", totalPrice);
                await command.ExecuteNonQueryAsync();
            }
            return newClientId;
        }
    }
    
    
}