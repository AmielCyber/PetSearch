using System.Net;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using PetSearch.API.Exceptions;
using PetSearch.API.Helpers;

namespace PetSearch.API.Tests.Helpers;

[TestSubject(typeof(MapBoxProblems))]
public class MapBoxProblemsTest
{
    private readonly double _longitude;
    private readonly double _latitude;
    private readonly string _zipcode;
    
    public MapBoxProblemsTest()
    {
        _longitude = -9.8066535;
        _latitude = 39.3700787;
        _zipcode = "12345";
    }
    
    [Fact]
    public void GetProblemFromZipcode_ShouldThrowForbiddenAccessException_IfResponseIsForbidden()
    {
        // Arrange
        HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.Forbidden);
        // Act and Assert
        Assert.Throws<ForbiddenAccessException>(() =>
            MapBoxProblems.GetProblemFromZipcode(httpResponseMessage, _zipcode));
    }

    [Fact]
    public void GetProblemFromZipcode_ShouldDisplayDetails_IfResponseIsNotFound()
    {
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
        // Act
        ProblemHttpResult problem = MapBoxProblems.GetProblemFromZipcode(httpResponseMessage, _zipcode);
        // Assert
        Assert.NotNull(problem.ProblemDetails.Detail);
        Assert.NotEqual(string.Empty, problem.ProblemDetails.Detail.Trim());
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.NotFound)]
    public void GetProblemFromZipcode_ShouldReturnTheCorrectProblemDetails_FromTheResponse(HttpStatusCode httpStatusCode)
    {
        // Arrange
        var httpResponseMessage = new HttpResponseMessage(httpStatusCode);
        // Act
        ProblemHttpResult problem = MapBoxProblems.GetProblemFromZipcode(httpResponseMessage, _zipcode);
        // Assert
        Assert.Equal((int)httpStatusCode, problem.StatusCode);
    }
    [Theory]
    [InlineData(HttpStatusCode.Conflict)]
    [InlineData(HttpStatusCode.BadGateway)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public void GetProblemFromZipcode_ShouldReturnServerError_FromAnyUnexpectedErrorResponse(HttpStatusCode httpStatusCode)
    {
        // Arrange
        var httpResponseMessage = new HttpResponseMessage(httpStatusCode);
        // Act
        ProblemHttpResult problem = MapBoxProblems.GetProblemFromZipcode(httpResponseMessage, _zipcode);
        // Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, problem.StatusCode);
    }

    [Fact]
    public void GetProblemFromCoords_ShouldThrowForbiddenAccessException_IfResponseIsForbidden()
    {
        // Arrange
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.Forbidden);
        // Act and Assert
        Assert.Throws<ForbiddenAccessException>(() => 
            MapBoxProblems.GetProblemFromCoords(httpResponseMessage, _longitude, _latitude));
    }
    
    [Fact]
    public void GetProblemFromCoords_ShouldDisplayDetails_IfResponseIsNotFound()
    {
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
        // Act
        ProblemHttpResult problem = MapBoxProblems.GetProblemFromCoords(httpResponseMessage, _longitude, _latitude);
        // Assert
        Assert.NotNull(problem.ProblemDetails.Detail);
        Assert.NotEqual(string.Empty, problem.ProblemDetails.Detail.Trim());
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.NotFound)]
    public void GetProblemFromCoords_ShouldReturnTheCorrectProblemDetails_FromTheResponse(HttpStatusCode httpStatusCode)
    {
        // Arrange
        var httpResponseMessage = new HttpResponseMessage(httpStatusCode);
        // Act
        ProblemHttpResult problem = MapBoxProblems.GetProblemFromCoords(httpResponseMessage, _longitude, _latitude);
        // Assert
        Assert.Equal((int)httpStatusCode, problem.StatusCode);
    }
    [Theory]
    [InlineData(HttpStatusCode.Conflict)]
    [InlineData(HttpStatusCode.BadGateway)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public void GetProblemFromCoords_ShouldReturnServerError_FromAnyUnexpectedErrorResponse(HttpStatusCode httpStatusCode)
    {
        // Arrange
        var httpResponseMessage = new HttpResponseMessage(httpStatusCode);
        // Act
        ProblemHttpResult problem = MapBoxProblems.GetProblemFromCoords(httpResponseMessage, _longitude, _latitude);
        // Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, problem.StatusCode);
    }
    
}