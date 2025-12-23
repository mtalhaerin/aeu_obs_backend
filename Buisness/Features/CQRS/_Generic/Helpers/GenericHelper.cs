using Microsoft.AspNetCore.Http;

namespace Business.Features.CQRS._Generic.Helpers
{
    public class GenericHelper : IGenericHelper
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        public GenericHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string? GetAccessTokenFromHeader()
        {
            var authorization = _httpContextAccessor.HttpContext!.Request.Headers["Authorization"].ToString();
            var token = authorization.StartsWith("Bearer ") ? authorization.Substring("Bearer ".Length).Trim() : authorization;
            return string.IsNullOrEmpty(token) ? null : token;
        }
    }
}
