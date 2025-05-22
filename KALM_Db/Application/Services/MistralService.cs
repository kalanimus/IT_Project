using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Services;

public class MistralService : IMistralService
{
  private readonly HttpClient _httpClient;
  private readonly string _apiKey;
  private readonly string _apiUrl;

  public MistralService(IConfiguration configuration)
  {
    _httpClient = new HttpClient();
    _apiKey = configuration["Mistral:ApiKey"];
    _apiUrl = configuration["Mistral:ApiUrl"] ?? "https://api.mistral.ai/v1/chat/completions";
  }

  public async Task<string> SendPromptAsync(string prompt)
  {
    var requestBody = new
    {
      model = "mistral-small-latest",
      messages = new[]
        {
          new { role = "user", content = prompt }
        }
    };

    var requestJson = JsonSerializer.Serialize(requestBody);
    var request = new HttpRequestMessage(HttpMethod.Post, _apiUrl);
    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
    request.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

    var response = await _httpClient.SendAsync(request);
    response.EnsureSuccessStatusCode();

    var responseJson = await response.Content.ReadAsStringAsync();
    using var doc = JsonDocument.Parse(responseJson);
    var content = doc.RootElement
        .GetProperty("choices")[0]
        .GetProperty("message")
        .GetProperty("content")
        .GetString();

    return content ?? "";
  }
}