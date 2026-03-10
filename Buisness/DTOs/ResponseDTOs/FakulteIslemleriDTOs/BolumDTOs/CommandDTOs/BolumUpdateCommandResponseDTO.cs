using Business.DTOs._Generic;

namespace Buisness.DTOs.ResponseDTOs.FakulteDTOs.CommandDTOs
{
    public class BolumUpdateCommandResponseDTO : CommandResponseDTOBase
    {
        public Guid BolumUuid { get; set; } = Guid.Empty;
        public string BolumAdi { get; set; } = string.Empty;
        public Guid FakulteUuid { get; set; } = Guid.Empty;
        public DateTime KurulusTarihi { get; set; } = DateTime.MinValue;
        public DateTime OlusturmaTarihi { get; set; } = DateTime.MinValue;
        public DateTime GuncellemeTarihi { get; set; } = DateTime.MinValue;
    }
}