namespace PetSearchAPI.Models.PetFinderResponse;

/// <summary>
/// PetListDto to return to client containing the list available pets in their area and the pagination object in order
/// to navigate the list.
/// </summary>
public class PetListDto
{
    public PetDto[] Pets { get; set; }
    public Pagination Pagination { get; set; }
}