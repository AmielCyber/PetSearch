using System.Text.Json.Serialization;

namespace PetSearchAPI.Models.Token;

/// <summary>
/// The body properties needed to obtain a token for our client application.
/// </summary>
public class TokenRequestBody
{
    [JsonPropertyName("grant_type")] public string GrantType { get; } = "client_credentials";
    [JsonPropertyName("client_id")] public required string ClientId { get; init; }
    [JsonPropertyName("client_secret")] public required string ClientSecret { get; init; }
}