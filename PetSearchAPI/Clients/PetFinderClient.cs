using System.Text;
using System.Web;
using ErrorOr;
using PetSearchAPI.Common.Errors;
using PetSearchAPI.Common.Exceptions;
using PetSearchAPI.Models.PetFinderResponse;
using PetSearchAPI.RequestHelpers;

namespace PetSearchAPI.Clients;

/// <summary>
/// PetFinderClient implementation to handle requests from the PetFinderAPI.
/// Returns the appropriate response to our client app.
/// </summary>
public class PetFinderClient : IPetFinderClient
{
    private readonly HttpClient _client;

    /// <summary>
    /// Set up dependency injection. 
    /// </summary>
    /// <param name="client">Have access to the global HttpClient object to make external API requests.</param>
    public PetFinderClient(HttpClient client)
    {
        _client = client; // To use the http client with a base address.
    }

    /// <summary>
    /// Gets a list of available pets for adoption and the pagination object.
    /// </summary>
    /// <param name="petsParams">Search Query object with its properties as the accepted parameters to use
    /// in the search query.</param>
    /// <param name="accessToken">Header access token we gave to the client to access this endpoint.</param>
    /// <returns>A PetsResponseDto with a list of available pets and the pagination object if the object
    /// was successfully fetched. Else, returns a custom error type.</returns>
    /// <exception cref="PetFinderForbidden">Throws when an error code is 403 since it is not a client error.</exception>
    public async Task<ErrorOr<PetsResponseDto>> GetPets(PetsParams petsParams, string accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            return Errors.Token.MissingToken;
        }

        string petUriQuery = GetPetsQueryString(petsParams);

        _client.DefaultRequestHeaders.Add("Authorization", accessToken);
        using HttpResponseMessage response = await _client.GetAsync($"animals?{petUriQuery}");

        if (!response.IsSuccessStatusCode)
        {
            return GetPetsError((int)response.StatusCode);
        }

        PetResponse? petResponse = await response.Content.ReadFromJsonAsync<PetResponse>();

        if (petResponse is null)
        {
            return GetPetsError(500);
        }

        return MapPetResponseToPetsResponseDto(petResponse);
    }

    /// <summary>
    /// Gets a single pet object by their id.
    /// </summary>
    /// <param name="id">The id of the pet.</param>
    /// <param name="accessToken">Header access token we gave to the client to access this endpoint.</param>
    /// <returns>A PetDto if the request was successful, else returns an ErrorOr Error if request return without a 200
    /// response code.</returns>
    /// <exception cref="PetFinderForbidden">Throws when an error code is 403 since it is not a client error.</exception>
    public async Task<ErrorOr<PetDto>> GetSinglePet(int id, string accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            return Errors.Token.MissingToken;
        }

        _client.DefaultRequestHeaders.Add("Authorization", accessToken);
        using HttpResponseMessage response = await _client.GetAsync($"animals/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return GetPetsError((int)response.StatusCode);
        }

        SinglePetResponse? petResponse = await response.Content.ReadFromJsonAsync<SinglePetResponse>();

        if (petResponse is null)
        {
            return GetPetsError(500);
        }
        
        // Only return PetDto.
        return petResponse.Pet;
    }

    /// <summary>
    /// Maps a PetResponse object to a PetsResponseDto to send to the client.
    /// </summary>
    /// <param name="petResponse">The partial pet response object we got from pets request from PetFinder API.</param>
    /// <returns>PetsResponseDto that lists all available pets and the server's pagination.</returns>
    private PetsResponseDto MapPetResponseToPetsResponseDto(PetResponse petResponse)
    {
        return new PetsResponseDto
        {
            Pets = petResponse.Animals,
            Pagination = petResponse.Pagination
        };
    }

    /// <summary>
    /// Transforms our PetsParams object into a query string for PetFinderAPI.
    /// </summary>
    /// <param name="petsParams">Pets Parameter object containing search queries as properties.</param>
    /// <returns>A query string to use for the PetFinder API</returns>
    public static string GetPetsQueryString(PetsParams petsParams)
    {
        var queryStringBuilder = new StringBuilder();

        // Required.
        queryStringBuilder.AppendFormat($"type={HttpUtility.UrlEncode(petsParams.Type)}");
        // Required.
        queryStringBuilder.AppendFormat($"&location={HttpUtility.UrlEncode(petsParams.Location)}");
        // Default page=1.
        queryStringBuilder.AppendFormat($"&page={HttpUtility.UrlEncode(petsParams.Page.ToString())}");
        // Default distance=50.
        queryStringBuilder.AppendFormat($"&distance={HttpUtility.UrlEncode(petsParams.Distance.ToString())}");
        // Default sort=recent.
        queryStringBuilder.AppendFormat($"&sort={HttpUtility.UrlEncode(petsParams.Sort)}");

        return queryStringBuilder.ToString();
    }

    /// <summary>
    /// Gets a custom ErrorOr Error while requesting an endpoint from PetFinder API.
    /// </summary>
    /// <param name="statusCode">The request status code that we got from an error.</param>
    /// <returns>ErrorOr Error type.</returns>
    /// <exception cref="PetFinderForbidden">Throws when an error code is 403 since it is not a client error.</exception>
    private static Error GetPetsError(int statusCode)
    {
        if (statusCode == 403)
        {
            // Throw exception since its unexpected and something we have to handle on our end, since our
            // client app handles auto refresh token.
            // Is catch by our global error handler and will log exception.
            throw new PetFinderForbidden();
        }

        return statusCode switch
        {
            400 => Errors.Pets.BadRequest,
            401 => Errors.Token.NotAuthorized,
            404 => Errors.Pets.NotFound,
            _ => Errors.Pets.ServerError,
        };
    }
}