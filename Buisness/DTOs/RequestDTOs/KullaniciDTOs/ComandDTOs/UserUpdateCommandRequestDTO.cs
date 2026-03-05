using Business.DTOs._Generic;
using Core.Entities.Enums;

namespace Business.DTOs.RequestDTOs.KullaniciDTOs
{
    public class UserUpdateCommandRequestDTO : CommandRequestDTOBase
    {
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
        public KullaniciTipi KullaniciTipi { get; set; } = KullaniciTipi._;
        public string? Ad { get; set; } = string.Empty;
        public string? OrtaAd { get; set; } = null;
        public string? Soyad { get; set; } = string.Empty;
        public string KurumEposta { get; set; } = string.Empty;
        public string KurumSicilNo { get; set; } = string.Empty;
    }
}

