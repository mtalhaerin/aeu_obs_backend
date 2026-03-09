using Business.DTOs._Generic;

namespace Business.DTOs.RequestDTOs.DersDTOs.DersIslemleriDTOs.QueryDTOs
{
    public class DersGetAllQueryRequestDTO : QueryRequestDTOBase
    {
        public string? DersKodu { get; set; } = string.Empty;
        public string? DersAdi { get; set; } = string.Empty;
        public int? Kredi { get; set; } = null;
        public int? Akts { get; set; } = null;
        public int? Page { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
        public DateTime OlusturmaTarihi { get; set; } = DateTime.MinValue;
        public DateTime GuncellemeTarihi { get; set; } = DateTime.MaxValue;
    }
}