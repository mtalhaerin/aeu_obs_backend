using Core.Entities.Enums;
using Core.Utilities.Results;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.DataResults;
using DataAccess.Concrete.EntityFramework.OzlukDals;
using Entities.Concrete.OzlukEntities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete.OzlukManagers
{
    public interface ITelefonService
    {
        Task<IDataResult<Telefon>> GetUserTelefonByUuidAsync(Guid kullaniciUuid, Guid? TeelfonUuid);
        Task<IDataResult<IEnumerable<Telefon>>> GetUserTelefonsAsync(Guid kullaniciUuid);
        Task<IResult> DeleteUserPhoneAsync(Guid kullaniciUuid, Guid telefonUuid);
        Task<IDataResult<Telefon>> AddUserPhoneAsync(Telefon telefon);
        Task<IDataResult<Telefon>> UpdateUserPhoneAsync(Telefon telefon);
    }
    public class TelefonManager : ITelefonService
    {
        private readonly ITelefonDal _telefonDal;

        public TelefonManager(ITelefonDal telefonDal)
        {
            _telefonDal = telefonDal;
        }

        public async Task<IDataResult<Telefon>> AddUserPhoneAsync(Telefon telefon)
        {
            if (telefon.KullaniciUuid== Guid.Empty)
                return new ErrorDataResult<Telefon>(null!, "Geçersiz kullanıcı UUID'si.");
            
            await _telefonDal.AddAsync(telefon);
            return new SuccessDataResult<Telefon>(telefon, "Telefon başarıyla eklendi.");

        }

        public async Task<IResult> DeleteUserPhoneAsync(Guid kullaniciUuid, Guid telefonUuid)
        {
            if (telefonUuid == Guid.Empty || kullaniciUuid == Guid.Empty)
                return new ErrorResult("Geçersiz telefon UUID'si.");

            Telefon? telefonToDelete = await _telefonDal.GetAsync(a => a.TelefonUuid == telefonUuid && a.KullaniciUuid == kullaniciUuid);
            if (telefonToDelete == null)
                return new ErrorResult("Telefon bulunamadı.");

            await _telefonDal.DeleteAsync(telefonToDelete);

            return new SuccessResult("Telefon başarıyla silindi.");
        }

        public async Task<IDataResult<Telefon>> GetUserTelefonByUuidAsync(Guid kullaniciUuid, Guid? telefonUuid)
        {
            if (telefonUuid == Guid.Empty || telefonUuid == null)
                return new ErrorDataResult<Telefon>(null!, "Geçersiz Telefon UUID'si.");
            Telefon? result = await _telefonDal.GetAsync(t => t.TelefonUuid == telefonUuid && t.KullaniciUuid == kullaniciUuid);
            if (result == null)
                return new ErrorDataResult<Telefon>(null!, "Telefon bulunamadı.");
            return new SuccessDataResult<Telefon>(result, "Telefon başarıyla getirildi.");
        }

        public async Task<IDataResult<IEnumerable<Telefon>>> GetUserTelefonsAsync(Guid kullaniciUuid)
        {
            if (kullaniciUuid == Guid.Empty)
                return new ErrorDataResult<IEnumerable<Telefon>>(new List<Telefon>(), "Geçersiz kullanıcı UUID'si.");

            IEnumerable<Telefon> result = await _telefonDal.GetAllAsync(t => t.KullaniciUuid == kullaniciUuid);
            if (result == null || !result.Any())
                return new SuccessDataResult<IEnumerable<Telefon>>(new List<Telefon>(), "Kullanıcıya ait Telefon bulunamadı.");

            return new SuccessDataResult<IEnumerable<Telefon>>(result, "Kullanıcıya ait Telefonlar başarıyla getirildi.");
        }

        public async Task<IDataResult<Telefon>> UpdateUserPhoneAsync(Telefon telefon)
        {
            if (telefon.TelefonUuid == Guid.Empty || telefon.KullaniciUuid == Guid.Empty)
                return new ErrorDataResult<Telefon>(null!, "Geçersiz Telefon veya Kullanıcı UUID'si.");

            Telefon? existingTelefon = _telefonDal.GetAsync(t => t.TelefonUuid == telefon.TelefonUuid && t.KullaniciUuid == telefon.KullaniciUuid).Result;
            if (existingTelefon == null)
                return new ErrorDataResult<Telefon>(null!, "Telefon bulunamadı.");

            await _telefonDal.UpdateAsync(telefon);
            return new SuccessDataResult<Telefon>(telefon, "Telefon başarıyla güncellendi.");
        }
    }
}
