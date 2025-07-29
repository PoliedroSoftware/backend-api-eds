using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Poliedro.Eds.Domain.SendMessage;

namespace Poliedor.External.WhatsApp.SendMessage;

public class WhatsAppService (HttpClient httpClient, IConfiguration config)  : ISendMessage
{
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

        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {config["WhatsApp:Token"]}");

        var response = await httpClient.PostAsync(config["WhatsApp:Url"], content);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error sending message to WhatsApp API.");
        }
    }
}
