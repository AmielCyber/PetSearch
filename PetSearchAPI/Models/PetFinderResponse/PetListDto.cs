namespace PetSearchAPI.Models.PetFinderResponse;

public class PetListDto
{
    public PetDto[] Pets { get; set; }
    public Pagination Pagination { get; set; }
}