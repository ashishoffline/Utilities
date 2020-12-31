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
        HttpResponseMessage Post<T>(string requestUri, T content, string name = null) where T : class;
        Task<HttpResponseMessage> PostAsync<T>(string requestUri, T content, string name = null) where T : class;
        HttpResponseMessage Put<T>(string requestUri, T content, string name = null) where T : class;
        Task<HttpResponseMessage> PutAsync<T>(string requestUri, T content, string name = null) where T : class;
    }
}