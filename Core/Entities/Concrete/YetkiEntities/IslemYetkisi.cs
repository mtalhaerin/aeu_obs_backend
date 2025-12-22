using Core.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Concrete.YetkiEntities
{
    public class IslemYetkisi : EntityBase
    {
        public Guid IslemYetkisiUuid { get; set; } = Guid.NewGuid();
        public string YetkiAdi { get; set; } = string.Empty;
        public string Aciklama { get; set; } = string.Empty;
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime GuncellemeTarihi { get; set; }

        // Navigation property
        public ICollection<KullaniciIslemYetkisi> KullaniciIslemYetkileri { get; set; } = new List<KullaniciIslemYetkisi>();
    }
}
