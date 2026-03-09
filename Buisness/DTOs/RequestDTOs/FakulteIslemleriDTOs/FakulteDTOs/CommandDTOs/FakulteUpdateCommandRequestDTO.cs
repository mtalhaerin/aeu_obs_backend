using Business.DTOs._Generic;

namespace Business.DTOs.RequestDTOs.FakulteIslemleriDTOs.FakulteDTOs.CommandDTOs
{
    public class FakulteUpdateCommandRequestDTO : CommandRequestDTOBase
    {
        public Guid FakulteUuid { get; set; } = Guid.Empty;
        public string FakulteAdi { get; set; } = string.Empty;
        public string WebAdres { get; set; } = string.Empty;
        public DateTime KurulusTarihi { get; set; } = DateTime.MinValue;
    }
}