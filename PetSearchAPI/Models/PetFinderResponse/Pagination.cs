using System.Text.Json.Serialization;

namespace PetSearchAPI.Models.PetFinderResponse;

public class Pagination
{
    [JsonPropertyName("count_per_page")]
    public int CountPerPage { get; set; }
    [JsonPropertyName("total_count")]
    public int TotalCount { get; set; }
    [JsonPropertyName("current_page")]
    public int CurrentPage { get; set; }
    [JsonPropertyName("total_pages")]
    public int TotalPages { get; set; }
}