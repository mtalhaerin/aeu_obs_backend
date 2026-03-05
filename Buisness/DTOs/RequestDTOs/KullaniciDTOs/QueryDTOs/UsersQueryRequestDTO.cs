using Business.DTOs._Generic;
using Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.RequestDTOs.KullaniciDTOs.QueryDTOs
{
    public class UsersQueryRequestDTO : QueryRequestDTOBase
    {
        public KullaniciTipi KullaniciTipi { get; set; } = KullaniciTipi._;
        public string? Ad { get; set; } = string.Empty;
        public string? OrtaAd { get; set; } = null;
        public string? Soyad { get; set; } = string.Empty;
        public string KurumEposta { get; set; } = string.Empty;
        public string KurumSicilNo { get; set; } = string.Empty;
        public DateTime OlusturmaTarihi { get; set; } = DateTime.MinValue;
        public DateTime GuncellemeTarihi { get; set; } = DateTime.MaxValue;
    }
}
