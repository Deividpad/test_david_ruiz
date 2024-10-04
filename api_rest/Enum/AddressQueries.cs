namespace api_rest.Enum;

public class AddressQueries
{
    public const string SAVE_ADDRESS = "CALL users.insertAddress(@neighborhood, @addressDescription, @municipalityId, null)";

}