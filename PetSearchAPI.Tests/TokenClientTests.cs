using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Moq;
using PetSearchAPI.Clients;
using PetSearchAPI.Common.Exceptions;
using PetSearchAPI.Models.Token;
using PetSearchAPI.StronglyTypedConfigurations;
using RichardSzalay.MockHttp;

namespace PetSearchAPI.Tests;

public class TokenClientTests
{
    private static readonly string TokenUrl = "https://api.petfinder.com/v2/oauth2/token";
    private readonly Uri _petFinderTokenUri;
    private readonly PetFinderConfiguration _petFinderConfiguration;
    private readonly Mock<IOptions<PetFinderConfiguration>> _petFinderOptionsMock;
    private readonly MockHttpMessageHandler _handlerMock;
    private readonly TokenResponseDto _expectedResponseDto;

    public TokenClientTests()
    {
        _petFinderTokenUri = new Uri(TokenUrl);
        _handlerMock = new MockHttpMessageHandler();

        // Set up configuration mock.
        const string keyValueMock = "Key Value";
        _petFinderConfiguration = new PetFinderConfiguration
        {
            ClientId = keyValueMock,
            ClientSecret = keyValueMock
        };
        _petFinderOptionsMock = new Mock<IOptions<PetFinderConfiguration>>();

        // Expected GetToken Result.
        _expectedResponseDto = new TokenResponseDto("Bearer", 3600, "TokenValue");
    }

    [Fact]
    public async Task GetToken_ShouldReturnToken_WhenSuccessful()
    {
        // Arrange.
        // Set up HttpMessageHandler mock for httpclient.
        var request = _handlerMock
            .When(TokenUrl)
            .Respond(HttpStatusCode.OK, JsonContent.Create(new
            {
                token_type = "Bearer",
                expires_in = 3600,
                access_token = "TokenValue"
            }));
        // Arrange httpClient and tokenClient with mock objects.
        using var httpClient = new HttpClient(_handlerMock) { BaseAddress = _petFinderTokenUri };
        _petFinderOptionsMock.Setup(options => options.Value).Returns(_petFinderConfiguration);
        var tokenClient = new TokenClient(httpClient, _petFinderOptionsMock.Object);

        // Act
        var result = await tokenClient.GetToken();

        // Assert.
        Assert.NotNull(result);
        Assert.Equal(_expectedResponseDto.AccessToken, result.AccessToken);
        Assert.Equal(_expectedResponseDto.ExpiresIn, result.ExpiresIn);
        Assert.Equal(_expectedResponseDto.TokenType, result.TokenType);
        // Verify calls are made to get our keys.
        _petFinderOptionsMock.Verify(options => options.Value, Times.Exactly(1));
        // Http client gets called only once.
        Assert.Equal(1, _handlerMock.GetMatchCount(request));
    }

    [Theory]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task GetToken_ShouldThrowTokenError_WhenResponseIsUnsuccessful(HttpStatusCode httpStatusCode)
    {
        // Arrange.
        // Set up HttpMessageHandler mock for httpclient.
        var request = _handlerMock
            .When(TokenUrl)
            .Respond(httpStatusCode);
        // Arrange httpClient and tokenClient with mock objects.
        using var httpClient = new HttpClient(_handlerMock) { BaseAddress = _petFinderTokenUri };
        _petFinderOptionsMock.Setup(options => options.Value).Returns(_petFinderConfiguration);
        var tokenClient = new TokenClient(httpClient, _petFinderOptionsMock.Object);

        // Assert
        TokenResponseDto? result = null;
        await Assert.ThrowsAsync<TokenFetchException>(async () =>
        {
            // Act
            result = await tokenClient.GetToken();
        });

        // Assert
        Assert.Null(result);
        // Verify calls are made to get our keys.
        _petFinderOptionsMock.Verify(options => options.Value, Times.Exactly(1));
        // Http client gets called only once.
        Assert.Equal(1, _handlerMock.GetMatchCount(request));
    }
}