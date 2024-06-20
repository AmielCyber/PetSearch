using System.Net;
using System.Net.Http.Json;
using ErrorOr;
using JetBrains.Annotations;
using Moq;
using PetSearch.API.Clients;
using PetSearch.API.Configurations;
using PetSearch.API.Entities;
using PetSearch.API.Exceptions;
using PetSearch.API.Helpers;
using PetSearch.API.Models;
using PetSearch.API.Models.PetFinderResponse;
using PetSearch.API.Profiles;
using PetSearch.API.Tests.Data;
using PetSearch.Data.Entities;
using PetSearch.Data.Services;
using PetSearch.Data.StronglyTypedConfigurations;
using RichardSzalay.MockHttp;

namespace PetSearch.API.Tests.Clients;

[TestSubject(typeof(PetFinderClient))]
public class PetFinderClientTest
{
    private readonly Uri _petFinderUri;
    private readonly MockHttpMessageHandler _mockHttp;
    private readonly PetsParams _petsParamsMock;
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly Token _mockToken;
    private readonly PetProfile _petProfile;
    private readonly PaginationMetaDataProfile _paginationMetaDataProfile;
    private const int Id = 0;
    private readonly string _expectedPetUri;

    public PetFinderClientTest()
    {
        _petFinderUri = new Uri(PetFinderConfiguration.Uri);
        _mockHttp = new MockHttpMessageHandler();
        _petsParamsMock = new PetsParams("dog", "92101");
        _mockTokenService = new Mock<ITokenService>();
        _mockToken = new Token
        {
            Id = 1,
            AccessToken = "Access Token",
        };
        _mockTokenService.Setup(tokenClient => tokenClient.GetToken()).ReturnsAsync(_mockToken);
        _petProfile = new PetProfile();
        _paginationMetaDataProfile = new PaginationMetaDataProfile();
        _expectedPetUri = $"{_petFinderUri}/{Id}";
    }

    [Theory]
    [ClassData(typeof(PetEntityData))]
    public async Task GetPets_ShouldReturnA_PagedListOfPetDto(Pet pet)
    {
        // Arrange
        Pagination pagination = new Pagination(1, 3, 1, 3);
        List<Pet> petList = GetPetList(pet, pagination);
        using HttpClient httpClient = _mockHttp.ToHttpClient();
        httpClient.BaseAddress = _petFinderUri;
        PaginatedPetList paginatedPetList = new PaginatedPetList(petList, pagination);
        var request = SetGetPetsRequest(HttpStatusCode.Accepted, paginatedPetList);
        var petFinderClient =
            new PetFinderClient(httpClient, _mockTokenService.Object, _petProfile, _paginationMetaDataProfile);

        // Action
        ErrorOr<PagedList<PetDto>> response = await petFinderClient.GetPetsAsync(_petsParamsMock);

        // Assert
        Assert.IsType<PagedList<PetDto>>(response.Value);
        Assert.Equal(1, _mockHttp.GetMatchCount(request));
    }

    [Theory]
    [ClassData(typeof(PetFinderClientErrorsData))]
    public async Task GetPets_ShouldReturnCorrectError_IfResponseIsUnsuccessful(
        HttpStatusCode statusCode, Error expectedError
    )
    {
        // Arrange
        using HttpClient httpClient = _mockHttp.ToHttpClient();
        httpClient.BaseAddress = _petFinderUri;
        var request = SetGetPetsRequest(statusCode);
        var petFinderClient =
            new PetFinderClient(httpClient, _mockTokenService.Object, _petProfile, _paginationMetaDataProfile);

        // Act
        ErrorOr<PagedList<PetDto>> response = await petFinderClient.GetPetsAsync(_petsParamsMock);


        // Assert
        Assert.Equal(1, _mockHttp.GetMatchCount(request));
        _mockHttp.Expect(MapBoxConfiguration.Url).Respond(statusCode);
        Assert.Equal(expectedError, response.FirstError);
    }

    [Fact]
    public async Task GetPets_ShouldThrowForbiddenAccess_IfApiStatusCodeIs403()
    {
        // Arrange
        Pagination pagination = new Pagination(1, 3, 1, 3);
        List<Pet> petList = GetPetList(PetEntityData.GetPetWithAllAttributesInitialized(), pagination);
        using HttpClient httpClient = _mockHttp.ToHttpClient();
        httpClient.BaseAddress = _petFinderUri;
        PaginatedPetList paginatedPetList = new PaginatedPetList(petList, pagination);
        SetGetPetsRequest(HttpStatusCode.Forbidden, paginatedPetList);
        var petFinderClient =
            new PetFinderClient(httpClient, _mockTokenService.Object, _petProfile, _paginationMetaDataProfile);

        // Act and Assert
        await Assert.ThrowsAsync<ForbiddenAccessException>(async () =>
            await petFinderClient.GetPetsAsync(_petsParamsMock)
        );
    }

    [Theory]
    [ClassData(typeof(PetEntityData))]
    public async Task GetSinglePet_ShouldReturnA_PetDto(Pet pet)
    {
        using HttpClient httpClient = _mockHttp.ToHttpClient();
        httpClient.BaseAddress = _petFinderUri;
        SetGetSinglePetRequest(Id, HttpStatusCode.Accepted, pet);

        var petFinderClient =
            new PetFinderClient(httpClient, _mockTokenService.Object, _petProfile, _paginationMetaDataProfile);

        ErrorOr<PetDto> result = await petFinderClient.GetSinglePetAsync(Id);

        Assert.IsType<PetDto>(result.Value);
        _mockHttp.Expect(_expectedPetUri).Respond(HttpStatusCode.Accepted);
    }

    [Theory]
    [ClassData(typeof(PetFinderClientErrorsData))]
    public async Task GetSinglePet_ShouldReturnCorrectError_IfResponseIsUnsuccessful(
        HttpStatusCode statusCode, Error expectedError
    )
    {
        using HttpClient httpClient = _mockHttp.ToHttpClient();
        httpClient.BaseAddress = _petFinderUri;
        SetGetSinglePetRequest(Id, statusCode, null);

        var petFinderClient =
            new PetFinderClient(httpClient, _mockTokenService.Object, _petProfile, _paginationMetaDataProfile);

        ErrorOr<PetDto> result = await petFinderClient.GetSinglePetAsync(0);

        // Assert
        _mockHttp.Expect(_expectedPetUri).Respond(statusCode);
        Assert.Equal(expectedError, result.FirstError);
    }


    [Fact]
    public async Task GetSinglePet_ShouldThrowForbiddenAccess_IfApiStatusCodeIs403()
    {
        // Arrange
        using HttpClient httpClient = _mockHttp.ToHttpClient();
        httpClient.BaseAddress = _petFinderUri;
        SetGetSinglePetRequest(Id, HttpStatusCode.Forbidden, null);
        var petFinderClient =
            new PetFinderClient(httpClient, _mockTokenService.Object, _petProfile, _paginationMetaDataProfile);

        // Act and Assert
        await Assert.ThrowsAsync<ForbiddenAccessException>(async () =>
            await petFinderClient.GetSinglePetAsync(Id)
        );
    }

    private List<Pet> GetPetList(Pet petEntity, Pagination pagination)
    {
        List<Pet> petList = new List<Pet>();
        for (int i = 0; i < pagination.TotalCount; i++)
        {
            petList.Add(petEntity);
        }

        return petList;
    }

    private IMockedRequest SetGetPetsRequest(HttpStatusCode statusCode,
        PaginatedPetList? paginatedPetList = null)
    {
        const string url = $"{PetFinderConfiguration.Uri}animals*";

        if (statusCode == HttpStatusCode.Accepted && paginatedPetList is not null)
        {
            return
                _mockHttp
                    .When(url)
                    .Respond(HttpStatusCode.Accepted, JsonContent.Create(new
                    {
                        paginatedPetList.Animals, paginatedPetList.Pagination
                    }));
        }

        return _mockHttp
            .When(url)
            .Respond(statusCode);
    }

    private IMockedRequest SetGetSinglePetRequest(int id,
        HttpStatusCode statusCode, Pet? pet
    )
    {
        string url = $"{PetFinderConfiguration.Uri}animals/{id}";

        if (statusCode == HttpStatusCode.Accepted && pet is not null)
        {
            return
                _mockHttp
                    .When(url)
                    .Respond(HttpStatusCode.Accepted, JsonContent.Create(new
                    {
                        Animal = pet
                    }));
        }

        return _mockHttp
            .When(url)
            .Respond(statusCode);
    }
}