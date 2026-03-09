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
    public interface IOgrenciBolumKayitService
    {
        Task<IDataResult<OgrenciBolumKayit>> GetByUuidAsync(Guid kayitUuid);
        Task<IDataResult<List<OgrenciBolumKayit>>> GetAllAsync();
        Task<IResult> AddKayitAsync(OgrenciBolumKayit kayit);
        Task<IResult> UpdateKayitAsync(OgrenciBolumKayit kayit);
        Task<IResult> DeleteKayitAsync(Guid kayitUuid);
        Task<IDataResult<List<OgrenciBolumKayit>>> GetAllByPaged(
            Guid? kullaniciUuid,
            Guid? bolumUuid,
            DateTime olusturmaTarihi,
            DateTime guncellemeTarihi,
            Pager? pager);
    }

    public class OgrenciBolumKayitManager : IOgrenciBolumKayitService
    {
        private readonly IOgrenciBolumKayitDal _kayitDal;

        public OgrenciBolumKayitManager(IOgrenciBolumKayitDal kayitDal)
        {
            _kayitDal = kayitDal;
        }

        public async Task<IDataResult<OgrenciBolumKayit>> GetByUuidAsync(Guid kayitUuid)
        {
            var result = await _kayitDal.GetAsync(k => k.BolumKayitUuid == kayitUuid);
            return result is null
                ? new DataResult<OgrenciBolumKayit>(null, false)
                : new DataResult<OgrenciBolumKayit>(result, true);
        }

        public async Task<IDataResult<List<OgrenciBolumKayit>>> GetAllAsync()
        {
            var result = await _kayitDal.GetAllAsync();
            var listResult = result is List<OgrenciBolumKayit> kayitList ? kayitList : new List<OgrenciBolumKayit>(result);
            return new DataResult<List<OgrenciBolumKayit>>(listResult, true);
        }

        public async Task<IResult> AddKayitAsync(OgrenciBolumKayit kayit)
        {
            await _kayitDal.AddAsync(kayit);
            return new SuccessResult("Öğrenci kayıt başarıyla eklendi.");
        }

        public async Task<IResult> UpdateKayitAsync(OgrenciBolumKayit kayit)
        {
            await _kayitDal.UpdateAsync(kayit);
            return new SuccessResult("Öğrenci kayıt başarıyla güncellendi.");
        }

        public async Task<IResult> DeleteKayitAsync(Guid kayitUuid)
        {
            var kayit = await _kayitDal.GetAsync(k => k.BolumKayitUuid == kayitUuid);
            if (kayit is null)
                return new ErrorResult("Öğrenci kayıt bulunamadı.");
            await _kayitDal.DeleteAsync(kayit);
            return new SuccessResult("Öğrenci kayıt başarıyla silindi.");
        }

        public async Task<IDataResult<List<OgrenciBolumKayit>>> GetAllByPaged(
            Guid? kullaniciUuid,
            Guid? bolumUuid,
            DateTime olusturmaTarihi,
            DateTime guncellemeTarihi,
            Pager? pager)
        {
            int page = pager?.Page <= 0 ? 1 : pager?.Page ?? 1;
            int pageSize = pager?.PageSize <= 0 ? 10 : pager?.PageSize ?? 10;

            var (items, totalCount) = await _kayitDal.GetPagedAsync(
                kullaniciUuid,
                bolumUuid,
                olusturmaTarihi == default ? null : olusturmaTarihi,
                guncellemeTarihi == default ? null : guncellemeTarihi,
                page,
                pageSize);

            if (pager != null)
                pager.TotalCount = totalCount;

            if (!items.Any())
                return new DataResult<List<OgrenciBolumKayit>>(items, false, "Öğrenci kayıt bulunamadı.");

            return new DataResult<List<OgrenciBolumKayit>>(items, true, "Öğrenci kayıtları başarıyla getirildi.");
        }
    }
}