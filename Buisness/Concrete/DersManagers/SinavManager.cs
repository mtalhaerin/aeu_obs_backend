using Core.Utilities.Results;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.DataResults;
using DataAccess.Concrete.EntityFramework.DersDals;
using Entities.Concrete.DersEntities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Concrete.DersManagers
{
    public interface ISinavService
    {
        Task<IDataResult<Sinav>> GetByUuidAsync(Guid sinavUuid);
        Task<IDataResult<List<Sinav>>> GetAllAsync();
        Task<IResult> AddSinavAsync(Sinav sinav);
        Task<IResult> UpdateSinavAsync(Sinav sinav);
        Task<IResult> DeleteSinavAsync(Guid sinavUuid);
    }

    public class SinavManager : ISinavService
    {
        private readonly ISinavDal _sinavDal;

        public SinavManager(ISinavDal sinavDal)
        {
            _sinavDal = sinavDal;
        }

        public async Task<IDataResult<Sinav>> GetByUuidAsync(Guid sinavUuid)
        {
            var result = await _sinavDal.GetAsync(s => s.SinavUuid == sinavUuid);
            return result is null
                ? new DataResult<Sinav>(null, false)
                : new DataResult<Sinav>(result, true);
        }

        public async Task<IDataResult<List<Sinav>>> GetAllAsync()
        {
            var result = await _sinavDal.GetAllAsync();
            return new DataResult<List<Sinav>>(result is List<Sinav> list ? list : new List<Sinav>(result), true);
        }

        public async Task<IResult> AddSinavAsync(Sinav sinav)
        {
            await _sinavDal.AddAsync(sinav);
            return new SuccessResult();
        }

        public async Task<IResult> UpdateSinavAsync(Sinav sinav)
        {
            await _sinavDal.UpdateAsync(sinav);
            return new SuccessResult();
        }

        public async Task<IResult> DeleteSinavAsync(Guid sinavUuid)
        {
            var sinav = await _sinavDal.GetAsync(s => s.SinavUuid == sinavUuid);
            if (sinav is null)
                return new ErrorResult("S?nav bulunamad?.");
            await _sinavDal.DeleteAsync(sinav);
            return new SuccessResult();
        }
    }
}