using System.Net;
using System.Net.Http.Json;
using ErrorOr;
using Moq;
using PetSearch.API.Common.Exceptions;
using PetSearch.API.Models.PetFinderResponse;
using PetSearch.API.RequestHelpers;
using PetSearch.Data.Entity;
using PetSearch.Data.Services;
using RichardSzalay.MockHttp;

namespace PetSearch.API.Tests.PetFinderClientTests;

public class GetPetsTests
{
    private const string PetFinderUrl = "https://api.petfinder.com/v2/";
    private readonly Uri _petFinderUri;
    private readonly MockHttpMessageHandler _handlerMock;
    private readonly PetResponse _expectedPetResponse;
    private readonly PetsParams _petsParamsMock;
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly Token _mockToken;

    public GetPetsTests()
    {
        _petFinderUri = new Uri(PetFinderUrl);
        _handlerMock = new MockHttpMessageHandler();
        _expectedPetResponse = new PetResponse
        (
            new[]
            {
                new PetDto
                {
                    Id = 123,
                    Url = "dogs.com",
                    Type = "dog",
                    Age = "Adult",
                    Gender = "Female",
                    Size = "Large",
                    Name = "Vanilla",
                    Description = "...",
                    Photos = Array.Empty<PhotoSizesUrl>(),
                    PrimaryPhotoSizesUrlCropped = null,
                    Status = "Adoptable",
                    Distance = 5
                }
            },
            new Pagination(20, 100, 1, 5)
        );
        _petsParamsMock = new PetsParams("dog", "92101");
        _mockTokenService = new Mock<ITokenService>();
        _mockToken = new Token
        {
            Id = 1,
            AccessToken = "Access Token",
        };
    }

    public static IEnumerable<object[]> GetValidPetsParams()
    {
        yield return new object[] { new PetsParams("dog", "92101")};
        yield return new object[] { new PetsParams("cat", "92101")};
    }

    [Theory]
    [MemberData(nameof(GetValidPetsParams))]
    public async Task GetPets_ShouldReturnPetsListAndPagination_IfPetsParamsIsValid(PetsParams petsParams)
    {
        // Arrange
        var request = _handlerMock
            .When($"{PetFinderUrl}animals?{Clients.PetFinderClient.GetPetsQueryString(petsParams)}")
            .Respond(HttpStatusCode.OK, JsonContent.Create(_expectedPetResponse));

        using var httpClient = new HttpClient(_handlerMock) { BaseAddress = _petFinderUri };
        // Set up mock token
        _mockTokenService.Setup(tokenClient => tokenClient.GetToken()).ReturnsAsync(_mockToken);
        var petFinderClient = new Clients.PetFinderClient(httpClient, _mockTokenService.Object);

        // Act
        ErrorOr<PetsResponseDto> response = await petFinderClient.GetPets(petsParams);
        var pets = response.Value;

        // Assert that we get the pagination and pet list result.
        Assert.NotNull(pets);
        Assert.IsType<PetsResponseDto>(pets);
        Assert.IsType<PetDto[]>(pets.Pets);
        Assert.InRange(pets.Pets.Length, 1, int.MaxValue);
        Assert.Contains(pets.Pets, pet => pet.GetType() == typeof(PetDto));
        Assert.IsType<Pagination>(pets.Pagination);
        Assert.Equal(1, pets.Pagination.CurrentPage);
        Assert.InRange(pets.Pagination.TotalCount, 1, int.MaxValue);
        Assert.InRange(pets.Pagination.CountPerPage, 10, int.MaxValue);
        Assert.InRange(pets.Pagination.TotalPages, 1, int.MaxValue);
        Assert.Equal(1, _handlerMock.GetMatchCount(request));
        _mockTokenService.Verify(tokenClient => tokenClient.GetToken(), Times.Once());
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task GetPets_ShouldReturnError_WhenResponseIsUnsuccessful(HttpStatusCode responseStatusCode)
    {
        // Arrange
        var request = _handlerMock
            .When($"{PetFinderUrl}animals?{Clients.PetFinderClient.GetPetsQueryString(_petsParamsMock)}")
            .Respond(responseStatusCode);
        using var httpClient = new HttpClient(_handlerMock) { BaseAddress = _petFinderUri };
        // Set up mock token
        _mockTokenService.Setup(tokenClient => tokenClient.GetToken()).ReturnsAsync(_mockToken);
        var petFinderClient = new Clients.PetFinderClient(httpClient, _mockTokenService.Object);

        // Act
        ErrorOr<PetsResponseDto> response = await petFinderClient.GetPets(_petsParamsMock);

        // Assert
        var expectedErrorType = PetFinderTestHelper.GetExpectedErrorType(responseStatusCode);
        Assert.Contains(response.Errors, error => error == expectedErrorType);
        Assert.Equal(1, _handlerMock.GetMatchCount(request));
        _mockTokenService.Verify(tokenClient => tokenClient.GetToken(), Times.Once());
    }

    [Fact]
    public async Task GetPets_ShouldThrowPetFinderForbidden_IfResponseIsForbidden()
    {
        // Arrange
        var request = _handlerMock
            .When($"{PetFinderUrl}animals?{Clients.PetFinderClient.GetPetsQueryString(_petsParamsMock)}")
            .Respond(HttpStatusCode.Forbidden);
        using var httpClient = new HttpClient(_handlerMock) { BaseAddress = _petFinderUri };
        // Set up mock token
        _mockTokenService.Setup(tokenClient => tokenClient.GetToken()).ReturnsAsync(_mockToken);
        var petFinderClient = new Clients.PetFinderClient(httpClient, _mockTokenService.Object);

        // Act and Assert.
        ErrorOr<PetsResponseDto>? result = null;
        await Assert.ThrowsAsync<PetFinderForbidden>(async () =>
        {
            result = await petFinderClient.GetPets(_petsParamsMock);
        });
        Assert.Null(result);
        Assert.Equal(1, _handlerMock.GetMatchCount(request));
        _mockTokenService.Verify(tokenClient => tokenClient.GetToken(), Times.Once());
    }
}