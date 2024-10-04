using api_rest.Dto;
using api_rest.Exception;
using api_rest.Models;
using api_rest.Repository;
using Microsoft.AspNetCore.Http.HttpResults;

namespace api_rest.Services;

public class UserService
{
    
    private readonly UserRepository _userRepository;
    private readonly AddressRepository _addressRepository;

    public UserService(UserRepository userRepository, AddressRepository addressRepository)
    {
        _userRepository = userRepository;
        _addressRepository = addressRepository;
    }

    public async Task<List<UserDTO>> GetUsers()
    {
        var users = await _userRepository.GetUsers();
    
        var userDtos = users.Select(user => new UserDTO()
        {
            IdUser = user.Id,
            UserName = user.UserName,
            Phone = user.Phone,
            AddressDto = new AddressDto()
            {
                IdAddress = user.Address.Id,
                Neighborhood = user.Address.Neighborhood,
                AddressDescription = user.Address.AddressDescription,
                MunicipalityId = user.Address.Municipality.Id,
                MunicipalityName = user.Address.Municipality.MunicipalityName,
                DepartmentId = user.Address.Municipality.Department.Id,
                DepartmentName = user.Address.Municipality.Department.DepartmentName,
                CountryId = user.Address.Municipality.Department.Country.Id,
                CountryName = user.Address.Municipality.Department.Country.CountryName,
            }
        }).ToList();

        return userDtos;
    }
    
    public async Task<UserDTO> GetUserById(int userId)
    {
        var userDb = await _userRepository.GetUserById(userId);

        AddressDto addressDto = new AddressDto()
        {
            IdAddress = userDb.Address.Id,
            Neighborhood = userDb.Address.Neighborhood,
            AddressDescription = userDb.Address.AddressDescription,
            MunicipalityId = userDb.Address.Municipality.Id,
            MunicipalityName = userDb.Address.Municipality.MunicipalityName,
            DepartmentId = userDb.Address.Municipality.Department.Id,
            DepartmentName = userDb.Address.Municipality.Department.DepartmentName,
            CountryId = userDb.Address.Municipality.Department.Country.Id,
            CountryName = userDb.Address.Municipality.Department.Country.CountryName,
        };
        
        return new UserDTO()
        {
            IdUser = userDb.Id,
            UserName = userDb.UserName,
            Phone = userDb.Phone,
            AddressDto = addressDto
        };
    }
    
    public async Task<UserDTO> SaveUser(UserDTO userDto)
    {
        //calling address repository saved
        Address address = new Address();
        address.Neighborhood = userDto.AddressDto.Neighborhood;
        address.AddressDescription = userDto.AddressDto.AddressDescription;
        address.MunicipalityId = userDto.AddressDto.MunicipalityId.GetValueOrDefault();
        var addressDb = await _addressRepository.SaveAddress(address);
        
        //calling user repository saved with address_id returned before
        User user = new User();
        user.Id = userDto.IdUser;
        user.UserName = userDto.UserName;
        user.Phone = userDto.Phone;
        user.AddressId = addressDb.Id;
        var userDb = await _userRepository.SaveUser(user);
        
        return await GetUserById(userDb.Id);
    }
    
    public async Task<UserDTO> UpdateUser(int userId, UserDTO userDto)
    {
        try {
            
            Address address = new Address();
            address.Neighborhood = userDto.AddressDto.Neighborhood;
            address.AddressDescription = userDto.AddressDto.AddressDescription;
            address.MunicipalityId = userDto.AddressDto.MunicipalityId.GetValueOrDefault();

            User user = new User();
            user.Id = userId;
            user.UserName = userDto.UserName;
            user.Phone = userDto.Phone;
            await _userRepository.UpdateUserAndAddress(user, address);
            
            return await GetUserById(user.Id);
        }
        catch (UserNotFoundException  ex)
        {
            throw new UserNotFoundException(userId);
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw new System.Exception("An error occurred while updating the user.", ex);
        }
    }
    
    public async Task<bool> DeleteUser(int userId)
    {
        return await _userRepository.DeleteUserAndAddress(userId);
    }
}