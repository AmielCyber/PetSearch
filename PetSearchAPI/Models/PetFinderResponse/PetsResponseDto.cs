namespace PetSearchAPI.Models.PetFinderResponse;

/// <summary>
/// Class that will be sent to the client after we map the PetResponse object to this.
/// </summary>
public class PetsResponseDto
{
    public PetDto[] Pets { get; set; }
    public Pagination Pagination { get; set; }
}