using Autofac;
using Business.Concrete;
using Business.Concrete.OzlukManagers;
using Business.Features.CQRS._Generic.Helpers;
using Core.Utilities.Security.JWT;
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

            builder.RegisterType<AdresManager>().As<IAdresService>().SingleInstance();
            builder.RegisterType<EFAdresDal>().As<IAdresDal>().SingleInstance();

            builder.RegisterType<TelefonManager>().As<ITelefonService>().SingleInstance();
            builder.RegisterType<EFTelefonDal>().As<ITelefonDal>().SingleInstance();


            builder.RegisterType<EpostaManager>().As<IEpostaService>().SingleInstance();
            builder.RegisterType<EFEpostaDal>().As<IEpostaDal>().SingleInstance();
        }
    }
}
