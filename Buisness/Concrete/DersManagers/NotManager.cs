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
    public interface INotService
    {
        Task<IDataResult<Not>> GetByUuidAsync(Guid notUuid);
        Task<IDataResult<List<Not>>> GetAllAsync();
        Task<IResult> AddNotAsync(Not not);
        Task<IResult> UpdateNotAsync(Not not);
        Task<IResult> DeleteNotAsync(Guid notUuid);
    }

    public class NotManager : INotService
    {
        private readonly INotDal _notDal;

        public NotManager(INotDal notDal)
        {
            _notDal = notDal;
        }

        public async Task<IDataResult<Not>> GetByUuidAsync(Guid notUuid)
        {
            var result = await _notDal.GetAsync(n => n.NotUuid == notUuid);
            return result is null
                ? new DataResult<Not>(null, false)
                : new DataResult<Not>(result, true);
        }

        public async Task<IDataResult<List<Not>>> GetAllAsync()
        {
            var result = await _notDal.GetAllAsync();
            return new DataResult<List<Not>>(result is List<Not> list ? list : new List<Not>(result), true);
        }

        public async Task<IResult> AddNotAsync(Not not)
        {
            await _notDal.AddAsync(not);
            return new SuccessResult();
        }

        public async Task<IResult> UpdateNotAsync(Not not)
        {
            await _notDal.UpdateAsync(not);
            return new SuccessResult();
        }

        public async Task<IResult> DeleteNotAsync(Guid notUuid)
        {
            var not = await _notDal.GetAsync(n => n.NotUuid == notUuid);
            if (not is null)
                return new ErrorResult("Not not found.");
            await _notDal.DeleteAsync(not);
            return new SuccessResult();
        }
    }
}