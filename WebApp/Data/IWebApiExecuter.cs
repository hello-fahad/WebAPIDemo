
namespace WebApp.Data
{
    public interface IWebApiExecuter
    {
        IHttpClientFactory HttpClientFactory { get; }

        Task<T?> InvokeGet<T>(string relativeUrl);
    }
}