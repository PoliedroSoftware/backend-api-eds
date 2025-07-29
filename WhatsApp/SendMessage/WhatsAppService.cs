using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Poliedro.Eds.Domain.SendMessage;

namespace Poliedor.External.WhatsApp.SendMessage
{
    public class WhatsAppService (HttpClient httpClient, IConfiguration config)  : ISendMessage
    {
        private readonly string _whatsappApiUrl = config ["WhatsApp:Url"];
        private readonly string _accessToken = "YOUR_ACCESS_TOKEN"; // Aquí colocas tu token de acceso de WhatsApp


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
            var content = new StringContent(jsonPayload, Encoding.UTF8, "appsettings.json");

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessToken}");

            var response = await httpClient.PostAsync(_whatsappApiUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error sending message to WhatsApp API.");
            }
        }
    }

}
