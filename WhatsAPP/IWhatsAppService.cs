public interface IWhatsAppService
{
    Task SendMessageAsync(string phoneNumber, string message);
}