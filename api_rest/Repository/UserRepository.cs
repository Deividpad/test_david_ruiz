using api_rest.Db;
using api_rest.Enum;
using api_rest.Exception;
using api_rest.Models;
using Npgsql;

namespace api_rest.Repository;

public class UserRepository
{
    private readonly string? _connectionString;

    public UserRepository(ConnectionStringProvider connectionStringProvider)
    {
        _connectionString = connectionStringProvider.GetConnectionString();
    }

    public async Task<List<User>> GetUsers()
    {
        var users = new List<User>();
        var connection = new NpgsqlConnection(_connectionString);
        
        await connection.OpenAsync();

        try
        {
            var command = new NpgsqlCommand(UserQueries.GET_ALL_USERS, connection);
            var reader = await command.ExecuteReaderAsync();

            try
            {
                while (await reader.ReadAsync())
                {
                    var country = new Country
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("country_id")),
                        CountryName = reader.GetString(reader.GetOrdinal("country_name"))
                    };
                    
                    var department = new Department
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("department_id")),
                        DepartmentName = reader.GetString(reader.GetOrdinal("department_name")),
                        Country = country
                    };
                    
                    var municipality = new Municipality
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("municipality_id")),
                        MunicipalityName = reader.GetString(reader.GetOrdinal("municipality_name")),
                        Department = department
                    };
                    
                    var address = new Address
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("address_id")),
                        Neighborhood = reader.GetString(reader.GetOrdinal("neighborhood")),
                        AddressDescription = reader.GetString(reader.GetOrdinal("address_description")),
                        Municipality = municipality
                    };
                    
                    var user = new User
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("user_id")),
                        UserName = reader.GetString(reader.GetOrdinal("user_name")),
                        Phone = reader.GetString(reader.GetOrdinal("phone")),
                        Address = address
                    };
                    
                    users.Add(user);
                }
            }
            finally
            {
                // Ensure the reader is closed
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
        return users;
    }
    
    public async Task<User> GetUserById(int userId)
    {
        await using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            await using (var command = new NpgsqlCommand(UserQueries.GET_USER_BY_ID, connection))
            {
                command.Parameters.AddWithValue("userId", userId);

                await using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var country = new Country
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("country_id")),
                            CountryName = reader.GetString(reader.GetOrdinal("country_name"))
                        };
                    
                        var department = new Department
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("department_id")),
                            DepartmentName = reader.GetString(reader.GetOrdinal("department_name")),
                            Country = country
                        };
                    
                        var municipality = new Municipality
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("municipality_id")),
                            MunicipalityName = reader.GetString(reader.GetOrdinal("municipality_name")),
                            Department = department
                        };
                    
                        var address = new Address
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("address_id")),
                            Neighborhood = reader.GetString(reader.GetOrdinal("neighborhood")),
                            AddressDescription = reader.GetString(reader.GetOrdinal("address_description")),
                            Municipality = municipality
                        };
                    
                        return new User
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("user_id")),
                            UserName = reader.GetString(reader.GetOrdinal("user_name")),
                            Phone = reader.GetString(reader.GetOrdinal("phone")),
                            Address = address
                        };
                    }
                    throw new UserNotFoundException(userId);
                    //throw new System.Exception($"User with ID {userId} was not found.");
                }
            }
        }
    }

    public async Task<User> SaveUser(User user)
    {
        var userDb = new User();

        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        try
        {
            var command = new NpgsqlCommand(UserQueries.SAVE_USER, connection);
        
            command.Parameters.AddWithValue("userName", user.UserName);
            command.Parameters.AddWithValue("phone", user.Phone);
            command.Parameters.AddWithValue("addressId", user.AddressId);

            var newUserId = (int)await command.ExecuteScalarAsync();
        
            userDb.Id = newUserId;
            userDb.UserName = user.UserName;
            userDb.Phone = user.Phone;
            userDb.AddressId = user.AddressId;
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
        return userDb;
    }
    
    public async Task UpdateUserAndAddress(User user, Address address)
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        try
        {
            var command = new NpgsqlCommand(UserQueries.UPDATE_USER_AND_ADDRESS, connection);
        
            command.Parameters.AddWithValue("idUser", user.Id);
            command.Parameters.AddWithValue("userName", user.UserName);
            command.Parameters.AddWithValue("phone", user.Phone);
            command.Parameters.AddWithValue("neighborhood", address.Neighborhood);
            command.Parameters.AddWithValue("addressDescription", address.AddressDescription);
            command.Parameters.AddWithValue("municipalityId", address.MunicipalityId);

            await command.ExecuteScalarAsync();
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw new System.Exception("An error occurred while updating the user.", ex);
        }
        finally
        {
            await connection.CloseAsync();
            await connection.DisposeAsync();
        }
    }
    
    public async Task<bool> DeleteUserAndAddress(int userId)
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        try
        {
            var command = new NpgsqlCommand(UserQueries.DELETE_USER_AND_ADDRESS, connection);
        
            command.Parameters.AddWithValue("idUser", userId);
            
            var affectedRows = (int)await command.ExecuteScalarAsync();
            return affectedRows > 0;
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
    }
}