using Core.Entities.Abstractions;

namespace Entities.Concrete.FakulteEntities
{
    public class Bolum : EntityBase
    {
        public Guid BolumUuid { get; set; } = Guid.NewGuid();
        public string BolumAdi { get; set; } = string.Empty;
        public Guid FakulteUuid { get; set; } = Guid.Empty;
        public DateTime KurulusTarihi { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime GuncellemeTarihi { get; set; }

        // Reference navigation
        public Fakulte? Fakulte { get; set; }
        public ICollection<AnaDal> Anadallar { get; set; } = new List<AnaDal>();

        public ICollection<AkademisyenBolumAtama> AkademisyenAtamalari { get; set; } = 
            new List<AkademisyenBolumAtama>();
        public ICollection<OgrenciBolumKayit> OgrenciKayitlari { get; set; } = 
            new List<OgrenciBolumKayit>();
    }
}

