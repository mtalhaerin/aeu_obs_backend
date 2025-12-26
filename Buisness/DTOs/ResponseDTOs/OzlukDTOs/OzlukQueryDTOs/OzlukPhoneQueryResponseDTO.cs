using Business.DTOs._Generic;
using Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.ResponseDTOs.OzlukDTOs.OzlukQueryDTOs
{
    public class OzlukPhoneQueryResponseDTO : QueryResponseDTOBase
    {
        public Guid TelefonUuid { get; set; } = Guid.Empty;
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
        public string UlkeKodu { get; set; } = string.Empty;
        public string TelefonNo { get; set; } = string.Empty;
        public TelefonTipi TelefonTipi { get; set; } = TelefonTipi.CEP;
        public bool Oncelikli { get; set; } = false;
    }

    public class OzlukPhonesQueryResponseDTO : QueryResponseDTOBase
    {
        public List<OzlukPhoneQueryResponseDTO> Telefonlar { get; set; } = new List<OzlukPhoneQueryResponseDTO>();
    }
}
