using Core.DataAccess;
using Core.DataAccess.EntityFramework;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete.OzlukEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework.OzlukDals
{
    public interface IEpostaDal : IEntityRepository<Eposta>
    {
    }
    public class EFEpostaDal : EfEntityRepositoryBase<Eposta, AEUContext>, IEpostaDal
    {
    }
}
