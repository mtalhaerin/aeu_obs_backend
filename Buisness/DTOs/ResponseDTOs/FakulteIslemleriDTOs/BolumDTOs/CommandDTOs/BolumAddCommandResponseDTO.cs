using Business.DTOs._Generic;

namespace Business.DTOs.ResponseDTOs.FakulteIslemleriDTOs.BolumDTOs.CommandDTOs
{
    public class BolumAddCommandResponseDTO : CommandResponseDTOBase
    {
        public Guid BolumUuid { get; set; } = Guid.Empty;
        public string BolumAdi { get; set; } = string.Empty;
        public Guid FakulteUuid { get; set; } = Guid.Empty;
        public DateTime KurulusTarihi { get; set; } = DateTime.MinValue;
        public DateTime OlusturmaTarihi { get; set; } = DateTime.MinValue;
        public DateTime GuncellemeTarihi { get; set; } = DateTime.MinValue;
    }
}