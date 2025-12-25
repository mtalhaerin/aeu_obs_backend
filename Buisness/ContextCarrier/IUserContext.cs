using Business.ContextCarrier._Generic;
using Core.Entities.Concrete.OzlukEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ContextCarrier
{
    public interface IUserContext : ICarrierContext
    {
        Kullanici CurrentUser { get; set; }
        Guid UserUuid { get; set; }
        string Token { get; set; }
    }

    public class UserContext : IUserContext
    {
        public Kullanici CurrentUser { get; set; } = default!;
        public Guid UserUuid { get; set; } = Guid.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
