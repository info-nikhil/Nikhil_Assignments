using Microsoft.EntityFrameworkCore;
using ResortBookingUI.MVC.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ResortBookingUI.MVC.Services
{
    public class ApiService
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContext;

        public ApiService(HttpClient client, IHttpContextAccessor httpContext)
        {
            _client = client;
            _httpContext = httpContext;
        }

        // ================= ADD TOKEN =================
        private void AddToken()
        {
            var token = _httpContext.HttpContext?.Session.GetString("JWT");

            // Clear previous header to avoid duplicates
            _client.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // ================= GET =================
        public async Task<T> GetAsync<T>(string url)
        {
            AddToken();

            var response = await _client.GetAsync(url);

            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"GET Error: {json}");

            return JsonSerializer.Deserialize<T>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        // ================= POST =================
        public async Task<HttpResponseMessage> PostAsync<T>(string url, T data)
        {
            AddToken();

            var content = new StringContent(
                JsonSerializer.Serialize(data),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"POST Error: {error}");
            }

            return response;
        }

        // ================= POST WITH RESPONSE =================
        public async Task<T> PostWithResponseAsync<T>(string url, object data)
        {
            AddToken();

            var content = new StringContent(
                JsonSerializer.Serialize(data),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PostAsync(url, content);

            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"POST Error: {json}");

            return JsonSerializer.Deserialize<T>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        // ================= PUT =================
        public async Task PutAsync<T>(string url, T data)
        {
            AddToken();

            var content = new StringContent(
                JsonSerializer.Serialize(data),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PutAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"PUT Error: {error}");
            }
        }

        // ================= PUT RAW (for string body like "Accepted") =================
        public async Task PutRawAsync(string url, string rawJson)
        {
            AddToken();

            var content = new StringContent(rawJson, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"PUT Error: {error}");
            }
        }

        // ================= DELETE =================
        public async Task DeleteAsync(string url)
        {
            AddToken();

            var response = await _client.DeleteAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"DELETE Error: {error}");
            }
        }

        
    }
}