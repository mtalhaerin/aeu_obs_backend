using Core.DataAccess;
using Core.DataAccess.EntityFramework;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public interface ITelefonDal : IEntityRepository<Telefon>
    {
    }
    public class EFTelefonDal : EfEntityRepositoryBase<Telefon, AEUContext>, ITelefonDal
    {
    }
}
