namespace PetSearchAPI.Models.PetFinderResponse;

/// <summary>
/// PetListDto to return to client containing the list available pets in their area and the pagination object in order
/// to navigate the list.
/// </summary>
public class PetListDto
{
    public required PetDto[] Pets { get; init; }
    public required Pagination Pagination { get; init; }
}