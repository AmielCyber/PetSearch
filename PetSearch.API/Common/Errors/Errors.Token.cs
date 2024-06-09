using ErrorOr;

namespace PetSearch.API.Common.Errors;

public static partial class Errors
{
    /// <summary>
    /// Token error types while accessing an endpoint that requires an access token.
    /// </summary>
    public static class Token
    {
        public static Error Unauthorized => Error.Unauthorized(
            code: "Token.NotAuthorized",
            description: "Access token is invalid or expired."
        );
    }
}