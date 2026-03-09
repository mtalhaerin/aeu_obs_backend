using Business.DTOs._Generic;

namespace Business.DTOs.RequestDTOs.FakulteIslemleriDTOs.AnadalDTOs.QueryDTOs
{
    public class AnaDalGetAllQueryRequestDTO : QueryRequestDTOBase
    {
        public string? AnaDalAdi { get; set; } = string.Empty;
        public Guid? FakulteUuid { get; set; } = Guid.Empty;
        public DateTime KurulusTarihi { get; set; } = DateTime.MinValue;
        public DateTime OlusturmaTarihi { get; set; } = DateTime.MinValue;
        public DateTime GuncellemeTarihi { get; set; } = DateTime.MaxValue;
    }
}