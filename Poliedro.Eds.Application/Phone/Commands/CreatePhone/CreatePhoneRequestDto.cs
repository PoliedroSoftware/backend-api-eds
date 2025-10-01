namespace Poliedro.Eds.Application.Phone.Commands.CreatePhone;

public record CreatePhoneRequestDto(List<PhoneCreateDto> Phones);

public record PhoneCreateDto(string Number, string Name);
