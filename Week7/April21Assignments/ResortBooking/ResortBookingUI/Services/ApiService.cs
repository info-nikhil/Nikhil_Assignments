using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class ApiService
{
    private readonly HttpClient _client;
    private readonly IHttpContextAccessor _httpContext;

    public ApiService(HttpClient client, IHttpContextAccessor httpContext)
    {
        _client = client;
        _httpContext = httpContext;
    }

    private void AddToken()
    {
        var token = _httpContext.HttpContext.Session.GetString("JWT");
        if (!string.IsNullOrEmpty(token))
        {
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<T> GetAsync<T>(string url)
    {
        AddToken();
        var res = await _client.GetAsync(url);
        var json = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task PostAsync<T>(string url, T data)
    {
        AddToken();
        var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        await _client.PostAsync(url, content);
    }

    public async Task PutAsync<T>(string url, T data)
    {
        AddToken();
        var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        await _client.PutAsync(url, content);
    }

    public async Task DeleteAsync(string url)
    {
        AddToken();
        await _client.DeleteAsync(url);
    }

    public async Task<T> PostWithResponseAsync<T>(string url, object data)
    {
        AddToken();

        var content = new StringContent(
            System.Text.Json.JsonSerializer.Serialize(data),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _client.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
            return default;

        var json = await response.Content.ReadAsStringAsync();

        return System.Text.Json.JsonSerializer.Deserialize<T>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task PutRawAsync(string url, string rawJson)
    {
        AddToken();

        var content = new StringContent(rawJson, Encoding.UTF8, "application/json");

        await _client.PutAsync(url, content);
    }
}