namespace PetSearch.API.Models.PetFinderResponse;

/// <summary>
/// PetList response from the PetFinder API.
/// Does not include sensitive data such as phone numbers, emails, since we will not send sensitive data to the client.
/// </summary>
/// <param name="Animals">The pet list.</param>
/// <param name="Pagination">The pagination metadata from the APIs JSON response.</param>
public record PaginatedPetList(List<Entities.Pet> Animals, Pagination Pagination);