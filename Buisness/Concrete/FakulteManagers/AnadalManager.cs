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
    public interface IAnaDalService
    {
        Task<IDataResult<AnaDal>> GetByUuidAsync(Guid anaDalUuid);
        Task<IDataResult<List<AnaDal>>> GetAllAsync();
        Task<IResult> AddAnaDalAsync(AnaDal anaDal);
        Task<IResult> UpdateAnaDalAsync(AnaDal anaDal);
        Task<IResult> DeleteAnaDalAsync(Guid anaDalUuid);
        Task<IDataResult<List<AnaDal>>> GetAllByPaged(
            string? anaDalAdi,
            Guid? bolumUuid,
            DateTime kurulusTarihi,
            DateTime olusturmaTarihi,
            DateTime guncellemeTarihi,
            Pager? pager);
    }

    public class AnaDalManager : IAnaDalService
    {
        private readonly IAnaDalDal _anaDalDal;

        public AnaDalManager(IAnaDalDal anaDalDal)
        {
            _anaDalDal = anaDalDal;
        }

        public async Task<IDataResult<AnaDal>> GetByUuidAsync(Guid anaDalUuid)
        {
            var result = await _anaDalDal.GetAsync(a => a.AnaDalUuid == anaDalUuid);
            return result is null
                ? new DataResult<AnaDal>(null, false)
                : new DataResult<AnaDal>(result, true);
        }

        public async Task<IDataResult<List<AnaDal>>> GetAllAsync()
        {
            var result = await _anaDalDal.GetAllAsync();
            var listResult = result is List<AnaDal> anaDalList ? anaDalList : new List<AnaDal>(result);
            return new DataResult<List<AnaDal>>(listResult, true);
        }

        public async Task<IResult> AddAnaDalAsync(AnaDal anaDal)
        {
            await _anaDalDal.AddAsync(anaDal);
            return new SuccessResult("Ana dal başarıyla eklendi.");
        }

        public async Task<IResult> UpdateAnaDalAsync(AnaDal anaDal)
        {
            await _anaDalDal.UpdateAsync(anaDal);
            return new SuccessResult("Ana dal başarıyla güncellendi.");
        }

        public async Task<IResult> DeleteAnaDalAsync(Guid anaDalUuid)
        {
            var anaDal = await _anaDalDal.GetAsync(a => a.AnaDalUuid == anaDalUuid);
            if (anaDal is null)
                return new ErrorResult("Ana dal bulunamadı.");
            await _anaDalDal.DeleteAsync(anaDal);
            return new SuccessResult("Ana dal başarıyla silindi.");
        }

        public async Task<IDataResult<List<AnaDal>>> GetAllByPaged(
            string? anaDalAdi,
            Guid? bolumUuid,
            DateTime kurulusTarihi,
            DateTime olusturmaTarihi,
            DateTime guncellemeTarihi,
            Pager? pager)
        {
            int page = pager?.Page <= 0 ? 1 : pager?.Page ?? 1;
            int pageSize = pager?.PageSize <= 0 ? 10 : pager?.PageSize ?? 10;

            // Only pass fakulteUuid if it has a meaningful value (not null and not empty)
            Guid? effectiveFakulteUuid = bolumUuid.HasValue && bolumUuid.Value != Guid.Empty ? bolumUuid : null;

            var (items, totalCount) = await _anaDalDal.GetPagedAsync(
                anaDalAdi,
                effectiveFakulteUuid,
                kurulusTarihi == default ? null : kurulusTarihi,
                olusturmaTarihi == default ? null : olusturmaTarihi,
                guncellemeTarihi == default ? null : guncellemeTarihi,
                page,
                pageSize);

            if (pager != null)
                pager.TotalCount = totalCount;

            if (!items.Any())
                return new DataResult<List<AnaDal>>(items, false, "Ana dal bulunamadı.");

            return new DataResult<List<AnaDal>>(items, true, "Ana dallar başarıyla getirildi.");
        }
    }
}