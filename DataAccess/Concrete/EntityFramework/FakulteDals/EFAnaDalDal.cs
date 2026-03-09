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
    public interface IAnaDalDal : IEntityRepository<AnaDal>
    {
        Task<(List<AnaDal>, int)> GetPagedAsync(
            string? anaDalAdi,
            Guid? fakulteUuid,
            DateTime? kurulusTarihi,
            DateTime? olusturmaTarihi,
            DateTime? guncellemeTarihi,
            int page,
            int pageSize);
    }

    public class EFAnaDalDal : EfEntityRepositoryBase<AnaDal, AEUContext>, IAnaDalDal
    {
        public async Task<(List<AnaDal>, int)> GetPagedAsync(
            string? anaDalAdi,
            Guid? fakulteUuid,
            DateTime? kurulusTarihi,
            DateTime? olusturmaTarihi,
            DateTime? guncellemeTarihi,
            int page,
            int pageSize)
        {
            using var context = new AEUContext();

            var query = context.AnaDallar.AsQueryable();

            if (!string.IsNullOrEmpty(anaDalAdi))
                query = query.Where(a => a.AnaDalAdi.Contains(anaDalAdi));

            if (fakulteUuid.HasValue && fakulteUuid != Guid.Empty)
                query = query.Where(a => a.FakulteUuid == fakulteUuid.Value);

            if (kurulusTarihi.HasValue)
                query = query.Where(a => a.KurulusTarihi >= kurulusTarihi.Value);

            if (olusturmaTarihi.HasValue)
                query = query.Where(a => a.OlusturmaTarihi >= olusturmaTarihi.Value);

            if (guncellemeTarihi.HasValue)
                query = query.Where(a => a.GuncellemeTarihi <= guncellemeTarihi.Value);

            int totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
