using Core.DataAccess;
using Core.DataAccess.EntityFramework;
using Core.Utilities.Paging;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete.DersEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework.DersDals
{
    public interface IDersDal : IEntityRepository<Ders>
    {
        Task<(List<Ders>, int)> GetPagedAsync(
            string? dersKodu,
            string? dersAdi,
            int? kredi,
            int? akts,
            DateTime? olusturmaTarihi,
            DateTime? guncellemeTarihi,
            int page,
            int pageSize);
    }
    public class EFDersDal : EfEntityRepositoryBase<Ders, AEUContext>, IDersDal
    {
        public async Task<(List<Ders>, int)> GetPagedAsync(
            string? dersKodu,
            string? dersAdi,
            int? kredi,
            int? akts,
            DateTime? olusturmaTarihi,
            DateTime? guncellemeTarihi,
            int page,
            int pageSize)
        {
            using var context = new AEUContext();

            var query = context.Dersler.AsQueryable();

            if (!string.IsNullOrEmpty(dersKodu))
                query = query.Where(d => d.DersKodu.Contains(dersKodu));

            if (!string.IsNullOrEmpty(dersAdi))
                query = query.Where(d => d.DersAdi.Contains(dersAdi));

            if (kredi.HasValue)
                query = query.Where(d => d.Kredi == kredi.Value);

            if (akts.HasValue)
                query = query.Where(d => d.Akts == akts.Value);

            if (olusturmaTarihi.HasValue)
                query = query.Where(d => d.OlusturmaTarihi >= olusturmaTarihi.Value);

            if (guncellemeTarihi.HasValue)
                query = query.Where(d => d.GuncellemeTarihi <= guncellemeTarihi.Value);

            int totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
