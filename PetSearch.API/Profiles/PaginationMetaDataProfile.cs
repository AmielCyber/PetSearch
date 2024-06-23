using PetSearch.API.Models;
using PetSearch.API.Models.PetFinderResponse;

namespace PetSearch.API.Profiles;

/// <summary>
/// Maps a Pagination entity from PetFinder to a PaginationMetaData to use in our headers.
/// </summary>
public class PaginationMetaDataProfile
{
    /// <summary>
    /// Maps a Pagination entity from PetFinder to a PaginationMetaData to use in our headers.
    /// </summary>
    public PaginationMetaData MapPaginationToPaginationMetaData(Pagination pagination)
    {
        return new PaginationMetaData(pagination.CurrentPage, pagination.TotalPages, pagination.CountPerPage,
            pagination.TotalCount);
    }
}