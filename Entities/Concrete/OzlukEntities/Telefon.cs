using Core.Entities.Abstractions;
using Entities.Enums;
using System;

namespace Entities.Concrete.OzlukEntities
{
    public class Telefon : EntityBase
    {
        public Guid TelefonUuid { get; set; } = Guid.NewGuid();
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
        public string UlkeKodu { get; set; } = string.Empty;
        public string TelefonNo { get; set; } = string.Empty;
        public TelefonTipi TelefonTipi { get; set; } = TelefonTipi.CEP;
        public bool Oncelikli { get; set; } = false;
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime GuncellemeTarihi { get; set; }

        // Reference navigation
        public Kullanici? Kullanici { get; set; }
    }
}
