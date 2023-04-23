namespace Currency_API.Tests;

public class TestHttpFactory : IHttpClientFactory
{
    public HttpClient CreateClient(string name)
    {
        return new HttpClient();
    }
}