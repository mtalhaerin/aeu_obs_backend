using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.CQRS._Generic
{
    /// <summary>
    /// Represents a CQRS query that returns a response of type <typeparamref name="TResponse"/>.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response returned by the query.</typeparam>
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }
}
