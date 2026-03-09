using Business.DTOs._Generic;

namespace Business.DTOs.ResponseDTOs.FakulteIslemleriDTOs.AnadalDTOs.CommandDTOs
{
    public class AnaDalUpdateCommandResponseDTO : CommandResponseDTOBase
    {
        public Guid AnaDalUuid { get; set; } = Guid.Empty;
        public string AnaDalAdi { get; set; } = string.Empty;
        public Guid FakulteUuid { get; set; } = Guid.Empty;
        public DateTime KurulusTarihi { get; set; } = DateTime.MinValue;
        public DateTime OlusturmaTarihi { get; set; } = DateTime.MinValue;
        public DateTime GuncellemeTarihi { get; set; } = DateTime.MinValue;
    }
}