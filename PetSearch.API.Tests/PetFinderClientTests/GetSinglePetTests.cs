using System.Net;
using System.Net.Http.Json;
using ErrorOr;
using Moq;
using PetSearch.API.Clients;
using PetSearch.API.Common.Exceptions;
using PetSearch.API.Models.PetFinderResponse;
using PetSearch.Data.Entities;
using PetSearch.Data.Services;
using RichardSzalay.MockHttp;

namespace PetSearch.API.Tests.PetFinderClientTests;

public class GetSinglePetTests
{
    private const string PetFinderUrl = "https://api.petfinder.com/v2/";
    private const int MockId = 1;
    private readonly Uri _petFinderUri;
    private readonly MockHttpMessageHandler _handlerMock;
    private readonly PetDto _expectedPetDto;
    private readonly SinglePetResponse _expectedSinglePetResponse;
    private readonly Mock<ITokenService> _mockTokenClient;
    private readonly Token _mockToken;

    public GetSinglePetTests()
    {
        _petFinderUri = new Uri(PetFinderUrl);
        _handlerMock = new MockHttpMessageHandler();
        _expectedPetDto = new PetDto
        {
            Id = MockId,
            Url = "PetFinder.com",
            Type = "Cat",
            Age = "Adult",
            Gender = "Female",
            Size = "Small",
            Name = "Apocalypse",
            Description = "...",
            Photos = Array.Empty<PhotoSizesUrl>(),
            PrimaryPhotoSizesUrlCropped = null,
            Status = "Adoptable",
            Distance = null,
        };
        _expectedSinglePetResponse = new SinglePetResponse(_expectedPetDto);
        _mockTokenClient = new Mock<ITokenService>();
        _mockToken = new Token
        {
            Id = 1,
            AccessToken = "Access Token",
        };
    }

    [Fact]
    public async Task GetSinglePet_ShouldReturn_APetDtoObject_IfResponseIsSuccessful()
    {
        // Arrange
        var request = _handlerMock
            .When($"{PetFinderUrl}animals/{MockId}")
            .Respond(HttpStatusCode.OK, JsonContent.Create(_expectedSinglePetResponse));
        using var httpClient = new HttpClient(_handlerMock) { BaseAddress = _petFinderUri };
        // Set up mock token
        _mockTokenClient.Setup(tokenClient => tokenClient.GetToken()).ReturnsAsync(_mockToken);
        var petFinderClient = new PetFinderClient(httpClient, _mockTokenClient.Object);

        // Act
        ErrorOr<PetDto> response = await petFinderClient.GetSinglePet(MockId);
        PetDto petDto = response.Value;

        // Assert
        Assert.IsType<PetDto>(response.Value);
        Assert.Equal(_expectedPetDto.Id, petDto.Id);
        Assert.Equal(_expectedPetDto.Name, petDto.Name);
        Assert.Equal(_expectedPetDto.Age, petDto.Age);
        Assert.Equal(1, _handlerMock.GetMatchCount(request));
        _mockTokenClient.Verify(tokenClient => tokenClient.GetToken(), Times.Once());
    }

    [Fact]
    public async Task GetSinglePet_ShouldThrowPetFinderForbidden_IfResponseIsForbidden()
    {
        // Arrange
        var request = _handlerMock
            .When($"{PetFinderUrl}animals/{MockId}")
            .Respond(HttpStatusCode.Forbidden);
        using var httpClient = new HttpClient(_handlerMock) { BaseAddress = _petFinderUri };
        // Set up mock token
        _mockTokenClient.Setup(tokenClient => tokenClient.GetToken()).ReturnsAsync(_mockToken);
        var petFinderClient = new PetFinderClient(httpClient, _mockTokenClient.Object);

        // Act and Assert
        ErrorOr<PetDto>? result = null;
        await Assert.ThrowsAsync<PetFinderForbidden>(async () =>
        {
            result = await petFinderClient.GetSinglePet(MockId);
        });
        Assert.Null(result);
        Assert.Equal(1, _handlerMock.GetMatchCount(request));
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
            .When($"{PetFinderUrl}animals/{MockId}")
            .Respond(responseStatusCode);
        using var httpClient = new HttpClient(_handlerMock) { BaseAddress = _petFinderUri };
        // Set up mock token
        _mockTokenClient.Setup(tokenClient => tokenClient.GetToken()).ReturnsAsync(_mockToken);
        var petFinderClient = new PetFinderClient(httpClient, _mockTokenClient.Object);

        // Act
        ErrorOr<PetDto> response = await petFinderClient.GetSinglePet(MockId);

        // Assert
        var expectedErrorType = PetFinderTestHelper.GetExpectedErrorType(responseStatusCode);
        Assert.Contains(response.Errors, error => error == expectedErrorType);
        Assert.Equal(1, _handlerMock.GetMatchCount(request));
    }
}