using Core.Utilities.Paging;
using Core.Utilities.Results;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.DataResults;
using DataAccess.Concrete.EntityFramework.FakulteDals;
using Entities.Concrete.FakulteEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Concrete.FakulteManagers
{
    public interface IFakulteService
    {
        Task<IDataResult<Fakulte>> GetByUuidAsync(Guid fakulteUuid);
        Task<IDataResult<List<Fakulte>>> GetAllAsync();
        Task<IResult> AddFakulteAsync(Fakulte fakulte);
        Task<IResult> UpdateFakulteAsync(Fakulte fakulte);
        Task<IResult> DeleteFakulteAsync(Guid fakulteUuid);
        Task<IDataResult<List<Fakulte>>> GetAllByPaged(
            string? fakulteAdi,
            string? webAdres,
            DateTime kurulusTarihi,
            DateTime olusturmaTarihi,
            DateTime guncellemeTarihi,
            Pager? pager);
    }

    public class FakulteManager : IFakulteService
    {
        private readonly IFakulteDal _fakulteDal;

        public FakulteManager(IFakulteDal fakulteDal)
        {
            _fakulteDal = fakulteDal;
        }

        public async Task<IDataResult<Fakulte>> GetByUuidAsync(Guid fakulteUuid)
        {
            var result = await _fakulteDal.GetAsync(f => f.FakulteUuid == fakulteUuid);
            return result is null
                ? new DataResult<Fakulte>(null, false)
                : new DataResult<Fakulte>(result, true);
        }

        public async Task<IDataResult<List<Fakulte>>> GetAllAsync()
        {
            var result = await _fakulteDal.GetAllAsync();
            var listResult = result is List<Fakulte> fakulteList ? fakulteList : new List<Fakulte>(result);
            return new DataResult<List<Fakulte>>(listResult, true);
        }

        public async Task<IResult> AddFakulteAsync(Fakulte fakulte)
        {
            await _fakulteDal.AddAsync(fakulte);
            return new SuccessResult("Fakulte başarıyla eklendi.");
        }

        public async Task<IResult> UpdateFakulteAsync(Fakulte fakulte)
        {
            await _fakulteDal.UpdateAsync(fakulte);
            return new SuccessResult("Fakulte başarıyla güncellendi.");
        }

        public async Task<IResult> DeleteFakulteAsync(Guid fakulteUuid)
        {
            var fakulte = await _fakulteDal.GetAsync(f => f.FakulteUuid == fakulteUuid);
            if (fakulte is null)
                return new ErrorResult("Fakulte bulunamadı.");
            await _fakulteDal.DeleteAsync(fakulte);
            return new SuccessResult("Fakulte başarıyla silindi.");
        }

        public async Task<IDataResult<List<Fakulte>>> GetAllByPaged(
            string? fakulteAdi,
            string? webAdres,
            DateTime kurulusTarihi,
            DateTime olusturmaTarihi,
            DateTime guncellemeTarihi,
            Pager? pager)
        {
            int page = pager?.Page <= 0 ? 1 : pager?.Page ?? 1;
            int pageSize = pager?.PageSize <= 0 ? 10 : pager?.PageSize ?? 10;

            var (items, totalCount) = await _fakulteDal.GetPagedAsync(
                fakulteAdi,
                webAdres,
                kurulusTarihi == default ? null : kurulusTarihi,
                olusturmaTarihi == default ? null : olusturmaTarihi,
                guncellemeTarihi == default ? null : guncellemeTarihi,
                page,
                pageSize);

            if (pager != null)
                pager.TotalCount = totalCount;

            if (!items.Any())
                return new DataResult<List<Fakulte>>(items, false, "Fakulte bulunamadı.");

            return new DataResult<List<Fakulte>>(items, true, "Fakulteler başarıyla getirildi.");
        }
    }
}