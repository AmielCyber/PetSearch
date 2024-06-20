using System.Net;
using System.Runtime.InteropServices.JavaScript;
using ErrorOr;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http.HttpResults;
using PetSearch.API.Handlers;
using PetSearch.API.Tests.Data;

namespace PetSearch.API.Tests.Handlers;

public class BaseHandlerSubClass : BaseHandler
{
    public ProblemHttpResult GetProblem(Error error) => GetProblemHttpResult(error);
}

[TestSubject(typeof(BaseHandler))]
public class BaseHandlerTest
{
    private BaseHandlerSubClass _baseHandler;

    public BaseHandlerTest()
    {
        _baseHandler = new BaseHandlerSubClass();
    }

    [Fact]
    public void GetProblemHttpResult_ReturnsA_ProblemHttpResult()
    {
        // Arrange
        var error = Error.Failure();

        // Action and Assert
        Assert.IsType<ProblemHttpResult>(_baseHandler.GetProblem(error));
    }

    [Theory]
    [ClassData(typeof(BaseHandlerErrorData))]
    public void GetProblemHttpResult_ReturnsA_BadRequestProblemHttpResult_WithAValidationError(Error error,
        HttpStatusCode expectedCode)
    {
        // Action
        var result = _baseHandler.GetProblem(error);
        // Assert
        Assert.Equal((int)expectedCode, result.StatusCode);
    }
}