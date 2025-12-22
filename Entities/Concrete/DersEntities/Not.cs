using Core.Entities.Abstractions;
using Core.Entities.Concrete.OzlukEntities;
using System;

namespace Entities.Concrete.DersEntities
{
    public class Not : EntityBase
    {
        public Guid NotUuid { get; set; } = Guid.NewGuid();
        public Guid SinavUuid { get; set; } = Guid.Empty;
        public Guid OgrenciUuid { get; set; } = Guid.Empty;
        public int AlinanPuan { get; set; } = 0;
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime GuncellemeTarihi { get; set; }

        // Reference navigations
        public Sinav? Sinav { get; set; }
        public Kullanici? Ogrenci { get; set; }
    }
}
