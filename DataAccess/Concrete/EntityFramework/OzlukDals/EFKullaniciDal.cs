using Core.DataAccess;
using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete.OzlukEntities;
using Core.Entities.Enums;
using Core.Utilities.Paging;
using DataAccess.Concrete.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework.OzlukDals
{
    public interface IKullaniciDal : IEntityRepository<Kullanici>
    {
        Task<(List<Kullanici> Items, int TotalCount)> GetPagedAsync(
            KullaniciTipi? kullaniciTipi,
            string? ad,
            string? ortaAd,
            string? soyad,
            string? kurumEposta,
            string? kurumSicilNo,
            DateTime? olusturmaTarihi,
            DateTime? guncellemeTarihi,
            int page,
            int pageSize);
    }

    public class EFKullaniciDal : EfEntityRepositoryBase<Kullanici, AEUContext>, IKullaniciDal
    {
        public async Task<(List<Kullanici> Items, int TotalCount)> GetPagedAsync(
            KullaniciTipi? kullaniciTipi,
            string? ad,
            string? ortaAd,
            string? soyad,
            string? kurumEposta,
            string? kurumSicilNo,
            DateTime? olusturmaTarihi,
            DateTime? guncellemeTarihi,
            int page,
            int pageSize)
        {
            using AEUContext context = new AEUContext();

            IQueryable<Kullanici> query = context.Kullanicilar.AsNoTracking();

            if (kullaniciTipi.HasValue && kullaniciTipi.Value != KullaniciTipi._)
                query = query.Where(k => k.KullaniciTipi == kullaniciTipi.Value);

            if (!string.IsNullOrWhiteSpace(ad))
                query = query.Where(k => k.Ad.StartsWith(ad));

            if (!string.IsNullOrWhiteSpace(ortaAd))
                query = query.Where(k => k.OrtaAd != null && k.OrtaAd.StartsWith(ortaAd));

            if (!string.IsNullOrWhiteSpace(soyad))
                query = query.Where(k => k.Soyad.StartsWith(soyad));

            if (!string.IsNullOrWhiteSpace(kurumEposta))
                query = query.Where(k => k.KurumEposta.StartsWith(kurumEposta));

            if (!string.IsNullOrWhiteSpace(kurumSicilNo))
                query = query.Where(k => k.KurumSicilNo.StartsWith(kurumSicilNo));

            if (olusturmaTarihi.HasValue && olusturmaTarihi.Value != default)
                query = query.Where(k => k.OlusturmaTarihi.Date >= olusturmaTarihi.Value.Date);

            if (guncellemeTarihi.HasValue && guncellemeTarihi.Value != default)
                query = query.Where(k => k.GuncellemeTarihi.Date <= guncellemeTarihi.Value.Date);

            int totalCount = await query.CountAsync();

            List<Kullanici> items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
