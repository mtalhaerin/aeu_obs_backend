using Business.Concrete;
using Business.ContextCarrier;
using Business.Features.CQRS._Generic;
using Business.Features.CQRS._Generic.Helpers;
using Core.CrossCuttingConcerns.Caching;
using MediatR;

namespace Business.Behaviors;

public class AuthBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ISecureRequest // Sadece ISecuredRequest olanlar
    where TResponse : class
{
    private readonly IGenericHelper _genericHelper;
    private readonly ITokenCacheManager _tokenCacheManager;
    private readonly IKullaniciService _kullaniciService;
    private readonly IUserContext _userContext;

    public AuthBehavior(IGenericHelper genericHelper, ITokenCacheManager tokenCacheManager, IKullaniciService kullaniciService, IUserContext userContext)
    {
        _genericHelper = genericHelper;
        _tokenCacheManager = tokenCacheManager;
        _kullaniciService = kullaniciService;
        _userContext = userContext;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var token = _genericHelper.GetAccessTokenFromHeader();
        if (string.IsNullOrEmpty(token))
            return CreateFailureResponse("Token bulunamadı", 400);

        Guid userUuid = _tokenCacheManager.ValidateToken(token);
        if (userUuid == Guid.Empty)
            return CreateFailureResponse("Token geçersiz veya süresi dolmuş", 401);

        var kullanici = await _kullaniciService.GetByUuidAsync(userUuid);
        if (!kullanici.Success)
            return CreateFailureResponse("Kullanıcı bulunamadı", 404);

        _userContext.CurrentUser = kullanici.Data;
        _userContext.UserUuid = userUuid;
        _userContext.Token = token;

        // Doğrulama başarılı, bir sonraki aşamaya (Handler'a) geç
        return await next();
    }

    private TResponse CreateFailureResponse(string message, int statusCode)
    {
        var type = typeof(TResponse);
        var method = type.GetMethod("Failure", new[] { typeof(string), typeof(string), typeof(int) });
        return (TResponse)method?.Invoke(null, new object[] { message, null, statusCode })!;
    }
}