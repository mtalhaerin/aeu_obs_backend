using Autofac;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework.OzlukDals;
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
        
            builder.RegisterType<KullaniciManager>().As<IKullaniciService>().SingleInstance();
            builder.RegisterType<EFKullaniciDal>().As<IKullaniciDal>().SingleInstance();
           


        }
    }
}
