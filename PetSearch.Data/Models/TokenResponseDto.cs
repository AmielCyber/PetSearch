using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PetSearch.Data.Models;

/// <summary>
/// Token Response DTO for client. Tells client when that their token will expire.
/// </summary>
/// <param name="TokenType">Token Type: Bearer</param>
/// <param name="ExpiresIn">Expires in milliseconds when obtained from the back end.</param>
/// <param name="AccessToken">The token string</param>
public record TokenResponseDto
(
    [property: JsonPropertyName("token_type"), Required]
    string TokenType,
    [property: JsonPropertyName("expires_in"), Required]
    int ExpiresIn,
    [property: JsonPropertyName("access_token"), Required]
    string AccessToken
);