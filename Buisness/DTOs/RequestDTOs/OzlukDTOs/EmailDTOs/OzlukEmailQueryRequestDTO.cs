using Business.DTOs._Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.RequestDTOs.OzlukDTOs.EmailDTOs
{
    public class OzlukEmailQueryRequestDTO : QueryRequestDTOBase
    {
        public string? Token { get; set; } = null;
        public Guid? EmailUuid { get; set; } = null;
    }
}
