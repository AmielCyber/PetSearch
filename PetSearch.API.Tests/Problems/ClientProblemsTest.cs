using System.Net;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using PetSearch.API.Problems;
using PetSearch.API.Exceptions;

namespace PetSearch.API.Tests.Problems;

[TestSubject(typeof(ClientProblems))]
public class ClientProblemsTest
{
    private readonly Mock<IExpectedProblems> _expectedProblems;

    public ClientProblemsTest()
    {
        _expectedProblems = new Mock<IExpectedProblems>();
    }

    [Fact]
    public void GetProblemHttpResult_ThrowsForbiddenException_WhenResponseIsForbidden()
    {
        const string forbiddenMessage = "Forbidden";
        _expectedProblems.SetupGet(x => x.ForbiddenErrorMessage).Returns(forbiddenMessage);

        Assert.Throws<ForbiddenAccessException>(() =>
            ClientProblems.GetProblemHttpResult(HttpStatusCode.Forbidden, _expectedProblems.Object));
    }

    [Fact]
    public void GetProblemHttpResult_ReturnsInternalServerError_WhenResponseIsUnexpected()
    {
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
        _expectedProblems.Setup(e => e.IsExpectedErrorCode(statusCode)).Returns(false);

        ProblemHttpResult result = ClientProblems.GetProblemHttpResult(statusCode, _expectedProblems.Object);

        Assert.Equal(500, result.StatusCode);
    }

    [Fact]
    public void GetProblemHttpResult_ReturnsDetails_WhenResponseIsExpected()
    {
        const string expectedMessage = "Problem Details Test";
        HttpStatusCode statusCode = HttpStatusCode.BadRequest;
        _expectedProblems.Setup(e => e.IsExpectedErrorCode(statusCode)).Returns(true);
        _expectedProblems.Setup(e => e.GetProblemDetails(statusCode)).Returns(expectedMessage);

        ProblemHttpResult result = ClientProblems.GetProblemHttpResult(statusCode, _expectedProblems.Object);

        Assert.Equal((int)statusCode, result.StatusCode);
        Assert.Contains(expectedMessage, result.ProblemDetails.Detail);
    }
}