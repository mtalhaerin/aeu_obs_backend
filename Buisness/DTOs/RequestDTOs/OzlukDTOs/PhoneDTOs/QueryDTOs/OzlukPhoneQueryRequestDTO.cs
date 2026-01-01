using Business.DTOs._Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.RequestDTOs.OzlukDTOs.PhoneDTOs.QueryDTOs
{
    public class OzlukPhoneQueryRequestDTO : QueryRequestDTOBase
    {
        public Guid? TelefonUuid { get; set; } = null;
    }
}
