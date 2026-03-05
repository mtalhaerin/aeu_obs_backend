using Business.Concrete;
using Business.Concrete.OzlukManagers;
using Business.ContextCarrier;
using Business.DTOs.ResponseDTOs.KullaniciDTOs.CommandDTOs;
using Business.DTOs.ResponseDTOs.KullaniciDTOs.QueryDTOs;
using Business.DTOs.ResponseDTOs.OzlukDTOs.AdresDTOs.CommandDTOs;
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

namespace Business.Features.CQRS.KullaniciIslemleri.UserHandlers.Command
{
    public class UserAddCommand : ISecuredCommand<BaseResponse<UserAddCommandResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public KullaniciTipi KullaniciTipi { get; set; } = KullaniciTipi._;
        public string? Ad { get; set; } = string.Empty;
        public string? OrtaAd { get; set; } = null;
        public string? Soyad { get; set; } = string.Empty;
        public string KurumEposta { get; set; } = string.Empty;
        public string KurumSicilNo { get; set; } = string.Empty;
    }
    public class UserAddCommandHandler : ICommandHandler<UserAddCommand, BaseResponse<UserAddCommandResponseDTO>>
    {
        private readonly IGenericHelper _genericHelper;
        private readonly IUserContext _userContext;
        private readonly IKullaniciService _kullaniciServis;
        public UserAddCommandHandler(
            IGenericHelper genericHelper,
            IUserContext userContext,
            IKullaniciService kullaniciServis)
        {
            _genericHelper = genericHelper;
            _userContext = userContext;
            _kullaniciServis = kullaniciServis;
        }

        public async Task<BaseResponse<UserAddCommandResponseDTO>> Handle(UserAddCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                if (kullanici.KullaniciTipi != KullaniciTipi.PERSONEL)
                    return BaseResponse<UserAddCommandResponseDTO>.Failure("Unauthorized", statusCode: 401);

                IDataResult<Kullanici> result = await _kullaniciServis.GetByEmailAsync(request.KurumEposta);
                if (result.Success)
                    return BaseResponse<UserAddCommandResponseDTO>.Failure("Bu e-posta adresi zaten kayıtlı.", statusCode: 400);

                result = await _kullaniciServis.GetBySicilNoAsync(request.KurumSicilNo);
                if (result.Success)
                    return BaseResponse<UserAddCommandResponseDTO>.Failure("Bu sicil numarası zaten kayıtlı.", statusCode: 400);

                Kullanici newKullanici = new Kullanici
                    {
                    KullaniciUuid = Guid.NewGuid(),
                    KullaniciTipi = request.KullaniciTipi,
                    Ad = request.Ad,
                    OrtaAd = request.OrtaAd,
                    Soyad = request.Soyad,
                    KurumEposta = request.KurumEposta,
                    KurumSicilNo = request.KurumSicilNo,
                    //TODO: Parola oluşturma işlemi için ayrı bir servis veya yardımcı sınıf kullanılabilir. Şimdilik sabit bir parola kullanılıyor.
                    ParolaHash = "2mNt2Qj4z7fiJIr7hTgFpULN6JWRQdwCvz5d4HZ60Mk=",
                    ParolaTuz = "5lcuF3NnFPDpXMilFBzmkVl1Vd+pjd+6Fx5PBDNEVd4PvZDed7Ygf94J0ELG9wL/j06WocE2lflfH+foX5/oBw==",
                    OlusturmaTarihi = DateTime.UtcNow,
                    GuncellemeTarihi = DateTime.UtcNow
                };

                await _kullaniciServis.AddKullaniciAsync(newKullanici); 
                if (!result.Success)
                    return BaseResponse<UserAddCommandResponseDTO>.Failure(result.Message, statusCode: 500);

                return BaseResponse<UserAddCommandResponseDTO>.Success(new UserAddCommandResponseDTO
                {
                    KullaniciUuid = newKullanici.KullaniciUuid,
                    KullaniciTipi = newKullanici.KullaniciTipi,
                    Ad = newKullanici.Ad ?? string.Empty,
                    OrtaAd = newKullanici.OrtaAd,
                    Soyad = newKullanici.Soyad ?? string.Empty,
                    KurumEposta = newKullanici.KurumEposta,
                    KurumSicilNo = newKullanici.KurumSicilNo,
                    OlusturmaTarihi = newKullanici.OlusturmaTarihi,
                    GuncellemeTarihi = newKullanici.GuncellemeTarihi
                }, "Kullanıcı başarıyla eklendi.", 201);
            }
            catch (Exception)
            {

                throw;
            }
            return BaseResponse<UserAddCommandResponseDTO>.Success(null, null, 200);
        }
    }
}

//B204
