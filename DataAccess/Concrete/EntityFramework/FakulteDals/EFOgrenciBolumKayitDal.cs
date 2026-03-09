using Core.DataAccess;
using Core.DataAccess.EntityFramework;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete.FakulteEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework.FakulteDals
{
    public interface IOgrenciBolumKayitDal : IEntityRepository<OgrenciBolumKayit>
    {
        Task<(List<OgrenciBolumKayit>, int)> GetPagedAsync(
            Guid? kullaniciUuid,
            Guid? bolumUuid,
            DateTime? olusturmaTarihi,
            DateTime? guncellemeTarihi,
            int page,
            int pageSize);
    }

    public class EFOgrenciBolumKayitDal : EfEntityRepositoryBase<OgrenciBolumKayit, AEUContext>, IOgrenciBolumKayitDal
    {
        public async Task<(List<OgrenciBolumKayit>, int)> GetPagedAsync(
            Guid? kullaniciUuid,
            Guid? bolumUuid,
            DateTime? olusturmaTarihi,
            DateTime? guncellemeTarihi,
            int page,
            int pageSize)
        {
            using var context = new AEUContext();

            var query = context.OgrenciBolumKayitlari.AsQueryable();

            if (kullaniciUuid.HasValue && kullaniciUuid != Guid.Empty)
                query = query.Where(o => o.KullaniciUuid == kullaniciUuid.Value);

            if (bolumUuid.HasValue && bolumUuid != Guid.Empty)
                query = query.Where(o => o.BolumUuid == bolumUuid.Value);

            if (olusturmaTarihi.HasValue)
                query = query.Where(o => o.OlusturmaTarihi >= olusturmaTarihi.Value);

            if (guncellemeTarihi.HasValue)
                query = query.Where(o => o.GuncellemeTarihi <= guncellemeTarihi.Value);

            int totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
