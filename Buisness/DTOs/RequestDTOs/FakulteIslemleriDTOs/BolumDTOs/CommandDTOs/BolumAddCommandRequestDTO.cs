using Business.DTOs._Generic;

namespace Business.DTOs.RequestDTOs.FakulteIslemleriDTOs.BolumDTOs.CommandDTOs
{
    public class BolumAddCommandRequestDTO : CommandRequestDTOBase
    {
        public string BolumAdi { get; set; } = string.Empty;
        public Guid AnaDalUuid { get; set; } = Guid.Empty;
        public DateTime KurulusTarihi { get; set; } = DateTime.MinValue;
    }
}