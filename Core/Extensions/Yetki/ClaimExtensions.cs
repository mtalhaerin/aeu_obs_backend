using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions.Yetki
{
    public static class ClaimExtensions
    {
        public static void AddEmail(this ICollection<Claim> claims, string email)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, email));
        }

        // Genel tam ad (Display Name gibi) için tutabilirsin
        public static void AddName(this ICollection<Claim> claims, string name)
        {
            claims.Add(new Claim(ClaimTypes.Name, name));
        }

        // 1. FIRST NAME (Given Name)
        public static void AddFirstName(this ICollection<Claim> claims, string firstName)
        {
            // ClaimTypes.GivenName standart olarak "First Name" karşılığıdır.
            claims.Add(new Claim(ClaimTypes.GivenName, firstName));
        }

        // 2. LAST NAME (Surname)
        public static void AddLastName(this ICollection<Claim> claims, string lastName)
        {
            // ClaimTypes.Surname standart olarak "Last Name" karşılığıdır.
            claims.Add(new Claim(ClaimTypes.Surname, lastName));
        }

        // 3. MIDDLE NAME
        public static void AddMiddleName(this ICollection<Claim> claims, string? middleName)
        {
            // ClaimTypes sınıfında doğrudan "MiddleName" yoktur.
            // Bu yüzden JWT standartlarındaki "middle_name"i veya JwtRegisteredClaimNames'i kullanırız.
            if (!string.IsNullOrEmpty(middleName))
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.MiddleName, middleName));
            }
        }

        public static void AddNameIdentifier(this ICollection<Claim> claims, string nameIdentifier)
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, nameIdentifier));
        }

        public static void AddRoles(this ICollection<Claim> claims, string[] roles)
        {
            roles.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
        }
    }
}
