using System.Text.Json.Serialization;

namespace PetSearchAPI.Models.Token;

/// <summary>
/// The body properties needed to obtain a token for our client application.
/// </summary>
public class TokenRequestBody
{
    [JsonPropertyName("grant_type")] public string GrantType { get; set; } = "client_credentials";

    [JsonPropertyName("client_id")] public string ClientId { get; set; }

    [JsonPropertyName("client_secret")] public string ClientSecret { get; set; }
}