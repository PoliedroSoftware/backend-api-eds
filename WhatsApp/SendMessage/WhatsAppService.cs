using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Poliedro.Eds.Domain.SendMessage;

namespace Poliedro.External.WhatsApp.SendMessage;

public class WhatsAppService : ISendMessage
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public WhatsAppService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;

        // Configura el encabezado Authorization solo una vez al crear el servicio
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_config["WhatsApp:Token"]}");
    }

    public async Task SendMessageAsync(string phoneNumber, string message)
    {
        var requestPayload = new
        {
            messaging_product = "whatsapp",
            recipient_type = "individual",
            to = phoneNumber,
            type = "text",
            text = new
            {
                preview_url = false,
                body = message
            }
        };

        var jsonPayload = JsonConvert.SerializeObject(requestPayload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(_config["WhatsApp:Url"], content);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error sending message to WhatsApp API.");
        }
    }
}

