namespace Poliedro.Eds.Application.Ports.Translations;

public class TransalationsAvailable
{
    public Dictionary<string, Dictionary<string, string>> translations { get; set; }
    public List<Language> Languages { get; set; }
}
