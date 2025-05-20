using System.Text.Json;

namespace WebApp.Data
{
    public class WebApiExecuter : IWebApiExecuter
    {
        private const string apiName = "ShirtsApi";
        public IHttpClientFactory HttpClientFactory { get; }

        public WebApiExecuter(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
        }

        public async Task<T?> InvokeGet<T>(string relativeUrl)
        {
            var httpClient = HttpClientFactory.CreateClient(apiName);
            var request = new HttpRequestMessage(HttpMethod.Get, relativeUrl);
            var response = await httpClient.SendAsync(request);
            await HandlePotentailError(response);

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T?> InvokePost<T>(string relativeUrl, T obj)
        {
            var httpClient = HttpClientFactory.CreateClient(apiName);
            var response = await httpClient.PostAsJsonAsync(relativeUrl, obj);

            await HandlePotentailError(response);

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task InvokePut<T>(string relativeUrl, T obj)
        {
            var httpClient = HttpClientFactory.CreateClient(apiName);
            var response = await httpClient.PutAsJsonAsync(relativeUrl, obj);
            await HandlePotentailError(response);
        }

        public async Task InvokeDelete(string relativeUrl)
        {
            var httpClient = HttpClientFactory.CreateClient(apiName);
            var response = await httpClient.DeleteAsync(relativeUrl);
            await HandlePotentailError(response);
        }

        private async Task HandlePotentailError(HttpResponseMessage httpResponse)
        {
            if (!httpResponse.IsSuccessStatusCode)
            {
                var errorJson = await httpResponse.Content.ReadAsStringAsync();
                throw new WebApiException(errorJson);
            }
        }
    }
}
