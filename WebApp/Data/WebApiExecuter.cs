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
            return await httpClient.GetFromJsonAsync<T>(relativeUrl);
        }

        public async Task<T?> InvokePost<T>(string relativeUrl, T obj)
        {
            var httpClient = HttpClientFactory.CreateClient(apiName);
            var response = await httpClient.PostAsJsonAsync(relativeUrl, obj);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}
