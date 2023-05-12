
namespace PetSearchAPI.Models.PetFinderResponse;

public class PetsResponseDto
{
    public PetDto[] Pets { get; set; }
    public Pagination Pagination { get; set; }
}