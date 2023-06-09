using System.Net;
using System.Net.Http.Json;
using ErrorOr;
using PetSearchAPI.Clients;
using PetSearchAPI.Common.Errors;
using PetSearchAPI.Common.Exceptions;
using PetSearchAPI.Models.PetFinderResponse;
using RichardSzalay.MockHttp;

namespace PetSearchAPI.Tests.PetFinderClientTests;

public class GetSinglePetTests
{
    private const string PetFinderUrl = "https://api.petfinder.com/v2/";
    private const string MockToken = "Token";
    private const int MockId = 1;
    private readonly Uri _petFinderUri;
    private readonly MockHttpMessageHandler _handlerMock;
    private readonly PetDto _expectedPetDto;
    private readonly SinglePetResponse _expectedSinglePetResponse;

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
    }

    [Fact]
    public async Task GetSinglePet_ShouldReturnMissingTokenError_IfTokenIsMissing()
    {
        // Arrange
        using var httpClient = new HttpClient(_handlerMock) { BaseAddress = _petFinderUri };
        var petFinderClient = new PetFinderClient(httpClient);

        // Act
        ErrorOr<PetDto> request = await petFinderClient.GetSinglePet(MockId, null);

        // Assert.
        Assert.Contains(request.Errors, error => error == Errors.Token.MissingToken);
    }

    [Fact]
    public async Task GetSinglePet_ShouldReturn_APetDtoObject_IfResponseIsSuccessful()
    {
        // Arrange
        var request = _handlerMock
            .When($"{PetFinderUrl}animals/{MockId}")
            .Respond(HttpStatusCode.OK, JsonContent.Create(_expectedSinglePetResponse));
        using var httpClient = new HttpClient(_handlerMock) { BaseAddress = _petFinderUri };
        var petFinderClient = new PetFinderClient(httpClient);

        // Act
        ErrorOr<PetDto> response = await petFinderClient.GetSinglePet(MockId, MockToken);
        PetDto petDto = response.Value;

        // Assert
        Assert.IsType<PetDto>(response.Value);
        Assert.Equal(_expectedPetDto.Id, petDto.Id);
        Assert.Equal(_expectedPetDto.Name, petDto.Name);
        Assert.Equal(_expectedPetDto.Age, petDto.Age);
        Assert.Equal(1, _handlerMock.GetMatchCount(request));
    }

    [Fact]
    public async Task GetSinglePet_ShouldThrowPetFinderForbidden_IfResponseIsForbidden()
    {
        // Arrange
        var request = _handlerMock
            .When($"{PetFinderUrl}animals/{MockId}")
            .Respond(HttpStatusCode.Forbidden);
        using var httpClient = new HttpClient(_handlerMock) { BaseAddress = _petFinderUri };
        var petFinderClient = new PetFinderClient(httpClient);

        // Act and Assert
        ErrorOr<PetDto>? result = null;
        await Assert.ThrowsAsync<PetFinderForbidden>(async () =>
        {
            result = await petFinderClient.GetSinglePet(MockId, MockToken);
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
        var petFinderClient = new PetFinderClient(httpClient);

        // Act
        ErrorOr<PetDto> response = await petFinderClient.GetSinglePet(MockId, MockToken);

        // Assert
        var expectedErrorType = PetFinderTestHelper.GetExpectedErrorType(responseStatusCode);
        Assert.Contains(response.Errors, error => error == expectedErrorType);
        Assert.Equal(1, _handlerMock.GetMatchCount(request));
    }
}