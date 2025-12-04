using System;
using Core.Entities.Abstractions;

namespace Entities.Concrete
{
    public class AkademisyenDersAtama : EntityBase
    {
        public Guid AtamaUuid { get; set; } = Guid.NewGuid();
        public Guid AkademisyenUuid { get; set; } = Guid.Empty;
        public Guid DersUuid { get; set; } = Guid.Empty;
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime GuncellemeTarihi { get; set; }

        // Reference navigations
        public Kullanici? Akademisyen { get; set; }
        public Ders? Ders { get; set; }
    }
}
