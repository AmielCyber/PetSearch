using System.Text.Json.Serialization;

namespace PetSearchAPI.Models.Token;

/// <summary>
/// The token object we will send to the client app in order to reuse the token with our server.
/// </summary>
public class TokenResponseDto
{
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    
}