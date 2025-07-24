using Backend.Models;
using Backend.Repositories;
using Backend.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationController : ControllerBase
{
    private readonly LocationRepository _locationRepository;

    public LocationController(LocationRepository LocationRepository)
    {
        _locationRepository = LocationRepository;
    }

    // Get: api/location
    [HttpGet]
    public async Task<ActionResult<List<Location>>> GetAll()
    {
        var locations = await _locationRepository.GetAllLocationAsync();
        return Ok(locations);
    }
}

