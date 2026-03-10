using Business.DTOs._Generic;

namespace Business.DTOs.RequestDTOs.FakulteIslemleriDTOs.AnadalDTOs.CommandDTOs
{
    public class AnaDalAddCommandRequestDTO : CommandRequestDTOBase
    {
        public string AnaDalAdi { get; set; } = string.Empty;
        public Guid BolumUuid { get; set; } = Guid.Empty;
        public DateTime KurulusTarihi { get; set; } = DateTime.MinValue;
    }
}