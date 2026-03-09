using Business.DTOs._Generic;

namespace Business.DTOs.RequestDTOs.DersDTOs.DersIslemleriDTOs.CommandDTOs
{
    public class DersUpdateCommandRequestDTO : CommandRequestDTOBase
    {
        public Guid DersUuid { get; set; } = Guid.Empty;
        public string DersKodu { get; set; } = string.Empty;
        public string DersAdi { get; set; } = string.Empty;
        public string? Aciklama { get; set; } = null;
        public int HaftalikDersSaati { get; set; } = 0;
        public int Kredi { get; set; } = 0;
        public int Akts { get; set; } = 0;
    }
}