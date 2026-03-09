using Core.Utilities.Results;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.DataResults;
using DataAccess.Concrete.EntityFramework.FakulteDals;
using Entities.Concrete.FakulteEntities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Concrete.FakulteManagers
{
    public interface IAkademisyenBolumAtamaService
    {
        Task<IDataResult<AkademisyenBolumAtama>> GetByUuidAsync(Guid atamaUuid);
        Task<IDataResult<List<AkademisyenBolumAtama>>> GetAllAsync();
        Task<IResult> AddAtamaAsync(AkademisyenBolumAtama atama);
        Task<IResult> UpdateAtamaAsync(AkademisyenBolumAtama atama);
        Task<IResult> DeleteAtamaAsync(Guid atamaUuid);
    }

    public class AkademisyenBolumAtamaManager : IAkademisyenBolumAtamaService
    {
        private readonly IAkademisyenBolumAtamaDal _atamaDal;

        public AkademisyenBolumAtamaManager(IAkademisyenBolumAtamaDal atamaDal)
        {
            _atamaDal = atamaDal;
        }

        public async Task<IDataResult<AkademisyenBolumAtama>> GetByUuidAsync(Guid atamaUuid)
        {
            var result = await _atamaDal.GetAsync(a => a.AtamaUuid == atamaUuid);
            return result is null
                ? new DataResult<AkademisyenBolumAtama>(null, false)
                : new DataResult<AkademisyenBolumAtama>(result, true);
        }

        public async Task<IDataResult<List<AkademisyenBolumAtama>>> GetAllAsync()
        {
            var result = await _atamaDal.GetAllAsync();
            var listResult = result is List<AkademisyenBolumAtama> atamaList ? atamaList : new List<AkademisyenBolumAtama>(result);
            return new DataResult<List<AkademisyenBolumAtama>>(listResult, true);
        }

        public async Task<IResult> AddAtamaAsync(AkademisyenBolumAtama atama)
        {
            await _atamaDal.AddAsync(atama);
            return new SuccessResult("Akademisyen atama başarıyla eklendi.");
        }

        public async Task<IResult> UpdateAtamaAsync(AkademisyenBolumAtama atama)
        {
            await _atamaDal.UpdateAsync(atama);
            return new SuccessResult("Akademisyen atama başarıyla güncellendi.");
        }

        public async Task<IResult> DeleteAtamaAsync(Guid atamaUuid)
        {
            var atama = await _atamaDal.GetAsync(a => a.AtamaUuid == atamaUuid);
            if (atama is null)
                return new ErrorResult("Akademisyen atama bulunamadı.");
            await _atamaDal.DeleteAsync(atama);
            return new SuccessResult("Akademisyen atama başarıyla silindi.");
        }
    }
}