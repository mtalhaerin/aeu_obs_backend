using Business.DTOs._Generic;

namespace Business.DTOs.ResponseDTOs.OzlukDTOs.AdresDTOs.CommandDTOs
{
    public class OzlukAdresUpdateCommandResponseDTO : CommandResponseDTOBase
    {
        public Guid AdresUuid { get; set; } = Guid.Empty;
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
        public string Sokak { get; set; } = string.Empty;
        public string Sehir { get; set; } = string.Empty;
        public string Ilce { get; set; } = string.Empty;
        public string PostaKodu { get; set; } = string.Empty;
        public string Ulke { get; set; } = string.Empty;
        public bool Oncelikli { get; set; } = false;
    }
}
