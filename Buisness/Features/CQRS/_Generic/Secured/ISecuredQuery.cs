using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.CQRS._Generic.Secured
{
    /// <summary>
    /// Hem veri sorgulama (Query) yeteneğine sahip olan hem de güvenlik/yetkilendirme 
    /// kontrolü gerektiren istekler için kullanılır.
    /// </summary>
    /// <typeparam name="TResponse">Dönecek olan cevap tipi.</typeparam>
    public interface ISecuredQuery<out TResponse> : IQuery<TResponse>, ISecureRequest
    {
    }
}
