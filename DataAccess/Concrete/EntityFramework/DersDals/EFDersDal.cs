using Core.DataAccess;
using Core.DataAccess.EntityFramework;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete.DersEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework.DersDals
{
    public interface IDersDal : IEntityRepository<Ders>
    {
    }
    public class EFDersDal : EfEntityRepositoryBase<Ders, AEUContext>, IDersDal
    {
    }
}
