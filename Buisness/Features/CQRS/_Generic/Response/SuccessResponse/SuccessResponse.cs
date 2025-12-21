using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.CQRS._Generic.Response.SuccessResponse
{
    internal class SuccessResponse
    {
    }

    /// <summary>
    /// Represents a successful response for a CQRS operation.
    /// </summary>
    /// <typeparam name="T">The type of the response data.</typeparam>
    public class SuccessResponse<T> : BaseResponse<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SuccessResponse{T}"/> class.
        /// </summary>
        public SuccessResponse() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SuccessResponse{T}"/> class with a message and status code.
        /// </summary>
        /// <param name="message">The success message.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        public SuccessResponse(string message, int statusCode) : base(true, message, statusCode)
        {
        }
    }
}
