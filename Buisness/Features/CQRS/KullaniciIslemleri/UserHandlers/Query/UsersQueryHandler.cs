using Business.Concrete;
using Business.Concrete.OzlukManagers;
using Business.ContextCarrier;
using Business.DTOs.ResponseDTOs.KullaniciDTOs.QueryDTOs;
using Business.DTOs.ResponseDTOs.OzlukDTOs.AdresDTOs.CommandDTOs;
using Business.DTOs.ResponseDTOs.OzlukDTOs.AdresDTOs.QueryDTOs;
using Business.Features.CQRS._Generic;
using Business.Features.CQRS._Generic.Helpers;
using Business.Features.CQRS._Generic.Response;
using Business.Features.CQRS._Generic.Secured;
using Core.Entities.Concrete.OzlukEntities;
using Core.Entities.Enums;
using Core.Utilities.Paging;
using Core.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.CQRS.KullaniciIslemleri.UserHandlers.Query
{
    public class UsersQuery : ISecuredQuery<BaseResponse<UsersQueryResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public KullaniciTipi KullaniciTipi { get; set; } = KullaniciTipi._;
        public string? Ad { get; set; } = string.Empty;
        public string? OrtaAd { get; set; } = null;
        public string? Soyad { get; set; } = string.Empty;
        public string KurumEposta { get; set; } = string.Empty;
        public string KurumSicilNo { get; set; } = string.Empty;
        public DateTime OlusturmaTarihi { get; set; } = DateTime.MinValue;
        public DateTime GuncellemeTarihi { get; set; } = DateTime.MaxValue;
        public Pager? Pager { get; set; } = null;
    }

    public class UsersQueryHandler : IQueryHandler<UsersQuery, BaseResponse<UsersQueryResponseDTO>>
    {
        private readonly IGenericHelper _genericHelper;
        private readonly IUserContext _userContext;
        private readonly IKullaniciService _kullaniciService;

        public UsersQueryHandler(
            IGenericHelper genericHelper,
            IUserContext userContext,
            IKullaniciService kullaniciServis)
        {
            _genericHelper = genericHelper;
            _userContext = userContext;
            _kullaniciService = kullaniciServis;
        }

        public async Task<BaseResponse<UsersQueryResponseDTO>> Handle(UsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                if (kullanici.KullaniciTipi != KullaniciTipi.PERSONEL)
                    return BaseResponse<UsersQueryResponseDTO>.Failure("Unauthorized", statusCode: 401);

                IDataResult<List<Kullanici>> kullanicis = await _kullaniciService.GetAllByPaged(
                    request.KullaniciTipi,
                    request.Ad,
                    request.OrtaAd,
                    request.Soyad,
                    request.KurumEposta,
                    request.KurumSicilNo,
                    request.OlusturmaTarihi,
                    request.GuncellemeTarihi,
                    request.Pager);

                if (!kullanicis.Success)
                    return BaseResponse<UsersQueryResponseDTO>.Failure(kullanicis.Message, statusCode: 400);

                UsersQueryResponseDTO response = new UsersQueryResponseDTO
                {
                    Users = kullanicis.Data.Select(k => new UserQueryResponseDTO
                    {
                        KullaniciUuid   = k.KullaniciUuid,
                        KullaniciTipi   = k.KullaniciTipi,
                        Ad              = k.Ad,
                        OrtaAd          = k.OrtaAd,
                        Soyad           = k.Soyad,
                        KurumEposta     = k.KurumEposta,
                        KurumSicilNo    = k.KurumSicilNo,
                        OlusturmaTarihi = k.OlusturmaTarihi,
                        GuncellemeTarihi= k.GuncellemeTarihi
                    }).ToList()
                };

                return BaseResponse<UsersQueryResponseDTO>.Success(response, kullanicis.Message, 200);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
