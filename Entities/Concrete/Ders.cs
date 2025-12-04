using Core.Entities.Abstractions;
using System;
using System.Collections.Generic;

namespace Entities.Concrete
{
    public class Ders : EntityBase
    {
        public Guid DersUuid { get; set; } = Guid.NewGuid();
        public string DersKodu { get; set; } = string.Empty;
        public string DersAdi { get; set; } = string.Empty;
        public string Aciklama { get; set; } = string.Empty;
        public int HaftalikDersSaati { get; set; }
        public int Kredi { get; set; }
        public int Akts { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime GuncellemeTarihi { get; set; }

        // Navigation collections
        public ICollection<Sinav> Sinavlar { get; set; } = new List<Sinav>();
        public ICollection<OgrenciKayit> OgrenciKayitlari { get; set; } = new List<OgrenciKayit>();
        public ICollection<AkademisyenDersAtama> AkademisyenDersAtamalari { get; set; } = new List<AkademisyenDersAtama>();
    }
}
