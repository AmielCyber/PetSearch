using System.Text.Json.Serialization;

namespace PetSearch.Data.Models;

/// <summary>
/// Token Request Body when we request a token from the PetFinderApi.
/// </summary>
/// <param name="ClientId">The Client Id key provided by PetFinderApi</param>
/// <param name="ClientSecret">The Client Secret key provided by PetFinderApi</param>
/// <param name="GrantType">Default to "client_credentials"</param>
public record TokenRequestBody
(
    [property: JsonPropertyName("client_id")]
    string ClientId,
    [property: JsonPropertyName("client_secret")]
    string ClientSecret,
    [property: JsonPropertyName("grant_type")]
    string GrantType = "client_credentials"
);