using PetSearch.API.Entities;
using PetSearch.API.Models;

namespace PetSearch.API.Profiles;

/// <summary>
/// Maps a pet entity to a pet DTO to display for the client.
/// </summary>
public class PetProfile
{
    /// <summary>
    /// Maps a pet IEnumerable entity to pet DTO list.
    /// </summary>
    /// <param name="petList">Pet enumerable entity</param>
    /// <returns>PetDto list</returns>
    public List<PetDto> MapPetListToPetDtoList(IEnumerable<Pet> petList)
    {
        return petList.Select(MapPetToPetDto).ToList();
    }

    /// <summary>
    /// Maps a pet entity to a pet DTO.
    /// </summary>
    /// <param name="pet">Pet entity.</param>
    /// <returns>Pet DTO.</returns>
    public PetDto MapPetToPetDto(Pet pet)
    {
        return new PetDto
        {
            Id = pet.Id,
            Url = pet.Url,
            Type = pet.Type,
            Age = pet.Age,
            Gender = pet.Gender,
            Size = pet.Size,
            Name = pet.Name,
            Description = pet.Description,
            Photos = pet.Photos.ToList(),
            PrimaryPhotoCropped = pet.PrimaryPhotoCropped,
            Status = pet.Status,
            Distance = pet.Distance,
        };
    }
}