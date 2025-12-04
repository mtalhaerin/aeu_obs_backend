using Core.Entities.Abstractions;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities.Concrete
{
    public class Kullanici : EntityBase
    {
        public Guid KullaniciUuid { get; set; } = Guid.NewGuid();
        public string KullaniciTipi { get; set; } = string.Empty;
        public string Ad { get; set; } = string.Empty;
        public string OrtaAd { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public string KurumEposta { get; set; } = string.Empty;
        public string KurumSicilNo { get; set; } = string.Empty;
        public string ParolaHash { get; set; } = string.Empty;
        public string ParolaTuz { get; set; } = string.Empty;
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime GuncellemeTarihi { get; set; }
    }
}
