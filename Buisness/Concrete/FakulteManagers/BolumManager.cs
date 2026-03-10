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
    public interface IBolumService
    {
        Task<IDataResult<Bolum>> GetByUuidAsync(Guid bolumUuid);
        Task<IDataResult<List<Bolum>>> GetAllAsync();
        Task<IResult> AddBolumAsync(Bolum bolum);
        Task<IResult> UpdateBolumAsync(Bolum bolum);
        Task<IResult> DeleteBolumAsync(Guid bolumUuid);
        Task<IDataResult<List<Bolum>>> GetAllByPaged(
            string? bolumAdi,
            Guid? fakulteUuid,
            DateTime kurulusTarihi,
            DateTime olusturmaTarihi,
            DateTime guncellemeTarihi,
            Pager? pager);
    }

    public class BolumManager : IBolumService
    {
        private readonly IBolumDal _bolumDal;

        public BolumManager(IBolumDal bolumDal)
        {
            _bolumDal = bolumDal;
        }

        public async Task<IDataResult<Bolum>> GetByUuidAsync(Guid bolumUuid)
        {
            var result = await _bolumDal.GetAsync(b => b.BolumUuid == bolumUuid);
            return result is null
                ? new DataResult<Bolum>(null, false)
                : new DataResult<Bolum>(result, true);
        }

        public async Task<IDataResult<List<Bolum>>> GetAllAsync()
        {
            var result = await _bolumDal.GetAllAsync();
            var listResult = result is List<Bolum> bolumList ? bolumList : new List<Bolum>(result);
            return new DataResult<List<Bolum>>(listResult, true);
        }

        public async Task<IResult> AddBolumAsync(Bolum bolum)
        {
            await _bolumDal.AddAsync(bolum);
            return new SuccessResult("Bölüm başarıyla eklendi.");
        }

        public async Task<IResult> UpdateBolumAsync(Bolum bolum)
        {
            await _bolumDal.UpdateAsync(bolum);
            return new SuccessResult("Bölüm başarıyla güncellendi.");
        }

        public async Task<IResult> DeleteBolumAsync(Guid bolumUuid)
        {
            var bolum = await _bolumDal.GetAsync(b => b.BolumUuid == bolumUuid);
            if (bolum is null)
                return new ErrorResult("Bölüm bulunamadı.");
            await _bolumDal.DeleteAsync(bolum);
            return new SuccessResult("Bölüm başarıyla silindi.");
        }

        public async Task<IDataResult<List<Bolum>>> GetAllByPaged(
            string? bolumAdi,
            Guid? fakulteUuid,
            DateTime kurulusTarihi,
            DateTime olusturmaTarihi,
            DateTime guncellemeTarihi,
            Pager? pager)
        {
            int page = pager?.Page <= 0 ? 1 : pager?.Page ?? 1;
            int pageSize = pager?.PageSize <= 0 ? 10 : pager?.PageSize ?? 10;

            var (items, totalCount) = await _bolumDal.GetPagedAsync(
                bolumAdi,
                fakulteUuid,
                kurulusTarihi == default ? null : kurulusTarihi,
                olusturmaTarihi == default ? null : olusturmaTarihi,
                guncellemeTarihi == default ? null : guncellemeTarihi,
                page,
                pageSize);

            if (pager != null)
                pager.TotalCount = totalCount;

            if (!items.Any())
                return new DataResult<List<Bolum>>(items, false, "Bölüm bulunamadı.");

            return new DataResult<List<Bolum>>(items, true, "Bölümler başarıyla getirildi.");
        }
    }
}