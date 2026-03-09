using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.DTOs._Generic;

namespace Business.DTOs.RequestDTOs.FakulteIslemleriDTOs.BolumDTOs.QueryDTOs
{
    public class BolumQueryRequestDTO : QueryRequestDTOBase
    {
        public Guid BolumUuid { get; set; } = Guid.Empty;
    }
}
