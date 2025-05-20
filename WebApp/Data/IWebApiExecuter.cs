
namespace WebApp.Data
{
    public interface IWebApiExecuter
    {
        IHttpClientFactory HttpClientFactory { get; }

        Task<T?> InvokeGet<T>(string relativeUrl);
        Task<T?> InvokePost<T>(string relativeUrl, T obj);
    }
}