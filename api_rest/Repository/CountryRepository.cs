using api_rest.Db;
using api_rest.Enum;
using api_rest.Models;
using Microsoft.OpenApi.Any;
using Npgsql;

namespace api_rest.Repository;

public class CountryRepository
{
    private readonly string? _connectionString;

    public CountryRepository(ConnectionStringProvider connectionStringProvider)
    {
        _connectionString = connectionStringProvider.GetConnectionString();
    }

    public async Task<List<Country>> GetCountryDataAsync()

    {
        var countries = new Dictionary<int, Country>();

        var connection = new NpgsqlConnection(_connectionString);
        
        await connection.OpenAsync();

        try
        {
            var command = new NpgsqlCommand(CountryQueries.GET_COUNTRIES, connection);
            var reader = await command.ExecuteReaderAsync();

            try
            {
                while (await reader.ReadAsync())
                {

                    // Extract country data
                    var countryId = reader.GetInt32(reader.GetOrdinal("id_country"));
                    if (!countries.ContainsKey(countryId))
                    {
                        countries[countryId] = new Country
                        {
                            Id = countryId,
                            CountryName = reader.GetString(reader.GetOrdinal("country_name")),
                            Departments = new List<Department>()
                        };
                    }
                    var country = countries[countryId];

                    // Extract department data
                    var departmentId = reader.GetInt32(reader.GetOrdinal("id_department"));
                    var department = country.Departments.FirstOrDefault(d => d.Id == departmentId);
                    if (department == null)
                    {
                        department = new Department
                        {
                            Id = departmentId,
                            DepartmentName = reader.GetString(reader.GetOrdinal("department_name")),
                            CountryId = countryId,
                            Municipalities = new List<Municipality>()
                        };
                        country.Departments.Add(department);
                    }

                    // Extract municipality data
                    var municipality = new Municipality
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id_municipality")),
                        MunicipalityName = reader.GetString(reader.GetOrdinal("municipality_name")),
                        DepartmentId = departmentId
                    };

                    department.Municipalities.Add(municipality);
                }
            }
            finally
            {
                await reader.CloseAsync();
            }
        }
        catch (System.Exception ex)
        {
            // Handle the exception (log it, rethrow it, etc.)
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw new ApplicationException("An error occurred while listing the users.", ex);
        }
        finally
        {
            // Ensure the connection is closed and disposed
            await connection.CloseAsync();
            await connection.DisposeAsync();
        }
        return countries.Values.ToList();
    }
}