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
    public interface IOgrenciDersKayitService
    {
        Task<IDataResult<OgrenciDersKayit>> GetByUuidAsync(Guid kayitUuid);
        Task<IDataResult<List<OgrenciDersKayit>>> GetAllAsync();
        Task<IResult> AddKayitAsync(OgrenciDersKayit kayit);
        Task<IResult> UpdateKayitAsync(OgrenciDersKayit kayit);
        Task<IResult> DeleteKayitAsync(Guid kayitUuid);
    }

    public class OgrenciDersKayitManager : IOgrenciDersKayitService
    {
        private readonly IOgrenciDersKayitDal _kayitDal;

        public OgrenciDersKayitManager(IOgrenciDersKayitDal kayitDal)
        {
            _kayitDal = kayitDal;
        }

        public async Task<IDataResult<OgrenciDersKayit>> GetByUuidAsync(Guid kayitUuid)
        {
            var result = await _kayitDal.GetAsync(k => k.KayitUuid == kayitUuid);
            return result is null
                ? new DataResult<OgrenciDersKayit>(null, false)
                : new DataResult<OgrenciDersKayit>(result, true);
        }

        public async Task<IDataResult<List<OgrenciDersKayit>>> GetAllAsync()
        {
            var result = await _kayitDal.GetAllAsync();
            return new DataResult<List<OgrenciDersKayit>>(result.ToList(), true);
        }

        public async Task<IResult> AddKayitAsync(OgrenciDersKayit kayit)
        {
            await _kayitDal.AddAsync(kayit);
            return new SuccessResult();
        }

        public async Task<IResult> UpdateKayitAsync(OgrenciDersKayit kayit)
        {
            await _kayitDal.UpdateAsync(kayit);
            return new SuccessResult();
        }

        public async Task<IResult> DeleteKayitAsync(Guid kayitUuid)
        {
            var kayit = await _kayitDal.GetAsync(k => k.KayitUuid == kayitUuid);
            if (kayit is null)
                return new ErrorResult("Kay?t bulunamad?.");
            await _kayitDal.DeleteAsync(kayit);
            return new SuccessResult();
        }
    }
}