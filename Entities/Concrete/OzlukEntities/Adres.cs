using Core.Entities.Abstractions;
using System;

namespace Entities.Concrete.OzlukEntities
{
    public class Adres : EntityBase
    {
        public Guid AdresUuid { get; set; } = Guid.NewGuid();
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
        public string Sokak { get; set; } = string.Empty;
        public string Sehir { get; set; } = string.Empty;
        public string Ilce { get; set; } = string.Empty;
        public string PostaKodu { get; set; } = string.Empty;
        public string Ulke { get; set; } = string.Empty;
        public bool Oncelikli { get; set; } = false;
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime GuncellemeTarihi { get; set; }

        // Reference navigation
        public Kullanici? Kullanici { get; set; }
    }
}
