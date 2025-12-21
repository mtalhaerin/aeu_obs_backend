using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Buisiness.Features.CQRS
{
    /// <summary>
    /// Represents a CQRS command that returns a response of type <typeparamref name="TResponse"/>.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response returned by the command.</typeparam>
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }

    /// <summary>
    /// Represents a CQRS command that does not return a value.
    /// </summary>
    public interface ICommand : IRequest
    {
    }
}
