using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs._Generic
{

    /// <summary>
    /// Marker interface for all Data Transfer Objects (DTOs).
    /// </summary>
    public interface IDTOBase
    {
    }

    /// <summary>
    /// Marker interface for request DTOs.
    /// </summary>
    public interface IRequestDTOBase : IDTOBase
    {
        // Marker interface for request DTOs
    }

    /// <summary>
    /// Marker interface for response DTOs.
    /// </summary>
    public interface IResponseDTOBase : IDTOBase
    {
        // Marker interface for response DTOs
    }

    public interface ICommandResponseDTOBase : IResponseDTOBase
    {
        // Marker interface for command response DTOs
    }

    public interface IQueryResponseDTOBase : IResponseDTOBase
    {
        // Marker interface for query response DTOs
    }
}
