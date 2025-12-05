using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.DataResults;
using DataAccess.Concrete.EntityFramework.OzlukDals;
using Entities.Concrete.OzlukEntities;
using System;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public interface IKullaniciService
    {
        Task<IDataResult<Kullanici?>> GetByUuid(Guid kullaniciUuid);
        Task<IDataResult<Kullanici?>> GetByUuidAsync(Guid kullaniciUuid);
    }

    public class KullaniciManager : IKullaniciService
    {
        private readonly IKullaniciDal _kullaniciDal;

        public KullaniciManager(IKullaniciDal kullaniciDal)
        {
            _kullaniciDal = kullaniciDal;
        }

        public async Task<IDataResult<Kullanici?>> GetByUuid(Guid kullaniciUuid)
        {
            var result = _kullaniciDal.Get(k => k.KullaniciUuid == kullaniciUuid);
            if (result is null)
            {
                return new DataResult<Kullanici?>(null, false); // or include message
            }
            return new DataResult<Kullanici?>(result, true);
        }

        public async Task<IDataResult<Kullanici?>> GetByUuidAsync(Guid kullaniciUuid)
        {
            var result = await _kullaniciDal.GetAsync(k => k.KullaniciUuid == kullaniciUuid);
            if (result is null)
            {
                return new DataResult<Kullanici?>(null, false); // or include message
            }
            return new DataResult<Kullanici?>(result, true);
        }
    }
}
