using System.Text;
using System.Web;
using ErrorOr;
using PetSearchAPI.Common.Errors;
using PetSearchAPI.Common.Exceptions;
using PetSearchAPI.Models.PetFinderResponse;
using PetSearchAPI.Models.Token;
using PetSearchAPI.RequestHelpers;

namespace PetSearchAPI.Clients;

/// <summary>
/// PetFinderClient implementation to handle requests from the PetFinderAPI.
/// Returns the appropriate response to our client app.
/// </summary>
public class PetFinderClient: IPetFinderClient
{
    private const string TokenUrl = "oauth2/token";
    private const string PetsUrl = "animals";
    
    private readonly IConfiguration _config;
    private readonly HttpClient _client;

    /// <summary>
    /// Set up dependencies. 
    /// </summary>
    /// <param name="config">Have access to our env keys.</param>
    /// <param name="client">Have access to the global HttpClient object to make requests.</param>
    public PetFinderClient(IConfiguration config, HttpClient client)
    {
        _config = config;   // To get our keys.
        _client = client;   // To use the http client with a base address.
    }

    /// <summary>
    /// Gets token for the client application in order to call our endpoints.
    /// </summary>
    /// <returns>Token response body</returns>
    /// <exception cref="TokenFetchException">Throws if we failed to obtain a token from PetFinder API</exception>
    public async Task<TokenResponseDto> GetToken()
    {
        var requestBody = new TokenRequestBody
        {
            ClientId = _config["PetFinder:ClientId"],
            ClientSecret = _config["PetFinder:ClientSecret"]
        };
        
        using HttpResponseMessage response = await _client.PostAsJsonAsync(TokenUrl, requestBody);

        if (!response.IsSuccessStatusCode)
        {
            // Throw the exception since that is something that happened in our end or the api (500).
            throw new TokenFetchException();
        }
        
        TokenResponseDto petFinderToken = await response.Content.ReadFromJsonAsync<TokenResponseDto>();
        return petFinderToken;
    }

    /// <summary>
    /// Gets a list of available pets for adoption and the pagination object.
    /// </summary>
    /// <param name="petsParams">Parameter object with its properties as the accepted parameters to use
    /// in the search query.</param>
    /// <param name="accessToken">Header access token we gave to the client to access this endpoint.</param>
    /// <returns>A PetsResponseDto with a list of available pets and the pagination object if the object
    /// was successfully fetched. Else, returns a custom error type.</returns>
    public async Task<ErrorOr<PetsResponseDto>> GetPets(PetsParams petsParams, string accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            return Errors.Token.MissingToken;
        }
        
        string petUriQuery = GetPetsQueryString(petsParams);
        
        _client.DefaultRequestHeaders.Add("Authorization", accessToken);
        using HttpResponseMessage response = await _client.GetAsync($"{PetsUrl}?{petUriQuery}");

        if (!response.IsSuccessStatusCode)
        {
            return GetPetsError((int)response.StatusCode);
        }

        PetResponse petResponse = await response.Content.ReadFromJsonAsync<PetResponse>();

        // Map PetResponse to 
        return MapPetResponseToPetsResponseDto(petResponse);
    }
    /// <summary>
    /// Gets a single pet by their id.
    /// </summary>
    /// <param name="id">The id of the pet.</param>
    /// <param name="accessToken">Header access token we gave to the client to access this endpoint.</param>
    /// <returns>A PetDto if the request was successful, else returns an ErrorOr Error if request return without a 200
    /// response code.</returns>
    public async Task<ErrorOr<PetDto>> GetSinglePet(int id, string accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            return Errors.Token.MissingToken;
        }
        
        _client.DefaultRequestHeaders.Add("Authorization", accessToken);
        using HttpResponseMessage response = await _client.GetAsync($"{PetsUrl}/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return GetPetsError((int)response.StatusCode);
        }
        
        SinglePetResponse petResponse = await response.Content.ReadFromJsonAsync<SinglePetResponse>();
        // Only return PetDto.
        return petResponse.Pet;
    }

    /// <summary>
    /// Maps a PetResponse object to a PetsResponseDto to send to the client.
    /// </summary>
    /// <param name="petResponse">The pet response we got from pets request from PetFinder API.</param>
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
    /// <returns>A query string</returns>
    private static string GetPetsQueryString(PetsParams petsParams)
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