using api_rest.Dto;
using api_rest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;

namespace api_rest.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CountryController : Controller
{
    private readonly CountryService _countryService;

    public CountryController(CountryService countryService)
    {
        _countryService = countryService;
    }

    [HttpGet("get-countries")]
    public async Task<ActionResult<List<CountryDto>>> Get()
    {
        var users = await _countryService.GetCountries();
        return Ok(users);
    }
}