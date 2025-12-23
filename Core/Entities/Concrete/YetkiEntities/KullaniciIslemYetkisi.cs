using System.ComponentModel.DataAnnotations.Schema; // Bunu ekleyin
using Core.Entities.Abstractions;
using Core.Entities.Concrete.OzlukEntities;

namespace Core.Entities.Concrete.YetkiEntities
{
    public class KullaniciIslemYetkisi : EntityBase
    {
        public Guid YetkiAtamaUuid { get; set; } = Guid.NewGuid();

        public Guid KullaniciUuid { get; set; } = Guid.Empty; // Yetkiyi Alan
        public Guid YetkiVerenUuid { get; set; } = Guid.Empty; // Yetkiyi Veren

        // FK for IslemYetkisi
        public Guid IslemYetkisiUuid { get; set; } = Guid.Empty;

        public DateTime OlusturmaTarihi { get; set; }
        public DateTime GuncellemeTarihi { get; set; }

        // --- Navigation Properties ---

        [ForeignKey("IslemYetkisiUuid")]
        public IslemYetkisi? IslemYetkisi { get; set; }

        // Bu property, Yetkiyi ALAN kullanıcıyı temsil eder.
        [ForeignKey("KullaniciUuid")]
        public Kullanici? Kullanici { get; set; }

        // Yetki Veren için ayrıca bir navigation property tanımlanmamış,
        // bu yüzden Fluent API'de ".HasOne<Kullanici>()" kullanacağız.
    }
}