using System.Net;
using System.Net.Http.Json;
using ErrorOr;
using Microsoft.Extensions.Options;
using Moq;
using PetSearch.API.Clients;
using PetSearch.API.Common.Errors;
using PetSearch.API.Common.Exceptions;
using PetSearch.API.Models.MabBoxResponse;
using PetSearch.API.StronglyTypedConfigurations;
using RichardSzalay.MockHttp;

namespace PetSearch.API.Tests.MapBoxClientTests;

public class GetLocationFromCoordinatesTests
{
    
    private const string DefaultLocationName = "San Diego";
    private const string DefaultZipCode = "92101";
    private const double DefaultLatitude = 32.7270;
    private const double DefaultLongitude = 117.1647;
    private const string MapBoxUrl = "https://api.mapbox.com/geocoding/v5/mapbox.places/";
    
    private readonly Uri _mapBoxUri;
    private readonly MockHttpMessageHandler _handlerMock;
    private readonly Mock<IOptions<MapBoxConfiguration>> _mapBoxOptionsMock;
    private readonly MapBoxConfiguration _mapBoxConfiguration;
    private readonly LocationDto _expectedDefaultLocationDto;

    public GetLocationFromCoordinatesTests()
    {
        _mapBoxUri = new Uri(MapBoxUrl);
        _handlerMock = new MockHttpMessageHandler();
        _expectedDefaultLocationDto = new LocationDto(DefaultZipCode, DefaultLocationName);
        _mapBoxConfiguration = new MapBoxConfiguration
        {
            AccessToken = "Token"
        };
        _mapBoxOptionsMock = new Mock<IOptions<MapBoxConfiguration>>();
    }
    
    
    [Fact]
    public async Task ShouldReturnALocationDto_IfCoordinatesAreValid()
    {
        // Arrange
        var request = _handlerMock
            .When($"{MapBoxUrl}{DefaultLongitude},{DefaultLatitude}*")
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
        using var httpClient = new HttpClient(_handlerMock) { BaseAddress = _mapBoxUri };
        _mapBoxOptionsMock.Setup(options => options.Value).Returns(_mapBoxConfiguration);
        var mapBoxClient = new MapBoxClient(httpClient, _mapBoxOptionsMock.Object);
        
        // Act
        var result = await mapBoxClient.GetLocationFromCoordinates(DefaultLongitude, DefaultLatitude);
        var locationDto = result.Value;
        
        
        // Assert
        Assert.NotNull(locationDto);
        Assert.IsType<LocationDto>(locationDto);
        Assert.Equal(_expectedDefaultLocationDto, locationDto);
        Assert.Equal(1, _handlerMock.GetMatchCount(request));
        _mapBoxOptionsMock.Verify(options => options.Value, Times.Once());
    }
    
    [Fact]
    public async Task ShouldReturnANotFoundError_CoordinatesNotFound()
    {
        // Arrange
        var request = _handlerMock
            .When($"{MapBoxUrl}{0},{0}*")
            .Respond(HttpStatusCode.Accepted, JsonContent.Create(new
            {
                // MapBox returns an empty list if zipcode is not found.
                features = new List<object>()
            }));
        using var httpClient = new HttpClient(_handlerMock) { BaseAddress = _mapBoxUri };
        _mapBoxOptionsMock.Setup(options => options.Value).Returns(_mapBoxConfiguration);
        var mapBoxClient = new MapBoxClient(httpClient, _mapBoxOptionsMock.Object);
        
        // Act
        var result = await mapBoxClient.GetLocationFromCoordinates(0,0);
        
        
        // Assert
        Assert.Contains(result.Errors, error => error == Errors.Location.NotFound);
        Assert.Equal(1, _handlerMock.GetMatchCount(request));
        _mapBoxOptionsMock.Verify(options => options.Value, Times.Once());
    }
    
    [Fact]
    public async Task ShouldThrowMapBoxForbiddenError_IfResponseStatusCodeIsForbidden()
    {
        // Arrange
        var request = _handlerMock
            .When($"{MapBoxUrl}{DefaultLongitude},{DefaultLatitude}*")
            .Respond(HttpStatusCode.Forbidden);
        using var httpClient = new HttpClient(_handlerMock) { BaseAddress = _mapBoxUri };
        _mapBoxOptionsMock.Setup(options => options.Value).Returns(_mapBoxConfiguration);
        var mapBoxClient = new MapBoxClient(httpClient, _mapBoxOptionsMock.Object);
        
        // Act and Assert.
        ErrorOr<LocationDto>? result = null;
        await Assert.ThrowsAsync<MapBoxForbidden>(async () =>
        {
            result = await mapBoxClient.GetLocationFromCoordinates(DefaultLongitude, DefaultLatitude);
        });
        Assert.Null(result);
        Assert.Equal(1, _handlerMock.GetMatchCount(request));
        _mapBoxOptionsMock.Verify(options => options.Value, Times.Once());
    }
    
    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task ShouldReturnError_WhenResponseIsUnsuccessful(HttpStatusCode responseStatusCode)
    {
        // Arrange
        var request = _handlerMock
            .When($"{MapBoxUrl}{DefaultLongitude},{DefaultLatitude}*")
            .Respond(responseStatusCode);
        using var httpClient = new HttpClient(_handlerMock) { BaseAddress = _mapBoxUri };
        _mapBoxOptionsMock.Setup(options => options.Value).Returns(_mapBoxConfiguration);
        var mapBoxClient = new MapBoxClient(httpClient, _mapBoxOptionsMock.Object);
        
        // Act
        var result = await mapBoxClient.GetLocationFromCoordinates(DefaultLongitude, DefaultLatitude);

        // Assert
        var expectedErrorType = MapBoxClientTestHelper.GetExpectedErrorType(responseStatusCode);
        Assert.Contains(result.Errors, error => error == expectedErrorType);
        Assert.Equal(1, _handlerMock.GetMatchCount(request));
        _mapBoxOptionsMock.Verify(options => options.Value, Times.Once());
    }

}