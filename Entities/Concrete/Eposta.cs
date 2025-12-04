using Core.Entities.Abstractions;
using Entities.Enums;
using System.Collections.Generic;

namespace Entities.Concrete
{
    public class Eposta : EntityBase
    {
        public Guid EpostaUuid { get; set; } = Guid.NewGuid();
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
        public string EpostaAdresi { get; set; } = string.Empty;
        public EpostaTipi EpostaTipi { get; set; }
        public bool Oncelikli { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime GuncellemeTarihi { get; set; }

        // Reference navigation
        public Kullanici? Kullanici { get; set; }
    }
}
