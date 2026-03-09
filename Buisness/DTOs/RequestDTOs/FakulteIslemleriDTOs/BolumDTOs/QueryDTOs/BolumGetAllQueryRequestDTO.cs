using Business.DTOs._Generic;

namespace Business.DTOs.RequestDTOs.FakulteIslemleriDTOs.BolumDTOs.QueryDTOs
{
    public class BolumGetAllQueryRequestDTO : QueryRequestDTOBase
    {
        public string? BolumAdi { get; set; } = string.Empty;
        public Guid? AnaDalUuid { get; set; } = Guid.Empty;
        public DateTime KurulusTarihi { get; set; } = DateTime.MinValue;
        public DateTime OlusturmaTarihi { get; set; } = DateTime.MinValue;
        public DateTime GuncellemeTarihi { get; set; } = DateTime.MaxValue;
    }
}
