using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Moq;
using PetSearchAPI.Clients;
using PetSearchAPI.Common.Exceptions;
using PetSearchAPI.Models.Token;
using RichardSzalay.MockHttp;

namespace PetSearchAPI.Tests;

public class TokenClientTests
{
    private static readonly Uri PetFinderTokenUri = new Uri("https://api.petfinder.com/v2/oauth2/token");
    private readonly MockHttpMessageHandler _handlerMock;
    private readonly Mock<IConfiguration> _configMock;
    private readonly TokenResponseDto _expectedResponseDto;

    public TokenClientTests()
    {
        _handlerMock = new MockHttpMessageHandler();
        
        // Set up configuration mock.
        const string keyValueMock = "Key Value";
        _configMock = new Mock<IConfiguration>();
        _configMock.Setup(c => c["PetFinder:ClientId"])
            .Returns(keyValueMock);
        _configMock.Setup(c => c["PetFinder:ClientSecret"])
            .Returns(keyValueMock);
        
        // Expected GetToken Result.
        _expectedResponseDto = new TokenResponseDto
        {
            TokenType = "Bearer",
            ExpiresIn = 3600,
            AccessToken = "TokenValue"
        };
    }

    [Fact]
    public async Task GetToken_ShouldReturnToken_WhenSuccessful()
    {
        // Arrange.
        // Set up HttpMessageHandler mock for httpclient.
        var request = _handlerMock
            .When("https://api.petfinder.com/v2/oauth2/token")
            .Respond(HttpStatusCode.OK, JsonContent.Create(new
            {
                token_type = "Bearer",
                expires_in = 3600,
                access_token = "TokenValue"
            }));
        // Arrange httpClient and tokenClient with mock objects.
        using var httpClient = new HttpClient(_handlerMock) { BaseAddress = PetFinderTokenUri };
        var tokenClient = new TokenClient(_configMock.Object, httpClient);
        
        // Act
        var result = await tokenClient.GetToken();
        
        // Assert.
        Assert.NotNull(result);
        Assert.Equal(_expectedResponseDto.AccessToken, result.AccessToken);
        Assert.Equal(_expectedResponseDto.ExpiresIn, result.ExpiresIn);
        Assert.Equal(_expectedResponseDto.TokenType, result.TokenType);
        // Verify calls are made to get our keys.
        _configMock.Verify(c => c["PetFinder:ClientId"], Times.Exactly(1));
        _configMock.Verify(c => c["PetFinder:ClientSecret"], Times.Exactly(1));
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
            .When("https://api.petfinder.com/v2/oauth2/token")
            .Respond(httpStatusCode);
        // Arrange httpClient and tokenClient with mock objects.
        using var httpClient = new HttpClient(_handlerMock) { BaseAddress = PetFinderTokenUri };
        var tokenClient = new TokenClient(_configMock.Object, httpClient);
        // Set up configuration mock.
        
        
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
        _configMock.Verify(c => c["PetFinder:ClientId"], Times.Exactly(1));
        _configMock.Verify(c => c["PetFinder:ClientSecret"], Times.Exactly(1));
        // Http client gets called only once.
        Assert.Equal(1, _handlerMock.GetMatchCount(request));
    }
    
}