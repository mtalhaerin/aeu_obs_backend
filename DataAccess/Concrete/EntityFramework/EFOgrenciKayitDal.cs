using Core.DataAccess;
using Core.DataAccess.EntityFramework;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework
{
    public interface IOgrenciDersKayitDal : IEntityRepository<OgrenciDersKayit>
    {
    }
    public class EFOgrenciKayitDal : EfEntityRepositoryBase<OgrenciDersKayit, AEUContext>, IOgrenciDersKayitDal
    {
    }
}
