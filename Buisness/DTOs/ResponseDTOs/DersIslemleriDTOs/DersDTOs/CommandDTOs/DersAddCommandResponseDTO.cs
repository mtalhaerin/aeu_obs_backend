using Business.DTOs._Generic;

namespace Business.DTOs.ResponseDTOs.DersIslemleriDTOs.DersDTOs.CommandDTOs
{
    public class DersAddCommandResponseDTO : CommandResponseDTOBase
    {
        public Guid DersUuid { get; set; } = Guid.Empty;
        public string DersKodu { get; set; } = string.Empty;
        public string DersAdi { get; set; } = string.Empty;
        public string? Aciklama { get; set; } = null;
        public int HaftalikDersSaati { get; set; } = 0;
        public int Kredi { get; set; } = 0;
        public int Akts { get; set; } = 0;
        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;
        public DateTime GuncellemeTarihi { get; set; } = DateTime.UtcNow;
    }
}