using Core.Entities.Concrete.OzlukEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Core.Utilities.Security.Encyption;
using System.IdentityModel.Tokens.Jwt;
using Core.Entities.Concrete.YetkiEntities;
using Core.Extensions.Yetki;

namespace Core.Utilities.Security.JWT
{
    public class JwtHelper : ITokenHelper
    {
        public IConfiguration Configuration { get; }
        private TokenOptions _tokenOptions;
        private DateTime _accessTokenExpiration;
        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();

        }
        public AccessToken CreateToken(Kullanici kullanici, List<IslemYetkisi> islemYetkileri)
        {
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            var jwt = CreateJwtSecurityToken(_tokenOptions, kullanici, signingCredentials, islemYetkileri);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };

        }

        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, Kullanici kullanici,
            SigningCredentials signingCredentials, List<IslemYetkisi> islemYetkileri)
        {
            var jwt = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: SetClaims(kullanici, islemYetkileri),
                signingCredentials: signingCredentials
            );
            return jwt;
        }

        private IEnumerable<Claim> SetClaims(Kullanici kullanici, List<IslemYetkisi> islemYetkileri)
        {
            var claims = new List<Claim>();
            claims.AddNameIdentifier(kullanici.KullaniciUuid.ToString());
            claims.AddEmail(kullanici.KurumEposta);
            claims.AddName($"{kullanici.Ad} {kullanici.Soyad}");
            claims.AddFirstName(kullanici.Ad);
            claims.AddLastName(kullanici.Soyad);
            claims.AddMiddleName(kullanici.OrtaAd);
            claims.AddRoles(islemYetkileri.Select(c => c.YetkiAdi).ToArray());

            return claims;
        }
    }
}
