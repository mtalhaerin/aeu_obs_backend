using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt; // Bunu mutlaka ekleyin
using System.Linq;
using System.Security.Claims;

namespace Core.Extensions.Yetki
{
    public static class ClaimExtensions
    {
        public static void AddEmail(this ICollection<Claim> claims, string email)
        {
            // "email" olarak çıkar
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, email));
        }

        public static void AddName(this ICollection<Claim> claims, string name)
        {
            // "unique_name" veya "name" olarak çıkar.
            // JwtRegisteredClaimNames.UniqueName genelde .NET'te standarttır ama
            // doğrudan "name" istiyorsan:
            claims.Add(new Claim(JwtRegisteredClaimNames.Name, name));
        }

        // 1. FIRST NAME (Given Name) -> "given_name"
        public static void AddFirstName(this ICollection<Claim> claims, string firstName)
        {
            // ClaimTypes.GivenName yerine JwtRegisteredClaimNames.GivenName kullanıyoruz
            claims.Add(new Claim(JwtRegisteredClaimNames.GivenName, firstName));
        }

        // 2. LAST NAME (Surname) -> "family_name"
        public static void AddLastName(this ICollection<Claim> claims, string lastName)
        {
            // ClaimTypes.Surname yerine JwtRegisteredClaimNames.FamilyName
            claims.Add(new Claim(JwtRegisteredClaimNames.FamilyName, lastName));
        }

        // 3. MIDDLE NAME -> "middle_name"
        public static void AddMiddleName(this ICollection<Claim> claims, string? middleName)
        {
            if (!string.IsNullOrEmpty(middleName))
            {
                // Zaten doğruydu
                claims.Add(new Claim(JwtRegisteredClaimNames.MiddleName, middleName));
            }
        }

        // 4. NAME IDENTIFIER (ID) -> "sub" (Subject)
        public static void AddNameIdentifier(this ICollection<Claim> claims, string nameIdentifier)
        {
            // JWT standardında kullanıcının ID'si "sub" (subject) olarak geçer.
            // ClaimTypes.NameIdentifier kullanırsan uzun URL gelir.
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, nameIdentifier));
        }
        public static void AddIdentityType(this ICollection<Claim> claims, string identityType)
        {
            claims.Add(new Claim("identity_type", identityType));
        }
        public static void AddIdentityNumber(this ICollection<Claim> claims, string identityNumber)
        {
            claims.Add(new Claim("identity_number", identityNumber));
        }
        // 5. ROLES -> "role"
        public static void AddRoles(this ICollection<Claim> claims, string[] roles)
        {
            // Rol için JwtRegisteredClaimNames içinde standart bir karşılık yoktur.
            // ClaimTypes.Role uzun URL verir.
            // Manuel olarak "role" string'ini kullanmak en temizidir.
            roles.ToList().ForEach(role => claims.Add(new Claim("role", role)));
        }
        public static void AddRoles(this ICollection<Claim> claims, Guid[] roles)
        {
            roles.ToList().ForEach(role => claims.Add(new Claim("role", role.ToString())));
        }
    }
}