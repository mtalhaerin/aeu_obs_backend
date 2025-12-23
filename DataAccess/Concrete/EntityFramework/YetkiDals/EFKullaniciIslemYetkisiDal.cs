using Core.DataAccess;
using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete.YetkiEntities;
using DataAccess.Concrete.EntityFramework.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework.YetkiDals
{
    public interface IKullaniciIslemYetkisiDal : IEntityRepository<KullaniciIslemYetkisi>
    {
    }
    public class EFKullaniciIslemYetkisiDal : EfEntityRepositoryBase<KullaniciIslemYetkisi, AEUContext>, IKullaniciIslemYetkisiDal
    {
    }
}
