using Core.DataAccess;
using Core.DataAccess.EntityFramework;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete.FakulteEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework.FakulteDal
{
    public interface IAnaDalDal : IEntityRepository<AnaDal>
    {
    }
    public class EFAnaDalDal : EfEntityRepositoryBase<AnaDal, AEUContext>, IAnaDalDal
    {
    }
}
