using Core.Buisiness.Features.CQRS;

namespace Business.Features.CQRS._Generic.Secured
{
    /// <summary>
    /// Hem veri değiştirme (Command) yeteneğine sahip olan hem de güvenlik/yetkilendirme
    /// kontrolü gerektiren istekler için kullanılır.
    /// </summary>
    /// <typeparam name="TResponse">Dönecek olan cevap tipi.</typeparam>
    public interface ISecuredCommand<out TResponse> : ICommand<TResponse>, ISecureRequest
    {
    }
}
