using System.Net;
using RichardSzalay.MockHttp;

namespace Currency_API.Tests;

public class MockHttpClientFactory : IHttpClientFactory
{
    private readonly Dictionary<string, RequestResponse> _responses = new Dictionary<string, RequestResponse>();

    public MockHttpClientFactory AddResponse(string url, string response, HttpStatusCode statusCode = HttpStatusCode.OK, string mediaType = "application/json")
    {
        _responses.Add(url, new RequestResponse(response, mediaType, statusCode));
        return this;
    }
    

    public HttpClient CreateClient(string name)
    {
        var mockHttp = new MockHttpMessageHandler();
        foreach (var pair in _responses)
        {
            mockHttp.When(pair.Key).Respond(pair.Value.StatusCode, pair.Value.MediaType, pair.Value.Content);
        }
        return mockHttp.ToHttpClient();
    }

    private class RequestResponse
    {
        public string Content { get; set; }
        public string MediaType { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public RequestResponse(string content, string mediaType, HttpStatusCode statusCode) =>
            (Content, MediaType, StatusCode) = (content, mediaType, statusCode);
    }
}