using Core.DataAccess;
using Core.DataAccess.EntityFramework;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete.FakulteEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework.FakulteDals
{
    public interface IAkademisyenBolumAtamaDal : IEntityRepository<AkademisyenBolumAtama>
    {
    }
    public class EFAkademisyenBolumAtamaDal : EfEntityRepositoryBase<AkademisyenBolumAtama, AEUContext>, IAkademisyenBolumAtamaDal
    {
    }
}
