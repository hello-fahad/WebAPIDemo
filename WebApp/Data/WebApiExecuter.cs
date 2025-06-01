using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WebApp.Data
{
    public class WebApiExecuter : IWebApiExecuter
    {
        private const string apiName = "ShirtsApi";
        private const string authApiName = "AuthorityApi";
        public IHttpClientFactory HttpClientFactory { get; }
        public IConfiguration Configuration { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }

        public WebApiExecuter(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            HttpClientFactory = httpClientFactory;
            Configuration = configuration;
            HttpContextAccessor = httpContextAccessor;
        }

        public async Task<T?> InvokeGet<T>(string relativeUrl)
        {
            var httpClient = HttpClientFactory.CreateClient(apiName);
            await AddJwtToHeader(httpClient);
            var request = new HttpRequestMessage(HttpMethod.Get, relativeUrl);
            var response = await httpClient.SendAsync(request);
            await HandlePotentailError(response);

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T?> InvokePost<T>(string relativeUrl, T obj)
        {
            var httpClient = HttpClientFactory.CreateClient(apiName);
            await AddJwtToHeader(httpClient);
            var response = await httpClient.PostAsJsonAsync(relativeUrl, obj);

            await HandlePotentailError(response);

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task InvokePut<T>(string relativeUrl, T obj)
        {
            var httpClient = HttpClientFactory.CreateClient(apiName);
            await AddJwtToHeader(httpClient);
            var response = await httpClient.PutAsJsonAsync(relativeUrl, obj);
            await HandlePotentailError(response);
        }

        public async Task InvokeDelete(string relativeUrl)
        {
            var httpClient = HttpClientFactory.CreateClient(apiName);
            await AddJwtToHeader(httpClient);
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

        private async Task AddJwtToHeader(HttpClient httpClient)
        {
            JwtToken? token = null;
            string? strToken = HttpContextAccessor.HttpContext?.Session.GetString("access_token");
            if(!string.IsNullOrWhiteSpace(strToken))
            {
                token = JsonConvert.DeserializeObject<JwtToken>(strToken);
            }

            if(token == null || token.ExpiresAt <= DateTime.UtcNow)
            {
                var clientId = Configuration.GetValue<string>("ClientId");
                var secret = Configuration.GetValue<string>("Secret");

                // Authenticate 
                var authoClient = HttpClientFactory.CreateClient(authApiName);


                var response = await authoClient.PostAsJsonAsync("auth", new AppCredential
                {
                    ClientId = clientId,
                    Secret = secret
                });

                response.EnsureSuccessStatusCode(); // This throws if not 2xx
                strToken = await response.Content.ReadAsStringAsync();

                HttpContextAccessor.HttpContext?.Session.SetString("access_token", strToken);
               
            }

            token = JsonConvert.DeserializeObject<JwtToken>(strToken);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken);
        }
    }
}
