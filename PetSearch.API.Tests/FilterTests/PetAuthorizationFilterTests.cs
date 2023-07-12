using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Moq;
using PetSearchAPI.Filters;

namespace PetSearchAPI.Tests.FilterTests;

public class PetAuthorizationFilterTests
{
    private readonly Mock<AuthorizationFilterContext> _contextMock;

    public PetAuthorizationFilterTests()
    {
        _contextMock = new Mock<AuthorizationFilterContext>();
    }

    [Fact]
    public void ShouldHaveAnUnauthorizedResult_IfAuthorizationTokenIsMissing()
    {
        // Arrange
        var emptyStringValues = StringValues.Empty;
        var httpContextMock = new Mock<HttpContext>();
        httpContextMock
            .Setup(a => a.Request.Headers["Authorization"])
            .Returns(emptyStringValues);
        
        httpContextMock
            .Setup(a => a.Request.Headers.ContainsKey("Authorization"))
            .Returns(false);
        
        ActionContext actionContextMock =
            new ActionContext(httpContextMock.Object, 
                new Microsoft.AspNetCore.Routing.RouteData(), 
                new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());
        
        var authorizationFilterContext = new AuthorizationFilterContext(actionContextMock, new List<IFilterMetadata>());
        
        var petAuthorizationFilter = new PetAuthorizationFilter();
        
        // Act.
        petAuthorizationFilter.OnAuthorization(authorizationFilterContext);
        
        // Assert
        Assert.IsType<UnauthorizedResult>(authorizationFilterContext.Result);
    }
    
    [Fact]
    public void ShouldHaveAnUnauthorizedResult_IfAuthorizationTokenIsAnEmptyString()
    {
        // Arrange
        var httpContextMock = new Mock<HttpContext>();
        
        httpContextMock
            .Setup(a => a.Request.Headers["Authorization"])
            .Returns(" ");
        
        ActionContext actionContextMock =
            new ActionContext(httpContextMock.Object, 
                new Microsoft.AspNetCore.Routing.RouteData(), 
                new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());
        
        var authorizationFilterContext = new AuthorizationFilterContext(actionContextMock, new List<IFilterMetadata>());
        
        var petAuthorizationFilter = new PetAuthorizationFilter();
        
        // Act.
        petAuthorizationFilter.OnAuthorization(authorizationFilterContext);
        
        // Assert
        Assert.IsType<UnauthorizedResult>(authorizationFilterContext.Result);
    }
    [Fact]
    public void ShouldNotHaveAnUnauthorizedResult_IfAuthorizationTokenIsValid()
    {
        // Arrange
        var httpContextMock = new Mock<HttpContext>();
        
        httpContextMock
            .Setup(a => a.Request.Headers["Authorization"])
            .Returns("token mock");
        
        ActionContext actionContextMock =
            new ActionContext(httpContextMock.Object, 
                new Microsoft.AspNetCore.Routing.RouteData(), 
                new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());
        
        var authorizationFilterContext = new AuthorizationFilterContext(actionContextMock, new List<IFilterMetadata>());
        
        var petAuthorizationFilter = new PetAuthorizationFilter();
        
        // Act.
        petAuthorizationFilter.OnAuthorization(authorizationFilterContext);
        
        // Assert
        Assert.IsNotType<UnauthorizedResult>(authorizationFilterContext.Result?.GetType());
    }

}