using ErrorOr;

namespace PetSearch.API.Errors;

public static partial class Errors
{
    /// <summary>
    /// Error types after or before sending a request to the PetFinder API.
    /// </summary>
    public static class Pets
    {
        /// <summary>
        /// Bad Request due to validation error. 
        /// </summary>
        public static Error BadRequest => Error.Validation(
            code: "Pets.BadRequest",
            description: "Invalid parameters entered."
        );

        /// <summary>
        /// Not Found Error.
        /// </summary>
        public static Error NotFound => Error.NotFound(
            code: "Pets.NotFound",
            description: "Pet with the passed id was not found."
        );

        /// <summary>
        /// Server Error.
        /// </summary>
        public static Error ServerError => Error.Failure(
            code: "Pets.ServerError",
            description: "Something went wrong with your request. Please try again."
        );
    }
}