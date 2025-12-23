using System;
using System.Collections.Generic;

namespace Core.CrossCuttingConcerns.Caching
{
    // Token yönetimi için özelleşmiş cache arayüzü
    public interface ITokenCacheManager
    {
        // Token'ı kaydeder (Callback ve Listeleme mekanizmasıyla)
        // duration: Dakika cinsinden ömür
        void RegisterToken(string token, Guid userUuid, int duration);

        // Belirli bir token'ı manuel siler (Blacklist)
        void RemoveToken(string token);

        // Kullanıcının aktif tokenlarını listeler
        List<string> GetActiveTokens(Guid userUuid);

        // Token geçerli mi diye bakar, geçerliyse UserId döner
        Guid? ValidateToken(string token);

        bool IsTokenActive(string token);
        bool IsTokenInactive(string token);

        void InvalidateToken(string token);
        void BlacklistToken(string token);

        void BlacklistAllTokensForUser(Guid userUuid);
        //void RemoveBlacklistToken(string token);

    }
}