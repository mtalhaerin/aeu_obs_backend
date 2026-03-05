using Business.DTOs._Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.RequestDTOs.OzlukDTOs.EmailDTOs.QueryDTOs
{
    public class OzlukEmailQueryRequestDTO : QueryRequestDTOBase
    {
        public Guid? EmailUuid { get; set; } = null;
        public Guid? KullaniciUuid { get; set; } = null;
    }
}
