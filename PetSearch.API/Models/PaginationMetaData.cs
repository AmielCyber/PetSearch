using System.Text.Json.Serialization;

namespace PetSearch.API.Models;

public record PaginationMetaData(
    [property: JsonPropertyName("currentPage")]
    int CurrentPage,
    [property: JsonPropertyName("totalPages")]
    int TotalPages,
    [property: JsonPropertyName("pageSize")]
    int PageSize,
    [property: JsonPropertyName("totalCount")]
    int TotalCount
);