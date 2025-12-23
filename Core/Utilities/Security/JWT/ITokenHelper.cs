using Core.Entities.Concrete.OzlukEntities;
using Core.Entities.Concrete.YetkiEntities;

namespace Core.Utilities.Security.JWT
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(Kullanici kullanici, IEnumerable<KullaniciIslemYetkisi>? islemYetkileri);
    }
}
