using System.Net;
using System.Net.Http.Json;
using ErrorOr;
using PetSearchAPI.Clients;
using PetSearchAPI.Common.Errors;
using PetSearchAPI.Common.Exceptions;
using PetSearchAPI.Models.PetFinderResponse;
using PetSearchAPI.RequestHelpers;
using RichardSzalay.MockHttp;

namespace PetSearchAPI.Tests.PetFinderClientTests;

public class GetPetsTests
{
    private const string PetFinderUrl = "https://api.petfinder.com/v2/";
    private const string TokenMock = "Token";
    private readonly Uri _petFinderUri;
    private readonly MockHttpMessageHandler _handlerMock;
    private readonly PetResponse _expectedPetResponse;
    private readonly PetsParams _petsParamsMock;

    public GetPetsTests()
    {
        _petFinderUri = new Uri(PetFinderUrl);
        _handlerMock = new MockHttpMessageHandler();
        _expectedPetResponse = new PetResponse
        {
            Pagination = new Pagination
            {
                CountPerPage = 20,
                TotalCount = 100,
                CurrentPage = 1,
                TotalPages = 5
            },
            Animals = new[]
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
                    Photos = null,
                    PrimaryPhotoSizesUrlCropped = null,
                    Status = "Adoptable",
                    Distance = 5
                }
            }
        };
        _petsParamsMock = new PetsParams { Type = "dog", Location = "92101" };
    }

    public static IEnumerable<object[]> GetValidPetsParams()
    {
        yield return new object[] { new PetsParams { Type = "dog", Location = "92101" } };
        yield return new object[] { new PetsParams { Type = "cat", Location = "92101" } };
    }

    [Fact]
    public async Task GetPets_ShouldReturn_MissingTokenError_IfTokenIsMissing()
    {
        // Arrange
        using var httpClient = new HttpClient(_handlerMock) { BaseAddress = _petFinderUri };
        var petFinderClient = new PetFinderClient(httpClient);

        // Act
        ErrorOr<PetsResponseDto> response = await petFinderClient.GetPets(_petsParamsMock, null);

        // Assert
        Assert.Contains(response.Errors, error => error == Errors.Token.MissingToken);
    }

    [Theory]
    [MemberData(nameof(GetValidPetsParams))]
    public async Task GetPets_ShouldReturnPetsListAndPagination_IfPetsParamsIsValid(PetsParams petsParams)
    {
        // Arrange
        var request = _handlerMock
            .When($"{PetFinderUrl}animals?{PetFinderClient.GetPetsQueryString(petsParams)}")
            .Respond(HttpStatusCode.OK, JsonContent.Create(_expectedPetResponse));

        using var httpClient = new HttpClient(_handlerMock) { BaseAddress = _petFinderUri };
        var petFinderClient = new PetFinderClient(httpClient);

        // Act
        ErrorOr<PetsResponseDto> response = await petFinderClient.GetPets(petsParams, TokenMock);
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
            .When($"{PetFinderUrl}animals?{PetFinderClient.GetPetsQueryString(_petsParamsMock)}")
            .Respond(responseStatusCode);
        using var httpClient = new HttpClient(_handlerMock) { BaseAddress = _petFinderUri };
        var petFinderClient = new PetFinderClient(httpClient);

        // Act
        ErrorOr<PetsResponseDto> response = await petFinderClient.GetPets(_petsParamsMock, TokenMock);

        // Assert
        var expectedErrorType = PetFinderTestHelper.GetExpectedErrorType(responseStatusCode);
        Assert.Contains(response.Errors, error => error == expectedErrorType);
        Assert.Equal(1, _handlerMock.GetMatchCount(request));
    }

    [Fact]
    public async Task GetPets_ShouldThrowPetFinderForbidden_IfResponseIsForbidden()
    {
        // Arrange
        var request = _handlerMock
            .When($"{PetFinderUrl}animals?{PetFinderClient.GetPetsQueryString(_petsParamsMock)}")
            .Respond(HttpStatusCode.Forbidden);
        using var httpClient = new HttpClient(_handlerMock) { BaseAddress = _petFinderUri };
        var petFinderClient = new PetFinderClient(httpClient);

        // Act and Assert.
        ErrorOr<PetsResponseDto>? result = null;
        await Assert.ThrowsAsync<PetFinderForbidden>(async () =>
        {
            result = await petFinderClient.GetPets(_petsParamsMock, TokenMock);
        });
        Assert.Null(result);
        Assert.Equal(1, _handlerMock.GetMatchCount(request));
    }
}