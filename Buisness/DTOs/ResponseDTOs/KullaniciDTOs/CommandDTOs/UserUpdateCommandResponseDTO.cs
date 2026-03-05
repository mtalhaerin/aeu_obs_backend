using Business.DTOs._Generic;
using Core.Entities.Enums;

namespace Business.DTOs.ResponseDTOs.KullaniciDTOs.CommandDTOs
{
    public class UserUpdateCommandResponseDTO : CommandResponseDTOBase
    {
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
        public KullaniciTipi KullaniciTipi { get; set; } = KullaniciTipi.OGRENCI;
        public string Ad { get; set; } = string.Empty;
        public string? OrtaAd { get; set; } = null;
        public string Soyad { get; set; } = string.Empty;
        public string KurumEposta { get; set; } = string.Empty;
        public string KurumSicilNo { get; set; } = string.Empty;
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime GuncellemeTarihi { get; set; }
    }
}
