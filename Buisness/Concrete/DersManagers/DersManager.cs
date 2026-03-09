using Core.Utilities.Paging;
using Core.Utilities.Results;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.DataResults;
using DataAccess.Concrete.EntityFramework.DersDals;
using Entities.Concrete.DersEntities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Concrete.DersManagers
{
    public interface IDersService
    {
        Task<IDataResult<Ders>> GetByUuidAsync(Guid dersUuid);
        Task<IDataResult<List<Ders>>> GetAllAsync();
        Task<IResult> AddDersAsync(Ders ders);
        Task<IResult> UpdateDersAsync(Ders ders);
        Task<IResult> DeleteDersAsync(Guid dersUuid);
        Task<IDataResult<List<Ders>>> GetAllByPaged(
            string? dersKodu,
            string? dersAdi,
            int? kredi,
            int? akts,
            DateTime olusturmaTarihi,
            DateTime guncellemeTarihi,
            Pager? pager);
    }

    public class DersManager : IDersService
    {
        private readonly IDersDal _dersDal;

        public DersManager(IDersDal dersDal)
        {
            _dersDal = dersDal;
        }

        public async Task<IDataResult<Ders>> GetByUuidAsync(Guid dersUuid)
        {
            var result = await _dersDal.GetAsync(d => d.DersUuid == dersUuid);
            return result is null
                ? new DataResult<Ders>(null, false)
                : new DataResult<Ders>(result, true);
        }

        public async Task<IDataResult<List<Ders>>> GetAllAsync()
        {
            var result = await _dersDal.GetAllAsync();
            var listResult = result is List<Ders> dersList ? dersList : new List<Ders>(result);
            return new DataResult<List<Ders>>(listResult, true);
        }

        public async Task<IResult> AddDersAsync(Ders ders)
        {
            await _dersDal.AddAsync(ders);
            return new SuccessResult();
        }

        public async Task<IResult> UpdateDersAsync(Ders ders)
        {
            await _dersDal.UpdateAsync(ders);
            return new SuccessResult();
        }

        public async Task<IResult> DeleteDersAsync(Guid dersUuid)
        {
            var ders = await _dersDal.GetAsync(d => d.DersUuid == dersUuid);
            if (ders is null)
                return new ErrorResult("Ders not found.");
            await _dersDal.DeleteAsync(ders);
            return new SuccessResult();
        }

        public async Task<IDataResult<List<Ders>>> GetAllByPaged(
            string? dersKodu,
            string? dersAdi,
            int? kredi,
            int? akts,
            DateTime olusturmaTarihi,
            DateTime guncellemeTarihi,
            Pager? pager)
        {
            int page = pager?.Page <= 0 ? 1 : pager?.Page ?? 1;
            int pageSize = pager?.PageSize <= 0 ? 10 : pager?.PageSize ?? 10;

            var (items, totalCount) = await _dersDal.GetPagedAsync(
                dersKodu,
                dersAdi,
                kredi,
                akts,
                olusturmaTarihi == default ? null : olusturmaTarihi,
                guncellemeTarihi == default ? null : guncellemeTarihi,
                page,
                pageSize);

            if (pager != null)
                pager.TotalCount = totalCount;

            if (!items.Any())
                return new DataResult<List<Ders>>(items, false, "No courses found.");

            return new DataResult<List<Ders>>(items, true, "Courses retrieved successfully.");
        }
    }
}