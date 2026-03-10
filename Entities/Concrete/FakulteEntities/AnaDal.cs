using Core.Entities.Abstractions;

namespace Entities.Concrete.FakulteEntities
{
    public class AnaDal : EntityBase
    {
     public Guid AnaDalUuid { get; set; } = Guid.NewGuid();
        public string AnaDalAdi { get; set; } = string.Empty;
        public Guid BolumUuid { get; set; } = Guid.Empty;
        public DateTime KurulusTarihi { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime GuncellemeTarihi { get; set; }

        // Reference navigation
        public Bolum? Bolum { get; set; }
        //public ICollection<Bolum> Bolumler { get; set; } = new List<Bolum>();
    }
}

