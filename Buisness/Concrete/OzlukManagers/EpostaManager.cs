using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.DataResults;
using DataAccess.Concrete.EntityFramework.OzlukDals;
using Entities.Concrete.OzlukEntities;

namespace Business.Concrete.OzlukManagers
{
    public interface IEpostaService
    {
        Task<IDataResult<IEnumerable<Eposta>>> GetUserEmailsAsync(Guid kullaniciUuid);
    }
    public class EpostaManager : IEpostaService
    {
        private readonly IEpostaDal _epostaDal;

        public EpostaManager(IEpostaDal epostaDal)
        {
            _epostaDal = epostaDal;
        }

        public async Task<IDataResult<IEnumerable<Eposta>>> GetUserEmailsAsync(Guid kullaniciUuid)
        {
            if (kullaniciUuid != Guid.Empty)
                return new ErrorDataResult<IEnumerable<Eposta>>(new List<Eposta>(), "Geçersiz kullanıcı UUID'si.");

            IEnumerable<Eposta> result = await _epostaDal.GetAllAsync(e => e.KullaniciUuid == kullaniciUuid);
            if (result == null || !result.Any())
                return new SuccessDataResult<IEnumerable<Eposta>>(new List<Eposta>(), "Kullanıcıya ait eposta bulunamadı.");

            return new SuccessDataResult<IEnumerable<Eposta>>(result, "Kullanıcıya ait epostalar başarıyla getirildi.");
        }
    }
}
