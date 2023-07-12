using ErrorOr;

namespace PetSearch.API.Common.Errors;

public static partial class Errors
{
    /// <summary>
    /// Error types after or before sending a request to the PetFinder API.
    /// </summary>
    public static class Pets
    {
        // 400
        public static Error BadRequest => Error.Validation(
            code: "Pets.BadRequest",
            description: "Invalid parameters entered."
        );

        // 404
        public static Error NotFound => Error.NotFound(
            code: "Pets.NotFound",
            description: "Pet with the passed id was not found."
        );

        // 500
        public static Error ServerError => Error.Failure(
            code: "Pets.ServerError",
            description: "Something went wrong with your request."
        );
    }
}