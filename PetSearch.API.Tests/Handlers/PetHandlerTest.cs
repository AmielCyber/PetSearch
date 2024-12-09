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
    private readonly int _id;
    private readonly Mock<IPetFinderClient> _mockPetFinderClient;
    private readonly HttpResponse _mockHttpResponse;
    private readonly string _expectedJsonSerializer;

    public PetHandlerTest()
    {
        _params = new PetsParams("dog", "92010");
        _id = 1;
        _mockPetFinderClient = new Mock<IPetFinderClient>();
        _mockHttpResponse = new DefaultHttpContext().Response;

        var petList = new List<PetDto>();
        petList.Add(PetDtoData.GetPetWithAllAttributesInitialized());
        petList.Add(PetDtoData.GetPetWithAllAttributesInitialized());
        var paginationMetaData = new PaginationMetaData(1, 2, 1, 2);

        var pagedList = new PagedList<PetDto>(petList, paginationMetaData);
        _mockPetFinderClient.Setup(c => c.GetPetsAsync(_params)).ReturnsAsync(TypedResults.Ok(pagedList));
        _mockPetFinderClient.Setup(c => c.GetSinglePetAsync(_id)).ReturnsAsync(TypedResults.Ok(petList[0]));
        _expectedJsonSerializer = JsonSerializer.Serialize(paginationMetaData);
    }


    [Fact]
    public async Task GetPets_ShouldReturnAn_OkPetListResult()
    {
        var result = await PetHandler.GetPetsAsync(_params, _mockPetFinderClient.Object, _mockHttpResponse);

        Assert.IsType<Ok<List<PetDto>>>(result.Result);
    }

    [Fact]
    public async Task GetPetsAsync_ShouldSet_XPaginationHeaders()
    {
        await PetHandler.GetPetsAsync(_params, _mockPetFinderClient.Object, _mockHttpResponse);

        Assert.True(_mockHttpResponse.Headers.ContainsKey("X-Pagination"));
        _mockHttpResponse.Headers.TryGetValue("X-Pagination", out var paginationStringVal);
        Assert.Equal(paginationStringVal, _expectedJsonSerializer);
    }

    [Fact]
    public async Task GetSinglePetAsync_ShouldReturnAnOkPetDto()
    {
        var result = await PetHandler.GetSinglePetAsync(_id, _mockPetFinderClient.Object);

        Assert.IsType<Ok<PetDto>>(result.Result);
    }
}