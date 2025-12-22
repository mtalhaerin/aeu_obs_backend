using Core.Entities.Abstractions;
using Core.Entities.Concrete.OzlukEntities;

namespace Core.Entities.Concrete.YetkiEntities
{
    public class KullaniciIslemYetkisi : EntityBase
    {
        public Guid YetkiAtamaUuid { get; set; } = Guid.NewGuid();
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
        public Guid YetkiVerenUuid { get; set; } = Guid.Empty;
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime GuncellemeTarihi { get; set; }

        // Navigation property
        public IslemYetkisi? IslemYetkisi { get; set; }
        public Kullanici? Kullanici { get; set; }
    }
}
