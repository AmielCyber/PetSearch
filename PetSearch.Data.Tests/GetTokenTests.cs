using System.Data.Common;
using System.Net;
using System.Net.Http.Json;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using PetSearch.Data.Common.Exceptions;
using PetSearch.Data.Entities;
using PetSearch.Data.Services;
using PetSearch.Data.StronglyTypedConfigurations;
using RichardSzalay.MockHttp;

namespace PetSearch.Data.Tests.TokenServiceTests;

public class GetTokenTests : IDisposable
{
    private const int TokenId = 1;
    private readonly DbConnection _connection;
    private readonly DbContextOptions<PetSearchContext> _contextOptions;
    private static readonly string TokenUrl = "https://api.petfinder.com/v2/oauth2/token";
    private readonly Uri _petFinderTokenUri;
    private readonly PetFinderConfiguration _petFinderConfiguration;
    private readonly Mock<IOptions<PetFinderConfiguration>> _petFinderOptionsMock;
    private readonly MockHttpMessageHandler _handlerMock;
    private readonly Token _expectedResponseToken;

    public GetTokenTests()
    {
        // FROM: https://learn.microsoft.com/en-us/ef/core/testing/testing-without-the-database#inmemory-provider
        // Create and open a connection. This creates the SQLite in-memory database,
        // which will persist until the connection is closed
        // at the end of the test (see Dispose below).
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        // These options will be used by the context instances in this test suite,
        // including the connection opened above.
        _contextOptions = new DbContextOptionsBuilder<PetSearchContext>()
            .UseSqlite(_connection)
            .Options;

        // Create the schema and seed some data
        using var context = new PetSearchContext(_contextOptions);

        context.Database.EnsureCreated();

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
        _expectedResponseToken = new Token { Id = TokenId, AccessToken = "TokenValue" };
    }

    [Fact]
    public async Task GetToken_ShouldCreateANewTokenInTheDatabase_AndReturnAToken()
    {
        // Arrange
        var context = CreateContext();
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
        var tokenClient = new TokenService(context, httpClient, _petFinderOptionsMock.Object);

        // Count the amount of tokens.
        int totalCount = await context.Tokens.Where(t => t.Id == TokenId).CountAsync();

        // Act
        var result = await tokenClient.GetToken();
        int resultCount = await context.Tokens.Where(t => t.Id == TokenId).CountAsync();

        // There are no rows at first.
        Assert.Equal(0, totalCount);
        // Check that there was a new row created.
        Assert.True(totalCount < resultCount);
        Assert.IsType<Token>(result);
        Assert.Equal(_expectedResponseToken.AccessToken, result.AccessToken);
        Assert.Equal(_expectedResponseToken.Id, result.Id);
        // Check that the expiration is in the future.
        Assert.True(result.ExpiresIn > DateTime.Now);
        // Verify calls are made to get our keys.
        _petFinderOptionsMock.Verify(options => options.Value, Times.Exactly(1));
        // Http client gets called only once when we call PetFinderApi to get a new access token.
        Assert.Equal(1, _handlerMock.GetMatchCount(request));
    }

    [Theory]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task GetToken_ShouldThrowTokenError_WhenResponseIsUnsuccessful(HttpStatusCode httpStatusCode)
    {
        // Arrange.
        var context = CreateContext();
        // Set up HttpMessageHandler mock for httpclient.
        var request = _handlerMock
            .When(TokenUrl)
            .Respond(httpStatusCode);
        // Arrange httpClient and tokenClient with mock objects.
        using var httpClient = new HttpClient(_handlerMock) { BaseAddress = _petFinderTokenUri };
        _petFinderOptionsMock.Setup(options => options.Value).Returns(_petFinderConfiguration);
        var tokenClient = new TokenService(context, httpClient, _petFinderOptionsMock.Object);

        // Assert
        Token? result = null;
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

    [Fact]
    public async Task GetToken_ShouldReturnTokenFromDb_WhenNonExpiredTokenIsInDb()
    {
        // Arrange
        var context = CreateContext();
        // Make sure there is one token row in our DB.
        Token expectedToken = await InsertTokenToDatabase(context, false);
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
        var tokenClient = new TokenService(context, httpClient, _petFinderOptionsMock.Object);

        // Count the amount of tokens.
        int totalCount = await context.Tokens.Where(t => t.Id == TokenId).CountAsync();

        // Act
        var result = await tokenClient.GetToken();
        int resultCount = await context.Tokens.Where(t => t.Id == TokenId).CountAsync();

        // Assert that there is one token in our DB.
        Assert.Equal(1, totalCount);
        // Assert that there was only an update to the token after calling GetToken.
        Assert.Equal(totalCount, resultCount);
        Assert.IsType<Token>(result);
        Assert.Equal(expectedToken.AccessToken, result.AccessToken);
        Assert.Equal(expectedToken.Id, result.Id);
        Assert.Equal(expectedToken.ExpiresIn, result.ExpiresIn);
        // Verify calls are made to get our keys.
        _petFinderOptionsMock.Verify(options => options.Value, Times.Exactly(1));
        // Http client does not get called since the token in our DB is not expired.
        Assert.Equal(0, _handlerMock.GetMatchCount(request));
    }

    [Fact]
    public async Task GetToken_ShouldFetchNewToken_WhenExpiredTokenIsInDb()
    {
        // Arrange
        var context = CreateContext();
        // Insert expired token in our database.
        Token expiredToken = await InsertTokenToDatabase(context, true);
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
        var tokenClient = new TokenService(context, httpClient, _petFinderOptionsMock.Object);

        // Count the amount of tokens.
        int totalCount = await context.Tokens.Where(t => t.Id == TokenId).CountAsync();

        // Act
        var result = await tokenClient.GetToken();
        int resultCount = await context.Tokens.Where(t => t.Id == TokenId).CountAsync();

        // Assert that there is one token in our DB.
        Assert.Equal(1, totalCount);
        // Assert that there was only an update to the token after calling GetToken.
        Assert.Equal(totalCount, resultCount);
        Assert.IsType<Token>(result);
        Assert.Equal(expiredToken.Id, result.Id);
        // Expired token is replaced.
        Assert.NotEqual(expiredToken.ExpiresIn, result.ExpiresIn);
        // Verify calls are made to get our keys.
        _petFinderOptionsMock.Verify(options => options.Value, Times.Exactly(1));
        // Http client does get called since Token in our DB is expired.
        Assert.Equal(1, _handlerMock.GetMatchCount(request));
    }

    private async Task<Token> InsertTokenToDatabase(PetSearchContext context, bool expiredToken)
    {
        Token token = new Token
        {
            Id = TokenId,
            AccessToken = "TokenValue",
            ExpiresIn = expiredToken ? DateTime.Now.AddMinutes(-5) : DateTime.Now.AddMinutes(55)
        };

        await context.Tokens.AddAsync(token);
        var saved = await context.SaveChangesAsync() > 0;

        if (!saved)
        {
            throw new TokenUpdateException("Failed to save changes.");
        }

        // Return new so it does not get tracked by EF.
        return new Token
        {
            Id = token.Id,
            AccessToken = token.AccessToken,
            ExpiresIn = token.ExpiresIn
        };
    }

    private PetSearchContext CreateContext() => new PetSearchContext(_contextOptions);

    public void Dispose() => _connection.Dispose();
}