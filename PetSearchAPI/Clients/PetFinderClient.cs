using System.Net.Http.Headers;
using System.Text;
using System.Web;
using PetSearchAPI.Models.PetFinderResponse;
using PetSearchAPI.Models.Token;
using PetSearchAPI.RequestHelpers;

namespace PetSearchAPI.Clients;

public class PetFinderClient: IPetFinderClient
{
    private const string TokenUrl = "oauth2/token";
    private const string PetsUrl = "animals";
    
    private readonly IConfiguration _config;
    private readonly HttpClient _client;

    public PetFinderClient(IConfiguration config, HttpClient client)
    {
        _config = config;
        _client = client;
    }

    public async Task<TokenResponseDto> GetToken()
    {
        var contentBody = new TokenRequestBody
        {
            ClientId = _config["PetFinder:ClientId"],
            ClientSecret = _config["PetFinder:ClientSecret"]
        };
        
        var response = await _client.PostAsJsonAsync(TokenUrl, contentBody);
        response.EnsureSuccessStatusCode();
        
        var petFinderToken = await response.Content.ReadFromJsonAsync<TokenResponseDto>();
        return petFinderToken;
    }

    public async Task<PetsResponseDto> GetPets(PetsParams petsParams, string token)
    {
        var petUriQuery = GetPetQueryString(petsParams);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync($"{PetsUrl}?{petUriQuery}");

        response.EnsureSuccessStatusCode();
        var petResponse = await response.Content.ReadFromJsonAsync<PetResponse>();

        var petList = new PetsResponseDto
        {
            Pets = petResponse.Animals,
            Pagination = petResponse.Pagination
        };

        return petList;
    }
    public async Task<PetDto> GetSinglePet(int id, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync($"{PetsUrl}/{id}");

        response.EnsureSuccessStatusCode();
        var petResponse = await response.Content.ReadFromJsonAsync<SinglePetResponse>();

        return petResponse.Pet;
    }

    private string GetPetQueryString(PetsParams petsParams)
    {
        var queryStringBuilder = new StringBuilder();
        queryStringBuilder.AppendFormat($"type={HttpUtility.UrlEncode(petsParams.Type)}");
        queryStringBuilder.AppendFormat($"&location={HttpUtility.UrlEncode(petsParams.Location)}");
        queryStringBuilder.AppendFormat($"&page={HttpUtility.UrlEncode(petsParams.Page.ToString())}");
        queryStringBuilder.AppendFormat($"&distance={HttpUtility.UrlEncode(petsParams.Distance.ToString())}");
        queryStringBuilder.AppendFormat($"&sort={HttpUtility.UrlEncode(petsParams.Sort)}");

        return queryStringBuilder.ToString();
    }
}