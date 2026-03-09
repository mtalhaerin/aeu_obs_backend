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
    public interface IBolumDal : IEntityRepository<Bolum>
    {
        Task<(List<Bolum>, int)> GetPagedAsync(
            string? bolumAdi,
            Guid? anaDalUuid,
            DateTime? kurulusTarihi,
            DateTime? olusturmaTarihi,
            DateTime? guncellemeTarihi,
            int page,
            int pageSize);
    }

    public class EFBolumDal : EfEntityRepositoryBase<Bolum, AEUContext>, IBolumDal
    {
        public async Task<(List<Bolum>, int)> GetPagedAsync(
            string? bolumAdi,
            Guid? anaDalUuid,
            DateTime? kurulusTarihi,
            DateTime? olusturmaTarihi,
            DateTime? guncellemeTarihi,
            int page,
            int pageSize)
        {
            using var context = new AEUContext();

            var query = context.Bolumler.AsQueryable();

            if (!string.IsNullOrEmpty(bolumAdi))
                query = query.Where(b => b.BolumAdi.Contains(bolumAdi));

            if (anaDalUuid.HasValue && anaDalUuid != Guid.Empty)
                query = query.Where(b => b.AnaDalUuid == anaDalUuid.Value);

            if (kurulusTarihi.HasValue)
                query = query.Where(b => b.KurulusTarihi >= kurulusTarihi.Value);

            if (olusturmaTarihi.HasValue)
                query = query.Where(b => b.OlusturmaTarihi >= olusturmaTarihi.Value);

            if (guncellemeTarihi.HasValue)
                query = query.Where(b => b.GuncellemeTarihi <= guncellemeTarihi.Value);

            int totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
