using Microsoft.Extensions.Configuration;
using PetSearchAPI.Clients;
using PetSearchAPI.Common.Exceptions;
using PetSearchAPI.Models.Token;
using Shouldly;

namespace PetSearchAPI.Tests;

public class TokenClientTests {
    private const string PetFinderTokenUrl = "https://api.petfinder.com/v2/oauth2/token";
    private IConfiguration Configuration { get; set; }
    private static readonly HttpClient Client = new HttpClient();

    public TokenClientTests()
    {
        IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
            .AddUserSecrets<TokenClientTests>();

        Configuration = configurationBuilder.Build();
        Client.BaseAddress = new Uri(PetFinderTokenUrl);
    }
    
    [Fact]
    public async Task Should_Return_Token()
    {
        // Arrange.
        var tokenClient = new TokenClient(Configuration, Client);
        // Act.
        TokenResponseDto? request = await tokenClient.GetToken();
        
        // Assert.
        request.ShouldNotBeNull();
        request.TokenType.ShouldBe("Bearer");
        request.ExpiresIn.ShouldBe(3600);
        request.AccessToken.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Return_Token_Fetch_Exception_With_Invalid_Config_Keys()
    {
        // Arrange.
        var emptyConfig = new ConfigurationBuilder().Build();
        
        // Act and Assert.
        await Assert.ThrowsAsync<TokenFetchException>(async () =>
        {
            var tokenClient = new TokenClient(emptyConfig, Client);
            await tokenClient.GetToken();
        });
    }
    
}