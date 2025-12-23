using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.CQRS._Generic.Helpers
{
    public interface IGenericHelper
    {
        string? GetAccessTokenFromHeader();
    }
}
