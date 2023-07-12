using System.ComponentModel.DataAnnotations;

namespace PetSearch.API.Models.PetFinderResponse;

/// <summary>
/// Pet Response Dto will be sent to the client after mapping PetResponse object.
/// </summary>
/// <param name="Pets">Pet list.</param>
/// <param name="Pagination">Pagination metadata.</param>
public record PetsResponseDto([property: Required] PetDto[] Pets, [property: Required] Pagination Pagination);