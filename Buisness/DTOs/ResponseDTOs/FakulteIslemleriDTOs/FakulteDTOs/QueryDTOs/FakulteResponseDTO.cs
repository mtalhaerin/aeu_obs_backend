using Business.DTOs._Generic;

namespace Business.DTOs.ResponseDTOs.FakulteIslemleriDTOs.FakulteDTOs.QueryDTOs
{
    public class FakulteResponseDTO : QueryResponseDTOBase
    {
        public Guid FakulteUuid { get; set; } = Guid.Empty;
        public string FakulteAdi { get; set; } = string.Empty;
        public string WebAdres { get; set; } = string.Empty;
        public DateTime KurulusTarihi { get; set; } = DateTime.MinValue;
        public DateTime OlusturmaTarihi { get; set; } = DateTime.MinValue;
        public DateTime GuncellemeTarihi { get; set; } = DateTime.MinValue;
    }
}