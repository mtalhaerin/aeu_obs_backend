using Core.DataAccess;
using Core.DataAccess.EntityFramework;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework
{
    public interface IOgrenciKayitDal : IEntityRepository<OgrenciKayit>
    {
    }
    public class EFOgrenciKayitDal : EfEntityRepositoryBase<OgrenciKayit, AEUContext>, IOgrenciKayitDal
    {
    }
}
