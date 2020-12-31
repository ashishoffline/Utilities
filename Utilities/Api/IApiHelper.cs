using System.Net.Http;
using System.Threading.Tasks;

namespace Utilities.Api
{
    public interface IApiHelper
    {
        HttpResponseMessage Delete(string requestUri, string name = null);
        Task<HttpResponseMessage> DeleteAsync(string requestUri, string name = null);
        HttpResponseMessage Get(string requestUri, string name = null);
        Task<HttpResponseMessage> GetAsync(string requestUri, string name = null);
        HttpResponseMessage Post(string requestUri, object content, string name = null);
        Task<HttpResponseMessage> PostAsync(string requestUri, object content, string name = null);
        HttpResponseMessage Put(string requestUri, object content, string name = null);
        Task<HttpResponseMessage> PutAsync(string requestUri, object content, string name = null);
    }
}