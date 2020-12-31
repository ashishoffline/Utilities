using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Utilities.Api
{
    public class ApiHelper : IApiHelper
    {
        private const HttpCompletionOption defaultCompletionOption = HttpCompletionOption.ResponseContentRead;
        private readonly IHttpClientFactory _httpClientFactory;
        public ApiHelper(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        #region GET
        public async Task<HttpResponseMessage> GetAsync(string requestUri, string name = null)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUri, UriKind.RelativeOrAbsolute)
            };

            HttpClient httpClient = string.IsNullOrWhiteSpace(name) ? _httpClientFactory.CreateClient() : _httpClientFactory.CreateClient(name);

            HttpResponseMessage httpResponse = await httpClient.SendAsync(request, defaultCompletionOption, CancellationToken.None);

            return httpResponse;
        }

        public HttpResponseMessage Get(string requestUri, string name = null)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUri, UriKind.RelativeOrAbsolute)
            };

            HttpClient httpClient = string.IsNullOrWhiteSpace(name) ? _httpClientFactory.CreateClient() : _httpClientFactory.CreateClient(name);

            HttpResponseMessage httpResponse = httpClient.Send(request, defaultCompletionOption);

            return httpResponse;
        }
        #endregion

        #region POST
        public async Task<HttpResponseMessage> PostAsync<T>(string requestUri, T content, string name = null) where T : class
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(requestUri, UriKind.RelativeOrAbsolute),
                Content = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json")
            };

            HttpClient httpClient = string.IsNullOrWhiteSpace(name) ? _httpClientFactory.CreateClient() : _httpClientFactory.CreateClient(name);
            HttpResponseMessage httpResponse = await httpClient.SendAsync(request, defaultCompletionOption, CancellationToken.None);

            return httpResponse;
        }
        public HttpResponseMessage Post<T>(string requestUri, T content, string name = null) where T : class
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(requestUri, UriKind.RelativeOrAbsolute),
                Content = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json")
            };

            HttpClient httpClient = string.IsNullOrWhiteSpace(name) ? _httpClientFactory.CreateClient() : _httpClientFactory.CreateClient(name);
            HttpResponseMessage httpResponse = httpClient.Send(request, defaultCompletionOption);

            return httpResponse;
        }
        #endregion

        #region PUT
        public async Task<HttpResponseMessage> PutAsync<T>(string requestUri, T content, string name = null) where T : class
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(requestUri, UriKind.RelativeOrAbsolute),
                Content = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json")
            };

            HttpClient httpClient = string.IsNullOrWhiteSpace(name) ? _httpClientFactory.CreateClient() : _httpClientFactory.CreateClient(name);
            HttpResponseMessage httpResponse = await httpClient.SendAsync(request, defaultCompletionOption, CancellationToken.None);

            return httpResponse;
        }
        public HttpResponseMessage Put<T>(string requestUri, T content, string name = null) where T : class
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(requestUri, UriKind.RelativeOrAbsolute),
                Content = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json")
            };

            HttpClient httpClient = string.IsNullOrWhiteSpace(name) ? _httpClientFactory.CreateClient() : _httpClientFactory.CreateClient(name);
            HttpResponseMessage httpResponse = httpClient.Send(request, defaultCompletionOption);

            return httpResponse;
        }
        #endregion

        #region DELETE
        public async Task<HttpResponseMessage> DeleteAsync(string requestUri, string name = null)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(requestUri, UriKind.RelativeOrAbsolute)
            };

            HttpClient httpClient = string.IsNullOrWhiteSpace(name) ? _httpClientFactory.CreateClient() : _httpClientFactory.CreateClient(name);
            HttpResponseMessage httpResponse = await httpClient.SendAsync(request, defaultCompletionOption, CancellationToken.None);

            return httpResponse;
        }

        public HttpResponseMessage Delete(string requestUri, string name = null)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(requestUri, UriKind.RelativeOrAbsolute)
            };

            HttpClient httpClient = string.IsNullOrWhiteSpace(name) ? _httpClientFactory.CreateClient() : _httpClientFactory.CreateClient(name);
            HttpResponseMessage httpResponse = httpClient.Send(request, defaultCompletionOption);

            return httpResponse;
        }
        #endregion
    }
}
