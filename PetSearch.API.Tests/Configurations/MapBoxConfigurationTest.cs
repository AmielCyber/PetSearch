using JetBrains.Annotations;
using PetSearch.API.Configurations;

namespace PetSearch.API.Tests.Configurations;

[TestSubject(typeof(MapBoxConfiguration))]
public class MapBoxConfigurationTest
{
    [Fact]
    public void SetsTheQueryString_WhenTheAccessTokenIsInitialized()
    {
        // Arrange and Action.
        var config = new MapBoxConfiguration()
        {
            AccessToken = "token"
        };

        // Assert.
        Assert.StartsWith("?", config.OptionsQuery);
    }

    [Fact]
    public void IncludesTheAccessTokenInTheOptionsQuery_AfterConstruction()
    {
        // Arrange
        var uniqueId = new Guid().ToString();
        var expectedToken = $"access_token={uniqueId}";

        // Action
        var config = new MapBoxConfiguration()
        {
            AccessToken = uniqueId
        };

        // Assert.
        Assert.Contains(expectedToken, config.OptionsQuery);
    }
}