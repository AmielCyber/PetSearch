using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using Moq;
using PetSearch.API.Clients;
using PetSearch.API.Configurations;
using PetSearch.API.Problems;
using PetSearch.API.Exceptions;
using PetSearch.API.Models;
using PetSearch.API.Tests.Data;
using RichardSzalay.MockHttp;

namespace PetSearch.API.Tests.Clients;

[TestSubject(typeof(MapBoxClient))]
public class MapBoxClientTest
{
    private const string DefaultLocationName = "San Diego";
    private const string DefaultZipCode = "92101";
    private const double DefaultLatitude = 32.7270;
    private const double DefaultLongitude = 117.1647;

    private readonly Uri _mapBoxUri;
    private readonly MockHttpMessageHandler _mockHttp;
    private readonly Mock<IOptions<MapBoxConfiguration>> _mapBoxOptionsMock;
    private readonly string _accessTokenValue;
    private readonly LocationDto _expectedDefaultLocationDto;
    private readonly IExpectedProblems _expectedProblems;

    public MapBoxClientTest()
    {
        _mapBoxUri = new Uri(MapBoxConfiguration.Url);
        _mockHttp = new MockHttpMessageHandler();
        _expectedDefaultLocationDto = new LocationDto(DefaultZipCode, DefaultLocationName);

        _accessTokenValue = new Guid().ToString();
        var mapBoxConfiguration = new MapBoxConfiguration
        {
            AccessToken = _accessTokenValue
        };
        _mapBoxOptionsMock = new Mock<IOptions<MapBoxConfiguration>>();
        _mapBoxOptionsMock.Setup(options => options.Value).Returns(mapBoxConfiguration);
        _expectedProblems = new MapBoxProblems();
    }

    [Fact]
    public async Task GetLocationFromZipCode_ShouldReturnALocationDto_IfZipcodeIsValid()
    {
        // Arrange
        var request = SetZipcodeRequest(_mockHttp, HttpStatusCode.Accepted);

        HttpClient mockHttpClient = _mockHttp.ToHttpClient();
        mockHttpClient.BaseAddress = _mapBoxUri;


        var mapBoxClient = new MapBoxClient(mockHttpClient, _mapBoxOptionsMock.Object, _expectedProblems);

        // Act
        var result = await mapBoxClient.GetLocationFromZipCode(DefaultZipCode);
        LocationDto? locationDto = Assert.IsType<Ok<LocationDto>>(result.Result).Value;
        // Assert
        Assert.NotNull(locationDto);
        Assert.IsType<LocationDto>(locationDto);
        Assert.Equal(_expectedDefaultLocationDto, locationDto);
        Assert.Equal(1, _mockHttp.GetMatchCount(request));
    }

    [Fact]
    public async Task GetLocationFromZipCode_ShouldCallMapBoxUrlFromMapBoxConfigUrl()
    {
        // Arrange
        IMockedRequest request = SetZipcodeRequest(_mockHttp, HttpStatusCode.Accepted);

        HttpClient mockHttpClient = _mockHttp.ToHttpClient();
        mockHttpClient.BaseAddress = _mapBoxUri;

        var mapBoxClient = new MapBoxClient(mockHttpClient, _mapBoxOptionsMock.Object, _expectedProblems);

        // Act
        await mapBoxClient.GetLocationFromZipCode(DefaultZipCode);

        // Assert
        Assert.Equal(1, _mockHttp.GetMatchCount(request));
        _mockHttp.Expect(HttpMethod.Get, MapBoxConfiguration.Url);
    }

    [Fact]
    public async Task GetLocationFromZipCode_ShouldCallMapBoxUrlWithAccessToken()
    {
        // Arrange
        IMockedRequest request = SetZipcodeRequest(_mockHttp, HttpStatusCode.Accepted);

        HttpClient mockHttpClient = _mockHttp.ToHttpClient();
        mockHttpClient.BaseAddress = _mapBoxUri;

        var mapBoxClient = new MapBoxClient(mockHttpClient, _mapBoxOptionsMock.Object, _expectedProblems);

        // Act
        await mapBoxClient.GetLocationFromZipCode(DefaultZipCode);

        // Assert
        Assert.Equal(1, _mockHttp.GetMatchCount(request));
        _mockHttp.Expect(MapBoxConfiguration.Url)
            .WithQueryString("access_token", _accessTokenValue);
    }

    [Fact]
    public async Task GetLocationFromZipCode_ShouldCallMapBoxUrlWithPassedZipcode()
    {
        // Arrange
        IMockedRequest request = SetZipcodeRequest(_mockHttp, HttpStatusCode.Accepted);

        HttpClient mockHttpClient = _mockHttp.ToHttpClient();
        mockHttpClient.BaseAddress = _mapBoxUri;

        var mapBoxClient = new MapBoxClient(mockHttpClient, _mapBoxOptionsMock.Object, _expectedProblems);

        // Act
        await mapBoxClient.GetLocationFromZipCode(DefaultZipCode);

        // Assert
        Assert.Equal(1, _mockHttp.GetMatchCount(request));
        _mockHttp.Expect(MapBoxConfiguration.Url)
            .WithContent(DefaultZipCode);
    }

    [Theory]
    [ClassData(typeof(MapBoxClientProblemsData))]
    public async Task GetLocationFromZipCode_ShouldReturnCorrectError_IfResultIsUnsuccessful(
        HttpStatusCode statusCode
    )
    {
        // Arrange
        SetZipcodeRequest(_mockHttp, statusCode);

        HttpClient mockHttpClient = _mockHttp.ToHttpClient();
        mockHttpClient.BaseAddress = _mapBoxUri;

        var mapBoxClient = new MapBoxClient(mockHttpClient, _mapBoxOptionsMock.Object, _expectedProblems);

        // Act
        Results<Ok<LocationDto>, ProblemHttpResult> result = await mapBoxClient.GetLocationFromZipCode(DefaultZipCode);

        ProblemHttpResult problemResult = Assert.IsType<ProblemHttpResult>(result.Result);
        // Assert
        _mockHttp.Expect(MapBoxConfiguration.Url).Respond(statusCode);
        Assert.Equal((int)statusCode, problemResult.StatusCode);
    }

    [Fact]
    public async Task GetLocationFromZipCode_ShouldThrowForbiddenAccess_IfApiStatusCodeIs403()
    {
        // Arrange
        SetZipcodeRequest(_mockHttp, HttpStatusCode.Forbidden);

        HttpClient mockHttpClient = _mockHttp.ToHttpClient();
        mockHttpClient.BaseAddress = _mapBoxUri;

        var mapBoxClient = new MapBoxClient(mockHttpClient, _mapBoxOptionsMock.Object, _expectedProblems);

        // Act
        await Assert.ThrowsAsync<ForbiddenAccessException>(
            async () => await mapBoxClient.GetLocationFromZipCode(DefaultZipCode)
        );
    }

    [Fact]
    public async Task GetLocationFromCoordinates_ShouldReturnALocationDto()
    {
        // Arrange
        var request = SetCoordinatesRequest(_mockHttp, HttpStatusCode.Accepted);

        HttpClient mockHttpClient = _mockHttp.ToHttpClient();
        mockHttpClient.BaseAddress = _mapBoxUri;

        var mapBoxClient = new MapBoxClient(mockHttpClient, _mapBoxOptionsMock.Object, _expectedProblems);

        // Act
        Results<Ok<LocationDto>, ProblemHttpResult> result =
            await mapBoxClient.GetLocationFromCoordinates(DefaultLongitude, DefaultLatitude);
        LocationDto? locationDto = Assert.IsType<Ok<LocationDto>>(result.Result).Value;

        // Assert
        Assert.NotNull(locationDto);
        Assert.IsType<LocationDto>(locationDto);
        Assert.Equal(_expectedDefaultLocationDto, locationDto);
        Assert.Equal(1, _mockHttp.GetMatchCount(request));
    }

    [Fact]
    public async Task GetLocationFromCoordinates_ShouldCallMapBoxUrlFromMapBoxConfigUrl()
    {
        // Arrange
        IMockedRequest request = SetCoordinatesRequest(_mockHttp, HttpStatusCode.Accepted);

        HttpClient mockHttpClient = _mockHttp.ToHttpClient();
        mockHttpClient.BaseAddress = _mapBoxUri;

        var mapBoxClient = new MapBoxClient(mockHttpClient, _mapBoxOptionsMock.Object, _expectedProblems);

        // Act
        await mapBoxClient.GetLocationFromCoordinates(DefaultLongitude, DefaultLatitude);

        // Assert
        Assert.Equal(1, _mockHttp.GetMatchCount(request));
        _mockHttp.Expect(MapBoxConfiguration.Url);
    }

    [Fact]
    public async Task GetLocationFromCoordinates_ShouldCallMapBoxUrlWithAccessToken()
    {
        // Arrange
        IMockedRequest request = SetCoordinatesRequest(_mockHttp, HttpStatusCode.Accepted);

        HttpClient mockHttpClient = _mockHttp.ToHttpClient();
        mockHttpClient.BaseAddress = _mapBoxUri;

        var mapBoxClient = new MapBoxClient(mockHttpClient, _mapBoxOptionsMock.Object, _expectedProblems);

        // Act
        await mapBoxClient.GetLocationFromCoordinates(DefaultLongitude, DefaultLatitude);

        // Assert
        Assert.Equal(1, _mockHttp.GetMatchCount(request));
        _mockHttp.Expect(MapBoxConfiguration.Url)
            .WithQueryString("access_token", _accessTokenValue);
    }

    [Fact]
    public async Task GetLocationFromCoordinates_ShouldCallMapBoxUrlWithPassedCoordinates()
    {
        // Arrange
        IMockedRequest request = SetCoordinatesRequest(_mockHttp, HttpStatusCode.Accepted);

        HttpClient mockHttpClient = _mockHttp.ToHttpClient();
        mockHttpClient.BaseAddress = _mapBoxUri;

        var mapBoxClient = new MapBoxClient(mockHttpClient, _mapBoxOptionsMock.Object, _expectedProblems);

        // Act
        await mapBoxClient.GetLocationFromCoordinates(DefaultLongitude, DefaultLatitude);

        // Assert
        Assert.Equal(1, _mockHttp.GetMatchCount(request));
        _mockHttp.Expect(MapBoxConfiguration.Url)
            .WithContent(DefaultLongitude.ToString(CultureInfo.InvariantCulture))
            .WithContent(DefaultLatitude.ToString(CultureInfo.InvariantCulture));
    }

    [Theory]
    [ClassData(typeof(MapBoxClientProblemsData))]
    public async Task GetLocationFromCoordinates_ShouldReturnCorrectError_IfResultIsUnsuccessful(
        HttpStatusCode statusCode
    )
    {
        // Arrange
        SetCoordinatesRequest(_mockHttp, statusCode);

        HttpClient mockHttpClient = _mockHttp.ToHttpClient();
        mockHttpClient.BaseAddress = _mapBoxUri;

        var mapBoxClient = new MapBoxClient(mockHttpClient, _mapBoxOptionsMock.Object, _expectedProblems);

        // Act
        Results<Ok<LocationDto>, ProblemHttpResult> result =
            await mapBoxClient.GetLocationFromCoordinates(DefaultLongitude, DefaultLatitude);

        ProblemHttpResult problemResult = Assert.IsType<ProblemHttpResult>(result.Result);

        // Assert
        _mockHttp.Expect(MapBoxConfiguration.Url).Respond(statusCode);
        Assert.Equal((int)statusCode, problemResult.StatusCode);
    }

    [Fact]
    public async Task GetLocationFromCoordinates_ShouldThrowForbiddenAccess_IfApiStatusCodeIs403()
    {
        // Arrange
        SetCoordinatesRequest(_mockHttp, HttpStatusCode.Forbidden);

        HttpClient mockHttpClient = _mockHttp.ToHttpClient();
        mockHttpClient.BaseAddress = _mapBoxUri;

        var mapBoxClient = new MapBoxClient(mockHttpClient, _mapBoxOptionsMock.Object, _expectedProblems);

        // Act
        await Assert.ThrowsAsync<ForbiddenAccessException>(
            async () => await mapBoxClient.GetLocationFromCoordinates(DefaultLongitude, DefaultLatitude)
        );
    }

    private IMockedRequest SetZipcodeRequest(MockHttpMessageHandler mockHttpMessageHandler, HttpStatusCode statusCode)
    {
        if (statusCode == HttpStatusCode.Accepted)
        {
            return
                mockHttpMessageHandler
                    .When($"{MapBoxConfiguration.Url}{DefaultZipCode}*")
                    .Respond(HttpStatusCode.Accepted, JsonContent.Create(new
                    {
                        features = new List<object>()
                        {
                            new
                            {
                                text_en = DefaultZipCode,
                                place_name_en = DefaultLocationName
                            }
                        },
                    }));
        }

        return mockHttpMessageHandler
            .When($"{MapBoxConfiguration.Url}{DefaultZipCode}*")
            .Respond(statusCode);
    }

    private IMockedRequest SetCoordinatesRequest(MockHttpMessageHandler mockHttpMessageHandler,
        HttpStatusCode statusCode)
    {
        if (statusCode == HttpStatusCode.Accepted)
        {
            return
                mockHttpMessageHandler
                    .When($"{MapBoxConfiguration.Url}{DefaultLongitude},{DefaultLatitude}*")
                    .Respond(HttpStatusCode.Accepted, JsonContent.Create(new
                    {
                        features = new List<object>()
                        {
                            new
                            {
                                text_en = DefaultZipCode,
                                place_name_en = DefaultLocationName
                            }
                        },
                    }));
        }

        return mockHttpMessageHandler
            .When($"{MapBoxConfiguration.Url}{DefaultLongitude},{DefaultLatitude}*")
            .Respond(statusCode);
    }
}