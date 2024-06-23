using JetBrains.Annotations;
using PetSearch.API.Models;
using PetSearch.API.Models.PetFinderResponse;
using PetSearch.API.Profiles;

namespace PetSearch.API.Tests.Profiles;

[TestSubject(typeof(PaginationMetaDataProfile))]
public class PaginationMetaDataProfileTest
{
    private readonly Pagination _pagination;

    public PaginationMetaDataProfileTest()
    {
        _pagination = new Pagination(5, 20, 1, 4);
    }

    [Fact]
    public void MapPaginationToPaginationMetaData_ShouldReturn_PaginationMetaData()
    {
        var paginationMetaDataProfile = new PaginationMetaDataProfile();

        var paginationMetaData = paginationMetaDataProfile.MapPaginationToPaginationMetaData(_pagination);

        Assert.IsType<PaginationMetaData>(paginationMetaData);
    }

    [Fact]
    public void MapPaginationToPaginationMetaData_ShouldReturn_SameValues()
    {
        var paginationMetaDataProfile = new PaginationMetaDataProfile();

        var paginationMetaData = paginationMetaDataProfile.MapPaginationToPaginationMetaData(_pagination);

        Assert.Equal(_pagination.CurrentPage, paginationMetaData.CurrentPage);
        Assert.Equal(_pagination.TotalCount, paginationMetaData.TotalCount);
        Assert.Equal(_pagination.CountPerPage, paginationMetaData.PageSize);
        Assert.Equal(_pagination.TotalPages, paginationMetaData.TotalPages);
    }
}