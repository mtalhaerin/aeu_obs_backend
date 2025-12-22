using Business.DTOs._Generic;
using Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.ResponseDTOs.AuthDTOs
{
    public class LoginResponseDTO : ResponseDTOBase
    {
        public string? AccessToken { get; set; } = null;
    }
}
