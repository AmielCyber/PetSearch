using System.Text.Json.Serialization;

namespace PetSearchAPI.Models.PetFinderResponse;

/// <summary>
/// Pagination object for our client app to navigate through the available pets in their area.
/// </summary>
public class Pagination
{
    [JsonPropertyName("count_per_page")] public int CountPerPage { get; init; }
    [JsonPropertyName("total_count")] public int TotalCount { get; init; }
    [JsonPropertyName("current_page")] public int CurrentPage { get; init; }
    [JsonPropertyName("total_pages")] public int TotalPages { get; init; }
}