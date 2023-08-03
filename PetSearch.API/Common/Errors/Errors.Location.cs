using ErrorOr;

namespace PetSearch.API.Common.Errors;

public static partial class Errors
{
    /// <summary>
    /// Error types after or before sending a request to the PetFinder API.
    /// </summary>
    public static class Location
    {
        // 400
        public static Error BadRequest => Error.Validation(
            code: "Location.BadRequest",
            description: "Invalid parameters entered."
        );

        // 404
        public static Error NotFound => Error.NotFound(
            code: "Location.NotFound",
            description: "Location was not found. Please enter a valid zip code or coordinates"
        );

        // 500
        public static Error ServerError => Error.Failure(
            code: "Location.ServerError",
            description: "Something went wrong with your request."
        );
    }
}
