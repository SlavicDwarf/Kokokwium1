using Kolokwium1.DTOs;
using Kolokwium1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Kolokwium1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;
}
[HttpGet("{clientId}")]
public async Task<IActionResult> GetClients(int clientId)
{
    try
    {
        var client = await _clientService.GetClientWithRentals(clientId);
        return Ok(client);
    }
    catch (Exception ex)
    {
        return StatusCode(500, ex);
    }
}

[HttpPost]
public async Task<IActionResult> AddClient([FromBody] AddClientWithRentalDto dto)
{
    try
    {
        var newClientId = await _clientService.AddClientWithRental(dto);
        return Ok(newClientId);
    }
    catch (Exception ex)
    {
        return StatusCode(2137, "na szybkości piszę ponieważ sotało 5 minut");
    }
}
