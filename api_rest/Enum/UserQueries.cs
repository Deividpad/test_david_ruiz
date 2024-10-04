using System.ComponentModel;

namespace api_rest.Enum;

public static class UserQueries
{
    public const string GET_ALL_USERS = "SELECT * FROM users.getusers();";
    public const string GET_USER_BY_ID = "SELECT * FROM users.getUserById(@userId);";
    public const string SAVE_USER = "CALL users.insertUser(@userName, @phone, @addressId, null);";
    public const string DELETE_USER_AND_ADDRESS = "CALL users.deleteUserAndAddress(@idUser, null);";
    public const string UPDATE_USER_AND_ADDRESS = "CALL users.updateUserAndAddress(@idUser, @userName, @phone, " +
                                                  "@neighborhood, @addressDescription, @municipalityId);";
    
}