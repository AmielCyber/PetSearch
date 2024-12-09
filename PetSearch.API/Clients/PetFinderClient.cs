using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using PetSearch.API.Problems;
using PetSearch.API.Exceptions;
using PetSearch.API.Models;
using PetSearch.API.Models.PetFinderResponse;
using PetSearch.API.Profiles;
using PetSearch.API.Helpers;
using PetSearch.Data.Entities;
using PetSearch.Data.Services;

namespace PetSearch.API.Clients;

/// <summary>
/// PetFinderClient implementation to handle requests from the PetFinderAPI.
/// Returns the appropriate response to our client app.
/// </summary>
public class PetFinderClient : IPetFinderClient
{
    private const string PetSearchEndpoint = "animals";
    private readonly HttpClient _client;
    private readonly ITokenService _tokenService;
    private readonly PetProfile _petProfile;
    private readonly PaginationMetaDataProfile _paginationMetaDataProfile;
    private readonly IExpectedProblems _expectedProblems;

    /// <summary>
    /// Set up dependency injection. 
    /// </summary>
    /// <param name="client">Have access to the global HttpClient object to make external API requests.</param>
    /// <param name="tokenService">The token used to make calls the PetFinder Api.</param>
    /// <param name="petProfile">Pet profile mapper to map pet entities to pet DTOs.</param>
    /// <param name="paginationMetaDataProfile">PaginationMetaData profile mapper
    /// to map Pagination to PaginationMetaData</param>
    /// <param name="expectedProblems">PetFinder expected problems to return if response fails.</param>
    public PetFinderClient(HttpClient client, ITokenService tokenService, PetProfile petProfile,
        PaginationMetaDataProfile paginationMetaDataProfile,
        [FromKeyedServices("petFinderProblems")] IExpectedProblems expectedProblems)
    {
        _client = client;
        _tokenService = tokenService;
        _petProfile = petProfile;
        _paginationMetaDataProfile = paginationMetaDataProfile;
        _expectedProblems = expectedProblems;
    }

    /// <summary>
    /// Gets a list of available pets for adoption and the pagination object.
    /// </summary>
    /// <param name="petsParams">Search Query object with its properties as the accepted parameters to use
    /// in the search query.</param>
    /// <returns>A PageList PetDto with a list of available pets and the pagination object if the object
    /// was successfully fetched, else returns a custom error type.</returns>
    /// <exception cref="ForbiddenAccessException">Throws when we get forbidden response from PetFinder Api.</exception>
    public async Task<Results<Ok<PagedList<PetDto>>, ProblemHttpResult>> GetPetsAsync(PetsParams petsParams)
    {
        await SetAuthenticationHeaders();
        using HttpResponseMessage response = await _client.GetAsync(GetPathWithQueryString(petsParams));
        if (!response.IsSuccessStatusCode)
            return ClientProblems.GetProblemHttpResult(response.StatusCode, _expectedProblems);

        PaginatedPetList? petResponse = await response.Content.ReadFromJsonAsync<PaginatedPetList>();
        if (petResponse is null)
            return ClientProblems.GetProblemHttpResult(HttpStatusCode.InternalServerError, _expectedProblems);

        var petList = _petProfile.MapPetListToPetDtoList(petResponse.Animals);
        var paginationMetaData = _paginationMetaDataProfile.MapPaginationToPaginationMetaData(petResponse.Pagination);

        return TypedResults.Ok(new PagedList<PetDto>(petList, paginationMetaData));
    }

    /// <summary>
    /// Gets a single pet object by their id.
    /// </summary>
    /// <param name="id">The id of the pet.</param>
    /// <returns>A PetDto if the request was successful, else returns an ErrorOr Error if request return without a 200
    /// response code.</returns>
    /// <exception cref="ForbiddenAccessException">Throws when we get forbidden response from PetFinder Api.</exception>
    public async Task<Results<Ok<PetDto>, ProblemHttpResult>> GetSinglePetAsync(int id)
    {
        await SetAuthenticationHeaders();

        using HttpResponseMessage response = await _client.GetAsync($"{PetSearchEndpoint}/{id}");
        if (!response.IsSuccessStatusCode)
            return ClientProblems.GetProblemHttpResult(response.StatusCode, _expectedProblems);

        SinglePet? petResponse = await response.Content.ReadFromJsonAsync<SinglePet>();
        if (petResponse is null)
            return ClientProblems.GetProblemHttpResult(HttpStatusCode.InternalServerError, _expectedProblems);

        // Only return PetDto.
        return TypedResults.Ok(_petProfile.MapPetToPetDto(petResponse.Pet));
    }

    /// <summary>
    /// Transforms our PetsParams object into a query string for PetFinderAPI.
    /// </summary>
    /// <param name="petsParams">Pets Parameter object containing search queries as properties.</param>
    /// <returns>A query string to use for the PetFinder API</returns>
    private static string GetPathWithQueryString(PetsParams petsParams)
    {
        var query = new QueryBuilder
        {
            // Required.
            { "type", petsParams.Type },
            // Required.
            { "location", petsParams.Location },
            // Default page=1.
            { "page", petsParams.Page.ToString() },
            // Default distance=50.
            { "distance", petsParams.Distance.ToString() },
            // Default sort=recent.
            { "sort", petsParams.Sort }
        };
        return $"{PetSearchEndpoint}{query}";
    }

    /// <summary>
    /// Sets authentication headers to our http client to access PetFinder API.
    /// </summary>
    private async Task SetAuthenticationHeaders()
    {
        Token token = await _tokenService.GetToken();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token.AccessToken);
    }
}