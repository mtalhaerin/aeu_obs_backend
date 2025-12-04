using Core.Entities.Abstractions;
using Entities.Enums;
using System;

namespace Entities.Concrete
{
    public class OgrenciKayit : EntityBase
    {
        public Guid KayitUuid { get; set; } = Guid.NewGuid();
        public Guid OgrenciUuid { get; set; } = Guid.Empty;
        public Guid DersUuid { get; set; } = Guid.Empty;
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime GuncellemeTarihi { get; set; }
        public Durum Durum { get; set; }

        // Reference navigations
        public Kullanici? Ogrenci { get; set; }
        public Ders? Ders { get; set; }
    }
}
