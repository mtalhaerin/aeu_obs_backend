using Business.Concrete;
using Business.Concrete.OzlukManagers;
using Business.ContextCarrier;
using Business.DTOs.ResponseDTOs.KullaniciDTOs.CommandDTOs;
using Business.DTOs.ResponseDTOs.OzlukDTOs.AdresDTOs.CommandDTOs;
using Business.Features.CQRS._Generic;
using Business.Features.CQRS._Generic.Helpers;
using Business.Features.CQRS._Generic.Response;
using Business.Features.CQRS._Generic.Secured;
using Core.Entities.Concrete.OzlukEntities;
using Core.Entities.Enums;
using Core.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.CQRS.KullaniciIslemleri.UserHandlers.Command
{
    public class UserUpdateCommand : ISecuredCommand<BaseResponse<UserUpdateCommandResponseDTO>>
    {
        public string? Authorization { get; set; } = null;
        public Guid KullaniciUuid { get; set; } = Guid.Empty;
        public KullaniciTipi KullaniciTipi { get; set; } = KullaniciTipi._;
        public string? Ad { get; set; } = string.Empty;
        public string? OrtaAd { get; set; } = null;
        public string? Soyad { get; set; } = string.Empty;
        public string KurumEposta { get; set; } = string.Empty;
        public string KurumSicilNo { get; set; } = string.Empty;
    }
    public class UserUpdateCommandHandler : ICommandHandler<UserUpdateCommand, BaseResponse<UserUpdateCommandResponseDTO>>
    {
        private readonly IGenericHelper _genericHelper;
        private readonly IUserContext _userContext;
        private readonly IKullaniciService _kullaniciServis;

        public UserUpdateCommandHandler(
            IGenericHelper genericHelper,
            IUserContext userContext,
            IKullaniciService kullaniciServis)
        {
            _genericHelper = genericHelper;
            _userContext = userContext;
            _kullaniciServis = kullaniciServis;
        }

        public async Task<BaseResponse<UserUpdateCommandResponseDTO>> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string token = _userContext.Token;
                Kullanici kullanici = _userContext.CurrentUser;

                if (kullanici.KullaniciTipi != KullaniciTipi.PERSONEL)
                    return BaseResponse<UserUpdateCommandResponseDTO>.Failure("Unauthorized", statusCode: 401);

                IDataResult<Kullanici> existingKullaniciResult = await _kullaniciServis.GetByUuidAsync(request.KullaniciUuid);
                if (!existingKullaniciResult.Success)
                    return BaseResponse<UserUpdateCommandResponseDTO>.Failure("Kullanıcı bulunamadı", statusCode: 404);

                Kullanici existingKullanici = existingKullaniciResult.Data;
                existingKullanici.KullaniciTipi = request.KullaniciTipi;
                existingKullanici.Ad = request.Ad;
                existingKullanici.OrtaAd = request.OrtaAd;
                existingKullanici.Soyad = request.Soyad;
                existingKullanici.KurumEposta = request.KurumEposta;
                existingKullanici.KurumSicilNo = request.KurumSicilNo;

                IResult updateResult = await _kullaniciServis.UpdateKullaniciAsync(existingKullanici);
                if (!updateResult.Success)
                    return BaseResponse<UserUpdateCommandResponseDTO>.Failure("Kullanıcı güncellenemedi", statusCode: 500);

                return BaseResponse<UserUpdateCommandResponseDTO>.Success(new UserUpdateCommandResponseDTO
                {
                    KullaniciUuid = existingKullanici.KullaniciUuid,
                    Ad = existingKullanici.Ad,
                    OrtaAd = existingKullanici.OrtaAd,
                    Soyad = existingKullanici.Soyad,
                    KullaniciTipi = existingKullanici.KullaniciTipi,
                    KurumEposta = existingKullanici.KurumEposta,
                    KurumSicilNo = existingKullanici.KurumSicilNo,
                    OlusturmaTarihi = existingKullanici.OlusturmaTarihi,
                    GuncellemeTarihi = existingKullanici.GuncellemeTarihi
                }, "Kullanıcı başarıyla güncellendi", 200);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
