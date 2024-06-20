using System.Text.Json;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using PetSearch.API.Clients;
using PetSearch.API.Handlers;
using PetSearch.API.Helpers;
using PetSearch.API.Models;
using PetSearch.API.Tests.Data;

namespace PetSearch.API.Tests.Handlers;

[TestSubject(typeof(PetHandler))]
public class PetHandlerTest
{
    private readonly PetsParams _params;
    private readonly Mock<IPetFinderClient> _mockPetFinderClient;
    private readonly HttpContext _mockHttpContext;
    private readonly string _expectedJsonSerializer;

    public PetHandlerTest()
    {
        _params = new PetsParams("dog", "92010");
        _mockPetFinderClient = new Mock<IPetFinderClient>();
        _mockHttpContext = new DefaultHttpContext();

        var petList = new List<PetDto>();
        petList.Add(PetDtoData.GetPetWithAllAttributesInitialized());
        petList.Add(PetDtoData.GetPetWithAllAttributesInitialized());
        var paginationMetaData = new PaginationMetaData(1, 2, 1, 2);

        var pagedList = new PagedList<PetDto>(petList, paginationMetaData);
        _mockPetFinderClient.Setup(c => c.GetPetsAsync(_params)).ReturnsAsync(pagedList);
        _expectedJsonSerializer = JsonSerializer.Serialize(paginationMetaData);
    }

    [Fact]
    public async Task GetPets_ShouldReturnAn_OkPetListResult()
    {
        var result = await PetHandler.GetPetsAsync(_params, _mockPetFinderClient.Object, _mockHttpContext);
        
        Assert.IsType<Ok<List<PetDto>>>(result.Result);
    }
    
    [Fact]
    public async Task GetPetsAsync_ShouldSet_XPaginationHeaders()
    {
        await PetHandler.GetPetsAsync(_params, _mockPetFinderClient.Object, _mockHttpContext);

        Assert.True(_mockHttpContext.Response.Headers.ContainsKey("X-Pagination"));
        _mockHttpContext.Response.Headers.TryGetValue("X-Pagination", out var paginationStringVal);
        Assert.Equal(paginationStringVal, _expectedJsonSerializer);
    }

    [Fact]
    public async Task GetSinglePetAsync_ShouldReturnAnOkPetDto()
    {
        var result = await PetHandler.GetSinglePetAsync(0, _mockPetFinderClient.Object);
        
        Assert.IsType<Ok<PetDto>>(result.Result);
    }
}