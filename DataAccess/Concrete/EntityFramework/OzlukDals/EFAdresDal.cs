using Core.DataAccess;
using Core.DataAccess.EntityFramework;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete.OzlukEntities;

namespace DataAccess.Concrete.EntityFramework.OzlukDals
{
    public interface IAdresDal : IEntityRepository<Adres>
    {
    }
    public class EFAdresDal : EfEntityRepositoryBase<Adres, AEUContext>, IAdresDal
    {
    }
}
