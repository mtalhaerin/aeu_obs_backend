namespace Business.Features.CQRS._Generic.Response.FailResponse
{
    /// <summary>
    /// Represents a failed response with error details for a CQRS operation.
    /// </summary>
    /// <typeparam name="T">The type of the response data.</typeparam>
    public class FailResponseWithErrors<T> : FailResponse<T>
    {
        /// <summary>
        /// Gets or sets the list of error messages.
        /// </summary>
        public List<string> Errors { get; set; } = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="FailResponseWithErrors{T}"/> class.
        /// </summary>
        public FailResponseWithErrors() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FailResponseWithErrors{T}"/> class with a message and status code.
        /// </summary>
        /// <param name="message">The failure message.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        public FailResponseWithErrors(string message, int statusCode) : base(message, statusCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FailResponseWithErrors{T}"/> class with errors, a message, and status code.
        /// </summary>
        /// <param name="errors">A list of error messages.</param>
        /// <param name="message">The failure message.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        public FailResponseWithErrors(List<string>? errors, string message, int statusCode) : base(message, statusCode)
        {

        }
    }
}
