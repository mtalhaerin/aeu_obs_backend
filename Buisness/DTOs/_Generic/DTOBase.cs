namespace Business.DTOs._Generic
{
    /// <summary>
    /// Abstract base class for all Data Transfer Objects (DTOs).
    /// <para>
    /// This class is typically used as a base for mapping incoming commands to DTOs.
    /// In some scenarios, after mapping, additional properties related to internal usage may be included in derived classes for internal processing.
    /// </para>
    /// </summary>
    public abstract class DTOBase : IDTOBase
    {
    }

    /// <summary>
    /// Abstract base class for request DTOs.
    /// </summary>
    public abstract class RequestDTOoBase : DTOBase, IRequestDTOBase
    {
    }

    /// <summary>
    /// Abstract base class for response DTOs.
    /// </summary>
    public abstract class ResponseDTOBase : DTOBase, IResponseDTOBase
    {
    }

    public abstract class CommandResponseDTOBase : ResponseDTOBase, ICommandResponseDTOBase
    {
    }

    public abstract class QueryResponseDTOBase : ResponseDTOBase, IQueryResponseDTOBase
    {
    }
}
