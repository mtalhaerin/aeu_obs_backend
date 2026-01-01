using Core.Entities.Abstractions;
using Core.Entities.Enums;
using Core.Entities.Concrete.OzlukEntities;
using System.Collections.Generic;

namespace Entities.Concrete.OzlukEntities
{
    public class Eposta : EntityBase
    {
        public Guid EpostaUuid { get; set; } = Guid.NewGuid();
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
        public string EpostaAdresi { get; set; } = string.Empty;
        public EpostaTipi EpostaTipi { get; set; } = EpostaTipi.DIGER;
        public bool Oncelikli { get; set; } = false;
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime GuncellemeTarihi { get; set; }

        // Reference navigation
        public Kullanici? Kullanici { get; set; }
    }
}
