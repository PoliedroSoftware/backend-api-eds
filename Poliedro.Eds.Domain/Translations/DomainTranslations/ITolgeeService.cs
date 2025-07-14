using System.Collections.Generic;
using System.Threading.Tasks;

namespace Poliedro.Eds.Application.Ports.Translations;

public interface ITolgeeService
{
    Task<Dictionary<string, Dictionary<string, string>>> GetAllTranslationsFromTolgee();
}
