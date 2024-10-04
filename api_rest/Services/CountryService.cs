using api_rest.Dto;
using api_rest.Repository;
using Microsoft.OpenApi.Any;

namespace api_rest.Services;

public class CountryService
{
    private readonly CountryRepository _countryRepository;

    public CountryService(CountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }

    public async Task<List<CountryDto>> GetCountries()
    {
        
        var countries = await _countryRepository.GetCountryDataAsync();
        return countries.Select(c => new CountryDto
        {
            CountryId = c.Id,
            CountryName = c.CountryName,
            Departments = c.Departments.Select(d => new DepartmentDto
            {
                DepartmentId = d.Id,
                DepartmentName = d.DepartmentName,
                Municipalities = d.Municipalities.Select(m => new MunicipalityDto
                {
                    MunicipalityId = m.Id,
                    MunicipalityName = m.MunicipalityName
                }).ToList()
            }).ToList()
        }).ToList();
    }
}