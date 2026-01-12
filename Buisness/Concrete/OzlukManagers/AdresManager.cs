using Core.Utilities.Results;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.DataResults;
using DataAccess.Concrete.EntityFramework.OzlukDals;
using Entities.Concrete.OzlukEntities;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
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
        Task<IResult> SetPrimaryAddres(Adres? newPrimaryAddres);
        Task<IDataResult<Adres>> ResetPrimaryAddress(Adres newPrimaryAddress);
        Task<IDataResult<Adres>> ResetNonPrimaryAddress(Adres nonPrimaryAddress);

        Task<IResult> DeleteByAddresUuidAsync(Guid adresUuid);
    }
    public class AdresManager : IAdresService
    {
        private readonly IAdresDal _adresDal;

        public AdresManager(IAdresDal adresDal)
        {
            _adresDal = adresDal;
        }

        public async Task<IDataResult<Adres>> AddAdresAsync(Adres adres)
        {
            if (adres == null)
                return new ErrorDataResult<Adres>(null!, "Adres bilgileri boş olamaz.");
            adres.OlusturmaTarihi = DateTime.UtcNow;
            adres.GuncellemeTarihi = DateTime.UtcNow;
            await _adresDal.AddAsync(adres);
            return new SuccessDataResult<Adres>(adres, "Adres başarıyla eklendi.");
        }

        public async Task<IResult> DeleteByAddresUuidAsync(Guid adresUuid)
        {
            if (adresUuid == Guid.Empty)
                return new ErrorResult("Geçersiz adres UUID'si.");

            Adres? adresToDelete = await _adresDal.GetAsync(a => a.AdresUuid == adresUuid);
            if (adresToDelete == null)
                return new ErrorResult("Adres bulunamadı.");

            await _adresDal.DeleteAsync(adresToDelete);

            return new SuccessResult("Adres başarıyla silindi.");
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

        public async Task<IDataResult<Adres>> ResetNonPrimaryAddress(Adres nonPrimaryAddress)
        {
            if (nonPrimaryAddress == null)
                return new ErrorDataResult<Adres>(null!, "Adres bilgileri boş olamaz.");
            nonPrimaryAddress.Oncelikli = false;
            nonPrimaryAddress.GuncellemeTarihi = DateTime.UtcNow;
            await _adresDal.UpdateAsync(nonPrimaryAddress);
            return new SuccessDataResult<Adres>(nonPrimaryAddress, "Adres önceliği başarıyla sıfırlandı.");
        }

        public async Task<IDataResult<Adres>> ResetPrimaryAddress(Adres newPrimaryAddress)
        {
            if (newPrimaryAddress == null)
                return new ErrorDataResult<Adres>(null!, "Adres bilgileri boş olamaz.");
            Adres? currentPrimaryAddress = await _adresDal.GetAsync(a => a.KullaniciUuid == newPrimaryAddress.KullaniciUuid && a.Oncelikli == true);
            if (currentPrimaryAddress != null)
            {
                currentPrimaryAddress.Oncelikli = false;
                currentPrimaryAddress.GuncellemeTarihi = DateTime.UtcNow;
                await _adresDal.UpdateAsync(currentPrimaryAddress);
            }
            newPrimaryAddress.Oncelikli = true;
            newPrimaryAddress.GuncellemeTarihi = DateTime.UtcNow;
            await _adresDal.UpdateAsync(newPrimaryAddress);
            return new SuccessDataResult<Adres>(newPrimaryAddress, "Yeni öncelikli adres başarıyla ayarlandı.");
        }

        public async Task<IResult> SetPrimaryAddres(Adres? newPrimaryAddres)
        {
            if (newPrimaryAddres == null)
                return new ErrorResult("Yeni öncelikli adres bilgileri boş olamaz.");

            Adres? adres = await _adresDal.GetAsync(a => a.AdresUuid == newPrimaryAddres.AdresUuid);
            if (adres == null)
                return new ErrorResult("Yeni öncelikli adres bulunamadı.");
            adres.Oncelikli = true;
            adres.GuncellemeTarihi = DateTime.UtcNow;
            await _adresDal.UpdateAsync(adres);
            return new SuccessResult("Yeni öncelikli adres başarıyla ayarlandı.");
        }
    }
}
