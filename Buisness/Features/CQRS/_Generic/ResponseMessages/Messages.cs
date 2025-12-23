using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.CQRS._Generic.ResponseMessages
{
    public static class Messages
    {
        public static string Added(string entityName) => $"{entityName} başarıyla eklendi.";
        public static string Deleted(string entityName) => $"{entityName} başarıyla silindi.";
        public static string Updated(string entityName) => $"{entityName} başarıyla güncellendi.";
        public static string NotFound(string entityName) => $"{entityName} bulunamadı.";
        public static string Listed(string entityName) => $"{entityName} başarıyla listelendi.";
        public static string UserRegistered = "Kullanıcı başarıyla kaydedildi.";
        public static string UserNotFound = "Kullanıcı bulunamadı.";
        public static string PasswordError = "Parola hatası.";
        public static string SuccessfulLogin = "Başarılı giriş.";
        public static string UserAlreadyExists = "Kullanıcı zaten mevcut.";
        public static string AccessTokenCreated = "Erişim belirteci oluşturuldu.";
    }
}
