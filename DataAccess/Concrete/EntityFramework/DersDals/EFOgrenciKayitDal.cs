using Core.DataAccess;
using Core.DataAccess.EntityFramework;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete.DersEntities;

namespace DataAccess.Concrete.EntityFramework.DersDals
{
    public interface IOgrenciDersKayitDal : IEntityRepository<OgrenciDersKayit>
    {
    }
    public class EFOgrenciKayitDal : EfEntityRepositoryBase<OgrenciDersKayit, AEUContext>, IOgrenciDersKayitDal
    {
    }
}
