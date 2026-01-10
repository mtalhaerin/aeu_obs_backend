using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.DataResults;
using DataAccess.Concrete.EntityFramework.OzlukDals;
using Entities.Concrete.OzlukEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete.OzlukManagers
{
    public interface IAdresService
    {
        Task<IDataResult<Adres>> AddAdresAsync(Adres adres);
        Task<IDataResult<Adres>> GetUserAddresByUuidAsync(Guid kullaniciUuid, Guid? adresUuid);
        Task<IDataResult<IEnumerable<Adres>>> GetUserAddresesAsync(Guid kullaniciUuid);
    }
    public class AdresManager : IAdresService
    {
        private readonly IAdresDal _adresDal;

        public AdresManager(IAdresDal adresDal)
        {
            _adresDal = adresDal;
        }

        public Task<IDataResult<Adres>> AddAdresAsync(Adres adres)
        {
            if (adres == null)
                return Task.FromResult<IDataResult<Adres>>(new ErrorDataResult<Adres>(null!, "Adres bilgileri boş olamaz."));
            adres.OlusturmaTarihi = DateTime.UtcNow;
            adres.GuncellemeTarihi = DateTime.UtcNow;
            _adresDal.Add(adres);
            return Task.FromResult<IDataResult<Adres>>(new SuccessDataResult<Adres>(adres, "Adres başarıyla eklendi."));
        }

        public async Task<IDataResult<Adres>> GetUserAddresByUuidAsync(Guid kullaniciUuid, Guid? adresUuid)
        {
            if (adresUuid == Guid.Empty || adresUuid == null)
                return new ErrorDataResult<Adres>(null!, "Geçersiz adres UUID'si.");
            Adres? result = await _adresDal.GetAsync(a => a.AdresUuid == adresUuid && a.KullaniciUuid == kullaniciUuid);
            if (result == null)
                return new ErrorDataResult<Adres>(null!, "Adres bulunamadı.");
            return new SuccessDataResult<Adres>(result, "Adres başarıyla getirildi.");
        }

        public async Task<IDataResult<IEnumerable<Adres>>> GetUserAddresesAsync(Guid kullaniciUuid)
        {
            if (kullaniciUuid == Guid.Empty)
                return new ErrorDataResult<IEnumerable<Adres>>(new List<Adres>(), "Geçersiz kullanıcı UUID'si.");

            IEnumerable<Adres> result = await _adresDal.GetAllAsync(a => a.KullaniciUuid == kullaniciUuid);
            if (result == null || !result.Any())
                return new SuccessDataResult<IEnumerable<Adres>>(new List<Adres>(), "Kullanıcıya ait adres bulunamadı.");

            return new SuccessDataResult<IEnumerable<Adres>>(result, "Kullanıcıya ait adresler başarıyla getirildi.");
        }
    }
}
