using System.Text;

public class WhatsAppService : IWhatsAppService
{
    private readonly HttpClient _httpClient;
    private readonly string _whatsappApiUrl = "https://graph.facebook.com/v13.0/{{WhatsApp-Phone-Number-ID}}/messages";
    private readonly string _accessToken = "YOUR_ACCESS_TOKEN"; // Aquí colocas tu token de acceso de WhatsApp

    public WhatsAppService(HttpClient httpClient)
    {
        _httpClient = httpClient;
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
        var content = new StringContent(jsonPayload, Encoding.UTF8, "appsettings.json");

        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessToken}");

        var response = await _httpClient.PostAsync(_whatsappApiUrl, content);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error sending message to WhatsApp API.");
        }
    }
}
