namespace Business.Features.CQRS._Generic.Response.SuccessResponse
{
    /// <summary>
    /// Represents a successful response with data for a CQRS operation.
    /// </summary>
    /// <typeparam name="T">The type of the response data.</typeparam>
    public class SuccessResponseWithData<T> : SuccessResponse<T>
    {
        /// <summary>
        /// Gets or sets the response data.
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SuccessResponseWithData{T}"/> class.
        /// </summary>
        public SuccessResponseWithData() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SuccessResponseWithData{T}"/> class with a message and status code.
        /// </summary>
        /// <param name="message">The success message.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        public SuccessResponseWithData(string message, int statusCode) : base(message, statusCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SuccessResponseWithData{T}"/> class with data, a message, and status code.
        /// </summary>
        /// <param name="data">The response data.</param>
        /// <param name="message">The success message.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        public SuccessResponseWithData(T data, string message, int statusCode) : base(message, statusCode)
        {
            Data = data;
        }
    }
}
