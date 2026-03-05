using Core.Entities.Concrete.OzlukEntities;
using Core.Entities.Enums;
using Core.Utilities.Paging;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.DataResults;
using DataAccess.Concrete.EntityFramework.OzlukDals;

namespace Business.Concrete
{
    public interface IKullaniciService
    {
        Task<IDataResult<Kullanici>> GetByUuid(Guid kullaniciUuid);
        Task<IDataResult<Kullanici>> UpdateKullaniciAsync(Kullanici kullanici);
        Task<IDataResult<Kullanici>> GetByUuidAsync(Guid kullaniciUuid);
        Task<IDataResult<Kullanici>> GetByEmailAsync(string kurum_eposta);
        Task<IDataResult<Kullanici>> GetBySicilNoAsync(string kurumSicilNo);
        Task<IDataResult<List<Kullanici>>> GetAllByPaged(KullaniciTipi kullaniciTipi, string? ad, string? ortaAd, string? soyad, string? kurumEposta, string? kurumSicilNo, DateTime olusturmaTarihi, DateTime guncellemeTarihi, Pager? pager);
    }

    public class KullaniciManager : IKullaniciService
    {
        private readonly IKullaniciDal _kullaniciDal;

        public KullaniciManager(IKullaniciDal kullaniciDal)
        {
            _kullaniciDal = kullaniciDal;
        }

        public async Task<IDataResult<Kullanici>> GetByUuid(Guid kullaniciUuid)
        {
            var result = _kullaniciDal.Get(k => k.KullaniciUuid == kullaniciUuid);
            if (result is null)
                return new DataResult<Kullanici>(null, false);
            return new DataResult<Kullanici>(result, true);
        }

        public async Task<IDataResult<Kullanici>> GetByUuidAsync(Guid kullaniciUuid)
        {
            var result = await _kullaniciDal.GetAsync(k => k.KullaniciUuid == kullaniciUuid);
            if (result is null)
                return new DataResult<Kullanici>(null, false);
            return new DataResult<Kullanici>(result, true);
        }

        public async Task<IDataResult<Kullanici>> GetByEmailAsync(string kurum_eposta)
        {
            var result = await _kullaniciDal.GetAsync(k => k.KurumEposta == kurum_eposta);
            if (result is null)
                return new DataResult<Kullanici>(null, false);
            return new DataResult<Kullanici>(result, true);
        }

        public async Task<IDataResult<Kullanici>> GetBySicilNoAsync(string kurumSicilNo)
        {
            var result = await _kullaniciDal.GetAsync(k => k.KurumSicilNo == kurumSicilNo);
            if (result is null)
                return new DataResult<Kullanici>(null, false);
            return new DataResult<Kullanici>(result, true);
        }

        public async Task<IDataResult<Kullanici>> UpdateKullaniciAsync(Kullanici kullanici)
        {
            await _kullaniciDal.UpdateAsync(kullanici);
            var result = await _kullaniciDal.GetAsync(k => k.KullaniciUuid == kullanici.KullaniciUuid);
            if (result is null)
                return new DataResult<Kullanici>(null, false);
            return new DataResult<Kullanici>(result, true);
        }

        public async Task<IDataResult<List<Kullanici>>> GetAllByPaged(
            KullaniciTipi kullaniciTipi,
            string? ad,
            string? ortaAd,
            string? soyad,
            string? kurumEposta,
            string? kurumSicilNo,
            DateTime olusturmaTarihi,
            DateTime guncellemeTarihi,
            Pager? pager)
        {
            int page     = pager?.Page     <= 0 ? 1  : pager?.Page     ?? 1;
            int pageSize = pager?.PageSize  <= 0 ? 10 : pager?.PageSize ?? 10;

            var (items, totalCount) = await _kullaniciDal.GetPagedAsync(
                kullaniciTipi == KullaniciTipi._ ? null : kullaniciTipi,
                ad,
                ortaAd,
                soyad,
                kurumEposta,
                kurumSicilNo,
                olusturmaTarihi == default ? null : olusturmaTarihi,
                guncellemeTarihi == default ? null : guncellemeTarihi,
                page,
                pageSize);

            if (pager != null)
                pager.TotalCount = totalCount;

            if (!items.Any())
                return new DataResult<List<Kullanici>>(items, false, "Kullan?c? bulunamad?.");

            return new DataResult<List<Kullanici>>(items, true, "Kullan?c?lar ba?ar?yla getirildi.");
        }
    }
}