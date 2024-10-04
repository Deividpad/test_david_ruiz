namespace api_rest.Exception;

public class UserNotFoundException : System.Exception
{
    public UserNotFoundException(int userId)
        : base($"User with ID {userId} was not found.")
    {
    }
}