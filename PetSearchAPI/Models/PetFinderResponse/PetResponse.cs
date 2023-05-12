namespace PetSearchAPI.Models.PetFinderResponse;

public class PetResponse
{
    public PetDto[] Animals { get; set; }
    public Pagination Pagination { get; set; }
}