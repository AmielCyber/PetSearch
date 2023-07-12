namespace PetSearch.API.Models.PetFinderResponse;

/// <summary>
/// The response the server will get when we call the PetFinder api.
/// Does not include sensitive data such as phone numbers, emails, since we will not send sensitive data to the client.
/// </summary>
/// <param name="Animals">The pet list.</param>
/// <param name="Pagination">The pagination metadata.</param>
public record PetResponse(PetDto[] Animals, Pagination Pagination);