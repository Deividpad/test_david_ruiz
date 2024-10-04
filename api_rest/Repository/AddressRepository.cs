using api_rest.Db;
using api_rest.Enum;
using api_rest.Models;
using Npgsql;

namespace api_rest.Repository;

public class AddressRepository
{
    private readonly string? _connectionString;

    public AddressRepository(ConnectionStringProvider connectionStringProvider)
    {
        _connectionString = connectionStringProvider.GetConnectionString();
    }

    public async Task<Address> SaveAddress(Address address)
    {
        var addressDb = new Address();

        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        try
        {
            var command = new NpgsqlCommand(AddressQueries.SAVE_ADDRESS, connection);
        
            command.Parameters.AddWithValue("neighborhood", address.Neighborhood);
            command.Parameters.AddWithValue("addressDescription", address.AddressDescription);
            command.Parameters.AddWithValue("municipalityId", address.MunicipalityId);

            var newAddressId = (int)await command.ExecuteScalarAsync();
        
            addressDb.Id = newAddressId;
            addressDb.Neighborhood = address.Neighborhood;
            addressDb.AddressDescription = address.AddressDescription;
            addressDb.MunicipalityId = address.MunicipalityId;
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw new ApplicationException("An error occurred while saving the user.", ex);
        }
        finally
        {
            await connection.CloseAsync();
            await connection.DisposeAsync();
        }
        return addressDb;
    }
}