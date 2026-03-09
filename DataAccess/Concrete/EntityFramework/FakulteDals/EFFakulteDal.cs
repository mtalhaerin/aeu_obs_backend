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
    public interface IFakulteDal : IEntityRepository<Fakulte>
    {
        Task<(List<Fakulte>, int)> GetPagedAsync(
            string? fakulteAdi,
            string? webAdres,
            DateTime? kurulusTarihi,
            DateTime? olusturmaTarihi,
            DateTime? guncellemeTarihi,
            int page,
            int pageSize);
    }

    public class EFFakulteDal : EfEntityRepositoryBase<Fakulte, AEUContext>, IFakulteDal
    {
        public async Task<(List<Fakulte>, int)> GetPagedAsync(
            string? fakulteAdi,
            string? webAdres,
            DateTime? kurulusTarihi,
            DateTime? olusturmaTarihi,
            DateTime? guncellemeTarihi,
            int page,
            int pageSize)
        {
            using var context = new AEUContext();

            var query = context.Fakulteler.AsQueryable();

            if (!string.IsNullOrEmpty(fakulteAdi))
                query = query.Where(f => f.FakulteAdi.Contains(fakulteAdi));

            if (!string.IsNullOrEmpty(webAdres))
                query = query.Where(f => f.WebAdres.Contains(webAdres));

            if (kurulusTarihi.HasValue)
                query = query.Where(f => f.KurulusTarihi >= kurulusTarihi.Value);

            if (olusturmaTarihi.HasValue)
                query = query.Where(f => f.OlusturmaTarihi >= olusturmaTarihi.Value);

            if (guncellemeTarihi.HasValue)
                query = query.Where(f => f.GuncellemeTarihi <= guncellemeTarihi.Value);

            int totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
