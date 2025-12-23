using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Core.CrossCuttingConcerns.Caching.Microsoft
{
    public class MemoryTokenCacheManager : ITokenCacheManager
    {
        // Standart .NET MemoryCache'i kullanıyoruz
        private readonly IMemoryCache _memoryCache;

        // Kullanıcı -> Token Listesi Haritası (Ram'de tutulan yan liste)
        // Static yaptık ki tüm instance'lar aynı listeyi görsün (Singleton da olsa garanti olsun)
        private static readonly ConcurrentDictionary<Guid, HashSet<string>> _userTokens = new();

        public MemoryTokenCacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void RegisterToken(string token, Guid userUuid, int duration)
        {
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(duration),
                Priority = CacheItemPriority.High
            };

            // --- CALLBACK MEKANİZMASI ---
            // Token süresi dolup silindiğinde bu metot çalışır
            cacheOptions.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                // Value olarak userUuid tutuyorduk, onu geri alıp listeyi temizliyoruz
                if (value is Guid uid)
                {
                    RemoveFromList(uid, key.ToString());
                }
            });

            // 1. Cache'e ekle (Token -> userUuid)
            _memoryCache.Set(token, userUuid, cacheOptions);

            // 2. Yan Listeye ekle (userUuid -> Token Listesi)
            AddToList(userUuid, token);
        }

        public void RemoveToken(string token)
        {
            // Cache'ten silince otomatik olarak Callback tetiklenir ve listeden de siler via 'RegisterPostEvictionCallback'
            _memoryCache.Remove(token);
        }

        public List<string> GetActiveTokens(Guid userUuid)
        {
            if (_userTokens.TryGetValue(userUuid, out var list))
            {
                lock (list) // Thread-safe okuma
                {
                    return list.ToList();
                }
            }
            return new List<string>();
        }

        public Guid? ValidateToken(string token)
        {
            if (_memoryCache.TryGetValue(token, out Guid userUuid))
            {
                return userUuid;
            }
            return null; // Token yok veya süresi dolmuş
        }

        // --- Private Yardımcı Metotlar (Listeyi Yönetmek İçin) ---

        private void AddToList(Guid userUuid, string token)
        {
            _userTokens.AddOrUpdate(userUuid,
                // İlk kez ekleniyorsa yeni liste oluştur
                new HashSet<string> { token },
                // Zaten varsa mevcut listeye ekle
                (key, existingList) =>
                {
                    lock (existingList)
                    {
                        existingList.Add(token);
                    }
                    return existingList;
                });
        }

        private void RemoveFromList(Guid userUuid, string token)
        {
            if (_userTokens.TryGetValue(userUuid, out var list))
            {
                lock (list)
                {
                    list.Remove(token);
                    // Kullanıcının hiç tokenı kalmadıysa anahtarı sözlükten tamamen silebiliriz
                    if (list.Count == 0)
                    {
                        _userTokens.TryRemove(userUuid, out _);
                    }
                }
            }
        }

        public bool IsTokenActive(string token)
        {
            if (_memoryCache.TryGetValue(token, out _))
            {
                return true; // Token aktif
            }
            return false; // Token pasif
        }

        public bool IsTokenInactive(string token)
        {
            if (!_memoryCache.TryGetValue(token, out _))
            {
                return true; // Token pasif
            }
            return false; // Token aktif
        }

        public void InvalidateToken(string token)
        {
            if (_memoryCache.TryGetValue(token, out Guid userUuid))
            {
                // Token'ı cache'ten sil
                _memoryCache.Remove(token);
                // Yan listeden de sil
                RemoveFromList(userUuid, token);
            }
        }

        public void BlacklistToken(string token)
        {
            if (_memoryCache.TryGetValue(token, out Guid userUuid))
            {
                // Token'ı cache'ten sil
                _memoryCache.Remove(token);
                // Yan listeden de sil
                RemoveFromList(userUuid, token);
            }
        }

        public void BlacklistAllTokensForUser(Guid userUuid)
        {
            if (_userTokens.TryGetValue(userUuid, out var list))
            {
                lock (list)
                {
                    foreach (var token in list.ToList())
                    {
                        // Her token'ı cache'ten sil
                        _memoryCache.Remove(token);
                    }
                    // Kullanıcının token listesini temizle
                    _userTokens.TryRemove(userUuid, out _);
                }
            }
        }
    }
}