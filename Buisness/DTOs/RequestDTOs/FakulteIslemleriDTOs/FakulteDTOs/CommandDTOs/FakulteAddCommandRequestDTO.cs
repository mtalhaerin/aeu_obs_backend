using Business.DTOs._Generic;

namespace Business.DTOs.RequestDTOs.FakulteIslemleriDTOs.FakulteDTOs.CommandDTOs
{
    public class FakulteAddCommandRequestDTO : CommandRequestDTOBase
    {
        public string FakulteAdi { get; set; } = string.Empty;
        public string WebAdres { get; set; } = string.Empty;
        public DateTime KurulusTarihi { get; set; } = DateTime.MinValue;
    }
}