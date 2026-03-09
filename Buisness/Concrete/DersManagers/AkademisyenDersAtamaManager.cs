using Core.Utilities.Results;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.DataResults;
using DataAccess.Concrete.EntityFramework.DersDals;
using Entities.Concrete.DersEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Concrete.DersManagers
{
    public interface IAkademisyenDersAtamaService
    {
        Task<IDataResult<AkademisyenDersAtama>> GetByUuidAsync(Guid atamaUuid);
        Task<IDataResult<List<AkademisyenDersAtama>>> GetAllAsync();
        Task<IResult> AddAtamaAsync(AkademisyenDersAtama atama);
        Task<IResult> UpdateAtamaAsync(AkademisyenDersAtama atama);
        Task<IResult> DeleteAtamaAsync(Guid atamaUuid);
    }

    public class AkademisyenDersAtamaManager : IAkademisyenDersAtamaService
    {
        private readonly IAkademisyenDersAtamaDal _atamaDal;

        public AkademisyenDersAtamaManager(IAkademisyenDersAtamaDal atamaDal)
        {
            _atamaDal = atamaDal;
        }

        public async Task<IDataResult<AkademisyenDersAtama>> GetByUuidAsync(Guid atamaUuid)
        {
            var result = await _atamaDal.GetAsync(a => a.AtamaUuid == atamaUuid);
            return result is null
                ? new DataResult<AkademisyenDersAtama>(null, false)
                : new DataResult<AkademisyenDersAtama>(result, true);
        }

        public async Task<IDataResult<List<AkademisyenDersAtama>>> GetAllAsync()
        {
            var result = await _atamaDal.GetAllAsync();
            return new DataResult<List<AkademisyenDersAtama>>(result.ToList(), true);
        }

        public async Task<IResult> AddAtamaAsync(AkademisyenDersAtama atama)
        {
            await _atamaDal.AddAsync(atama);
            return new SuccessResult();
        }

        public async Task<IResult> UpdateAtamaAsync(AkademisyenDersAtama atama)
        {
            await _atamaDal.UpdateAsync(atama);
            return new SuccessResult();
        }

        public async Task<IResult> DeleteAtamaAsync(Guid atamaUuid)
        {
            var atama = await _atamaDal.GetAsync(a => a.AtamaUuid == atamaUuid);
            if (atama is null)
                return new ErrorResult("Atama not found.");
            await _atamaDal.DeleteAsync(atama);
            return new SuccessResult();
        }
    }
}