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
    public interface ITelefonService
    {
        Task<IDataResult<Telefon>> GetUserTelefonByUuidAsync(Guid kullaniciUuid, Guid? TeelfonUuid);
        Task<IDataResult<IEnumerable<Telefon>>> GetUserTelefonsAsync(Guid kullaniciUuid);
    }
    public class TelefonManager : ITelefonService
    {
        private readonly ITelefonDal _telefonDal;

        public TelefonManager(ITelefonDal telefonDal)
        {
            _telefonDal = telefonDal;
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
            if (kullaniciUuid != Guid.Empty)
                return new ErrorDataResult<IEnumerable<Telefon>>(new List<Telefon>(), "Geçersiz kullanıcı UUID'si.");

            IEnumerable<Telefon> result = await _telefonDal.GetAllAsync(t => t.KullaniciUuid == kullaniciUuid);
            if (result == null || !result.Any())
                return new SuccessDataResult<IEnumerable<Telefon>>(new List<Telefon>(), "Kullanıcıya ait Telefon bulunamadı.");

            return new SuccessDataResult<IEnumerable<Telefon>>(result, "Kullanıcıya ait Telefonlar başarıyla getirildi.");
        }
    }
}
