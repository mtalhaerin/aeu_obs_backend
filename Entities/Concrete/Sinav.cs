using Core.Entities.Abstractions;
using Entities.Enums;
using System;
using System.Collections.Generic;

namespace Entities.Concrete
{
    public class Sinav : EntityBase
    {
        public Guid SinavUuid { get; set; } = Guid.NewGuid();
        public Guid DersUuid { get; set; } = Guid.Empty;
        public SinavTipi SinavTipi { get; set; } = SinavTipi.QUIZ;
        public DateTime SinavTarih { get; set; } = DateTime.UtcNow; // UTC+0
        public int ToplamPuan { get; set; } = 100;
        public decimal SinavAgirligi { get; set; } = 0;
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime GuncellemeTarihi { get; set; }

        // Reference navigation
        public Ders? Ders { get; set; }

        // Navigation collection
        public ICollection<Not> Notlar { get; set; } = new List<Not>();
    }
}
