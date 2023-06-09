namespace PetSearchAPI.Models.PetFinderResponse;

/// <summary>
/// The response the server will get when we call the PetFinder api.
/// Does not include sensitive data such as phone numbers, emails, since we will not send sensitive data to the client.
/// </summary>
public class PetResponse
{
    public required PetDto[] Animals { get; init; }
    public required Pagination Pagination { get; init; }
}