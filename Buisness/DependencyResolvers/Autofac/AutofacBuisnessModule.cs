using Autofac;
using Business.Concrete;
using Business.Concrete.DersManagers;
using Business.Concrete.FakulteManagers;
using Business.Concrete.OzlukManagers;
using Business.Features.CQRS._Generic.Helpers;
using Core.Utilities.Security.JWT;
using DataAccess.Concrete.EntityFramework.DersDals;
using DataAccess.Concrete.EntityFramework.FakulteDals;
using DataAccess.Concrete.EntityFramework.OzlukDals;
using DataAccess.Concrete.EntityFramework.YetkiDals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Here you can register your business services with Autofac
            // Example:
            // builder.RegisterType<YourService>().As<IYourService>().SingleInstance();
            builder.RegisterType<GenericHelper>().As<IGenericHelper>().SingleInstance();
            builder.RegisterType<JWTHelper>().As<ITokenHelper>().SingleInstance();
        
            builder.RegisterType<KullaniciManager>().As<IKullaniciService>().SingleInstance();
            builder.RegisterType<EFKullaniciDal>().As<IKullaniciDal>().SingleInstance();
           
            builder.RegisterType<YetkiManager>().As<IYetkiService>().SingleInstance();
            builder.RegisterType<EFIslemYetkisiDal>().As<IIslemYetkisiDal>().SingleInstance();
            builder.RegisterType<EFKullaniciIslemYetkisiDal>().As<IKullaniciIslemYetkisiDal>().SingleInstance();

            #region Ozluk
            builder.RegisterType<AdresManager>().As<IAdresService>().SingleInstance();
            builder.RegisterType<EFAdresDal>().As<IAdresDal>().SingleInstance();

            builder.RegisterType<TelefonManager>().As<ITelefonService>().SingleInstance();
            builder.RegisterType<EFTelefonDal>().As<ITelefonDal>().SingleInstance();

            builder.RegisterType<EpostaManager>().As<IEpostaService>().SingleInstance();
            builder.RegisterType<EFEpostaDal>().As<IEpostaDal>().SingleInstance();
            #endregion

            #region Ders
            builder.RegisterType<DersManager>().As<IDersService>().SingleInstance();
            builder.RegisterType<EFDersDal>().As<IDersDal>().SingleInstance();

            builder.RegisterType<AkademisyenDersAtamaManager>().As<IAkademisyenDersAtamaService>().SingleInstance();
            builder.RegisterType<EFAkademisyenDersAtamaDal>().As<IAkademisyenDersAtamaDal>().SingleInstance();

            builder.RegisterType<NotManager>().As<INotService>().SingleInstance();
            builder.RegisterType<EFNotDal>().As<INotDal>().SingleInstance();
            #endregion

            #region Fakulte
            builder.RegisterType<FakulteManager>().As<IFakulteService>().SingleInstance();
            builder.RegisterType<EFFakulteDal>().As<IFakulteDal>().SingleInstance();

            builder.RegisterType<AnaDalManager>().As<IAnaDalService>().SingleInstance();
            builder.RegisterType<EFAnaDalDal>().As<IAnaDalDal>().SingleInstance();

            builder.RegisterType<BolumManager>().As<IBolumService>().SingleInstance();
            builder.RegisterType<EFBolumDal>().As<IBolumDal>().SingleInstance();

            builder.RegisterType<AkademisyenBolumAtamaManager>().As<IAkademisyenBolumAtamaService>().SingleInstance();
            builder.RegisterType<EFAkademisyenBolumAtamaDal>().As<IAkademisyenBolumAtamaDal>().SingleInstance();

            builder.RegisterType<OgrenciBolumKayitManager>().As<IOgrenciBolumKayitService>().SingleInstance();
            builder.RegisterType<EFOgrenciBolumKayitDal>().As<IOgrenciBolumKayitDal>().SingleInstance();
            #endregion
        }
    }
}
