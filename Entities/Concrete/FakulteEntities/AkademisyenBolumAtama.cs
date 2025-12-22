using Core.Entities.Abstractions;
using Core.Entities.Concrete.OzlukEntities;

namespace Entities.Concrete.FakulteEntities
{
    public class AkademisyenBolumAtama : EntityBase
    {
        public Guid AtamaUuid { get; set; } = Guid.NewGuid();
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
        public Guid BolumUuid { get; set; } = Guid.Empty;
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime GuncellemeTarihi { get; set; }
        // Reference navigations
        public Kullanici? Akademisyen { get; set; }
        public Bolum? Bolum { get; set; }

    }
}

