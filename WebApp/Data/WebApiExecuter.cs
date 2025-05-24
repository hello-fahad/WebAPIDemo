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

        public WebApiExecuter(IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            HttpClientFactory = httpClientFactory;
            Configuration = configuration;
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
            // await AddJwtToHeader(httpClient);
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

            var clientId = Configuration.GetValue<string>("ClientId");
            var secret = Configuration.GetValue<string>("Secret");

            // Authenticate 
            var authoClient = HttpClientFactory.CreateClient(authApiName);




            try
            {
                var response = await authoClient.PostAsJsonAsync("auth", new AppCredential
                {
                    ClientId = clientId,
                    Secret = secret
                });

                var content = await response.Content.ReadAsStringAsync();

                response.EnsureSuccessStatusCode(); // This throws if not 2xx

                var token = JsonConvert.DeserializeObject<JwtToken>(content);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken);
            }
            catch (HttpRequestException ex)
            {
                // Log full error response for debugging
                throw new Exception("Authentication failed: " + ex.Message);
            }








            //var response = await authoClient.PostAsJsonAsync("auth", new AppCredential
            //{
            //    ClientId = clientId,
            //    Secret = secret
            //});

            //response.EnsureSuccessStatusCode();

            //// Get the JWT
            //string strToken = await response.Content.ReadAsStringAsync();
            //var token = JsonConvert.DeserializeObject<JwtToken>(strToken);


            //// Pass the JWT to the endpoints through the http headers
            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken);
        }
    }
}
