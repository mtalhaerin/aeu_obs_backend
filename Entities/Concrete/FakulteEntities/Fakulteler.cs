using Core.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.FakulteEntities
{
    public class Fakulte : EntityBase
    {
        public Guid FakulteUuid { get; set; } = Guid.NewGuid();
        public string FakulteAdi { get; set; } = string.Empty;
        public string WebAdres { get; set; } = string.Empty;
        public DateTime KurulusTarihi { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime GuncellemeTarihi { get; set; }

        // Navigation collections
        public ICollection<Bolum> Bolumler { get; set; } = new
        List<Bolum>();

    }
}

