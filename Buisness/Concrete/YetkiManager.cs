using Core.Entities.Concrete.OzlukEntities;
using Core.Entities.Concrete.YetkiEntities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.DataResults;
using DataAccess.Concrete.EntityFramework.OzlukDals;
using DataAccess.Concrete.EntityFramework.YetkiDals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public interface IYetkiService
    {
        // Düzeltme 1: Interface dönüş tipi IEnumerable (Liste) olarak güncellendi
        Task<IDataResult<IEnumerable<KullaniciIslemYetkisi>>> GetKullaniciYetkileriAsync(Guid kullaniciUuid);
    }

    public class YetkiManager : IYetkiService
    {
        private readonly IKullaniciIslemYetkisiDal _kullaniciIslemYetkisiDal;
        private readonly IIslemYetkisiDal _islemYetkisiDal;

        public YetkiManager(IIslemYetkisiDal islemYetkisiDal, IKullaniciIslemYetkisiDal kullaniciIslemYetkisiDal)
        {
            _islemYetkisiDal = islemYetkisiDal;
            _kullaniciIslemYetkisiDal = kullaniciIslemYetkisiDal;
        }

        public async Task<IDataResult<IEnumerable<KullaniciIslemYetkisi>>> GetKullaniciYetkileriAsync(Guid kullaniciUuid)
        {
            if (kullaniciUuid == Guid.Empty)
                return new ErrorDataResult<IEnumerable<KullaniciIslemYetkisi>>(new List<KullaniciIslemYetkisi>(), "Geçersiz kullanıcı UUID'si.");

            IEnumerable<KullaniciIslemYetkisi> result = await _kullaniciIslemYetkisiDal.GetAllAsync(k => k.KullaniciUuid == kullaniciUuid);
            if (result == null || !result.Any())
                return new SuccessDataResult<IEnumerable<KullaniciIslemYetkisi>>(new List<KullaniciIslemYetkisi>(), "Kullanıcıya ait bir yetki bulunamadı.");

            return new SuccessDataResult<IEnumerable<KullaniciIslemYetkisi>>(result, "Kullanıcıya ait yetkiler başarıyla getirildi.");
        }
    }
}