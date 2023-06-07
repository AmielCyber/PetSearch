namespace PetSearchAPI.Models.PetFinderResponse;

/// <summary>
/// Class that will be sent to the client after we map the PetResponse object to this.
/// </summary>
public class PetsResponseDto
{
    public required PetDto[] Pets { get; init; }
    public required Pagination Pagination { get; init; }
}