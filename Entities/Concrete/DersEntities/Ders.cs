using Core.Entities.Abstractions;
using System;
using System.Collections.Generic;

namespace Entities.Concrete.DersEntities
{
    public class Ders : EntityBase
    {
        public Guid DersUuid { get; set; } = Guid.NewGuid();
        public string DersKodu { get; set; } = string.Empty;
        public string DersAdi { get; set; } = string.Empty;
        public string? Aciklama { get; set; } = null;
        public int HaftalikDersSaati { get; set; } = 0;
        public int Kredi { get; set; } = 0;
        public int Akts { get; set; } = 0;
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime GuncellemeTarihi { get; set; }

        // Navigation collections
        public ICollection<Sinav> Sinavlar { get; set; } = new List<Sinav>();
        public ICollection<OgrenciDersKayit> OgrenciKayitlari { get; set; } = new List<OgrenciDersKayit>();
        public ICollection<AkademisyenDersAtama> AkademisyenDersAtamalari { get; set; } = new List<AkademisyenDersAtama>();
    }
}
