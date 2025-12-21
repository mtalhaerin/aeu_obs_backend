using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.CQRS._Generic.Response.FailResponse
{


    /// <summary>
    /// Represents a failed response for a CQRS operation.
    /// </summary>
    /// <typeparam name="T">The type of the response data.</typeparam>
    public class FailResponse<T> : BaseResponse<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseResponseFail{T}"/> class.
        /// </summary>
        public FailResponse() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseResponseFail{T}"/> class with a message and status code.
        /// </summary>
        /// <param name="message">The failure message.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        public FailResponse(string message, int statusCode) : base(false, message, statusCode)
        {
        }
    }
}
