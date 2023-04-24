using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Currency_API.Tests.IntegrationTests;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    private MockHttpClientFactory _mockHttpClientFactory = new MockHttpClientFactory();
    
    public void SetMockHttpClientFactory(MockHttpClientFactory mockHttpClientFactory)
    {
        _mockHttpClientFactory = mockHttpClientFactory;
    }
    
    public CustomWebApplicationFactory<TProgram> AddResponse(string url, string content, 
        HttpStatusCode statusCode = HttpStatusCode.OK, string contentType = "application/json")
    {
        _mockHttpClientFactory.AddResponse(url, content, statusCode, contentType);
        return this;
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // remove default HttpClientFactory
            var httpClientFactory = services.SingleOrDefault(
                d => d.GetType() == typeof(IHttpClientFactory));
            if (httpClientFactory != null) services.Remove(httpClientFactory);
            
            services.RemoveAll(typeof(IHttpClientFactory));
            
            // replace with mock
            services.AddSingleton<IHttpClientFactory>(_mockHttpClientFactory);
        });
    }
}