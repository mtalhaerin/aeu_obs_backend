using Core.Utilities.Results;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.DataResults;
using DataAccess.Concrete.EntityFramework.OzlukDals;
using Entities.Concrete.OzlukEntities;

namespace Business.Concrete.OzlukManagers
{
    public interface IEpostaService
    {
        Task<IDataResult<Eposta>> AddEmailAsync(Eposta eposta);
        Task<IDataResult<Eposta>> GetUserEmailByUuidAsync(Guid kullaniciUuid, Guid? epostaUuid);
        Task<IDataResult<IEnumerable<Eposta>>> GetUserEmailsAsync(Guid kullaniciUuid);
        Task<IResult> SetPrimaryEmail(Eposta? newPrimaryEmail);
        Task<IResult> DeleteByEmailUuidAsync(Guid emailUuid);
        Task<IDataResult<Eposta>> ResetNonPrimaryEmail(Eposta nonPrimaryEmail);
        Task<IDataResult<Eposta>> ResetPrimaryEmail(Eposta newPrimaryEmail);
    }
    public class EpostaManager : IEpostaService
    {
        private readonly IEpostaDal _epostaDal;

        public EpostaManager(IEpostaDal epostaDal)
        {
            _epostaDal = epostaDal;
        }

        public async Task<IDataResult<Eposta>> AddEmailAsync(Eposta eposta)
        {
            if (eposta == null)
                return new ErrorDataResult<Eposta>("Eklenecek eposta bilgisi boş olamaz.");
            await _epostaDal.AddAsync(eposta);

            return new SuccessDataResult<Eposta>(eposta, "Email başarıyla eklendi.");
        }

        public async Task<IResult> DeleteByEmailUuidAsync(Guid emailUuid)
        {
            if (emailUuid == Guid.Empty)
                return new ErrorResult("Geçersiz adres UUID'si.");

            Eposta? emailToDelete = await _epostaDal.GetAsync(a => a.EpostaUuid == emailUuid);
            if (emailToDelete == null)
                return new ErrorResult("Email bulunamadı.");

            await _epostaDal.DeleteAsync(emailToDelete);

            return new SuccessResult("Email başarıyla silindi.");
        }

        public async Task<IDataResult<Eposta>> GetUserEmailByUuidAsync(Guid kullaniciUuid, Guid? epostaUuid)
        {
            if (epostaUuid == Guid.Empty || epostaUuid == null)
                return new ErrorDataResult<Eposta>(null, "Geçersiz eposta UUID'si.");

            Eposta? result = await _epostaDal.GetAsync(e => e.EpostaUuid == epostaUuid && e.KullaniciUuid == kullaniciUuid);
            if (result == null)
                return new ErrorDataResult<Eposta>(null, "Email bulunamadı.");
            return new SuccessDataResult<Eposta>(result, "Email başarıyla getirildi.");
        }

        public async Task<IDataResult<IEnumerable<Eposta>>> GetUserEmailsAsync(Guid kullaniciUuid)
        {
            if (kullaniciUuid == Guid.Empty)
                return new ErrorDataResult<IEnumerable<Eposta>>(new List<Eposta>(), "Geçersiz kullanıcı UUID'si.");

            IEnumerable<Eposta> result = await _epostaDal.GetAllAsync(e => e.KullaniciUuid == kullaniciUuid);
            if (result == null || !result.Any())
                return new SuccessDataResult<IEnumerable<Eposta>>(new List<Eposta>(), "Kullanıcıya ait eposta bulunamadı.");

            return new SuccessDataResult<IEnumerable<Eposta>>(result, "Kullanıcıya ait epostalar başarıyla getirildi.");
        }

        public async Task<IDataResult<Eposta>> ResetNonPrimaryEmail(Eposta nonPrimaryEmail)
        {
            if (nonPrimaryEmail == null)
                return new ErrorDataResult<Eposta>(null!, "Email bilgileri boş olamaz.");
            nonPrimaryEmail.Oncelikli = false;
            nonPrimaryEmail.GuncellemeTarihi = DateTime.UtcNow;
            await _epostaDal.UpdateAsync(nonPrimaryEmail);
            return new SuccessDataResult<Eposta>(nonPrimaryEmail, "Email önceliği başarıyla sıfırlandı.");
        }

        public async Task<IDataResult<Eposta>> ResetPrimaryEmail(Eposta newPrimaryEmail)
        {
            if (newPrimaryEmail == null)
                return new ErrorDataResult<Eposta>(null!, "Email bilgileri boş olamaz.");
            Eposta? currentPrimaryAddress = await _epostaDal.GetAsync(a => a.KullaniciUuid == newPrimaryEmail.KullaniciUuid && a.Oncelikli == true);
            if (currentPrimaryAddress != null)
            {
                currentPrimaryAddress.Oncelikli = false;
                currentPrimaryAddress.GuncellemeTarihi = DateTime.UtcNow;
                await _epostaDal.UpdateAsync(currentPrimaryAddress);
            }
            newPrimaryEmail.Oncelikli = true;
            newPrimaryEmail.GuncellemeTarihi = DateTime.UtcNow;
            await _epostaDal.UpdateAsync(newPrimaryEmail);
            return new SuccessDataResult<Eposta>(newPrimaryEmail, "Yeni öncelikli email adres başarıyla ayarlandı.");
        }

        public async Task<IResult> SetPrimaryEmail(Eposta? newPrimaryEmail)
        {
            if (newPrimaryEmail == null)
                return new ErrorResult("Yeni öncelikli email adres bilgileri boş olamaz.");

            Eposta? adres = await _epostaDal.GetAsync(a => a.EpostaUuid == newPrimaryEmail.EpostaUuid);
            if (adres == null)
                return new ErrorResult("Yeni öncelikli email adres bulunamadı.");
            adres.Oncelikli = true;
            adres.GuncellemeTarihi = DateTime.UtcNow;
            await _epostaDal.UpdateAsync(adres);
            return new SuccessResult("Yeni öncelikli email adres başarıyla ayarlandı.");
        }
    }
}
