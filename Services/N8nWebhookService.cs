using System.Text;
using System.Text.Json;

namespace VulpesFormsApp.Services;

public class N8nWebhookService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly ILogger<N8nWebhookService> _logger;

    public N8nWebhookService(HttpClient httpClient, IConfiguration config, ILogger<N8nWebhookService> logger)
    {
        _httpClient = httpClient;
        _config = config;
        _logger = logger;
    }

    public async Task<bool> SendToWebhookAsync(string webhookKey, object payload)
    {
        var url = _config[$"N8nWebhooks:{webhookKey}"];
        if (string.IsNullOrEmpty(url))
        {
            _logger.LogError("Webhook URL not found for key: {Key}", webhookKey);
            return false;
        }

        try
        {
            var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully sent to {Key}", webhookKey);
                return true;
            }

            _logger.LogWarning("Webhook {Key} returned {Status}", webhookKey, response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending to webhook {Key}", webhookKey);
            return false;
        }
    }
}