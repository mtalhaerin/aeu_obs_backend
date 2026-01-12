using Business.DTOs._Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.RequestDTOs.OzlukDTOs.AdresDTOs.CommandDTOs
{
    public class OzlukAddresAddCommandRequestDTO : CommandRequestDTOBase
    {
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
        public string Sokak { get; set; } = string.Empty;
        public string Sehir { get; set; } = string.Empty;
        public string Ilce { get; set; } = string.Empty;
        public string PostaKodu { get; set; } = string.Empty;
        public string Ulke { get; set; } = string.Empty;
        public bool Oncelikli { get; set; } = false;
    }
}
