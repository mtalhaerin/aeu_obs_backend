using Business.DTOs._Generic;
using Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.ResponseDTOs.OzlukDTOs.EmailDTOs.QueryDTOs
{
    public class OzlukEmailQueryResponseDTO : QueryResponseDTOBase
    {
        public Guid EpostaUuid { get; set; } = Guid.Empty;
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
        public string EpostaAdresi { get; set; } = string.Empty;
        public EpostaTipi EpostaTipi { get; set; } = EpostaTipi.DIGER;
        public bool Oncelikli { get; set; } = false;
    }

    public class OzlukEmailsQueryResponseDTO : QueryResponseDTOBase
    {
        public List<OzlukEmailQueryResponseDTO> Epostalar { get; set; } = new List<OzlukEmailQueryResponseDTO>();
    }
}
