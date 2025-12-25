using Business.DTOs._Generic;
using Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.ResponseDTOs.AuthDTOs.Command
{
    public class LoginCommandResponseDTO : CommandResponseDTOBase
    {
        public string? AccessToken { get; set; } = null;
        public KullaniciTipi UserType { get; set; } = KullaniciTipi.OGRENCI;
        public string? UserName { get; set; } = null;
        public string? GivenName { get; set; } = null;
        public string? MiddleName { get; set; } = null;
        public string? Surname { get; set; } = null;
        public string? InstitutionEmail { get; set; } = null;
        public string? InstitutionRegistrationNumber { get; set; } = null;
    }
}
