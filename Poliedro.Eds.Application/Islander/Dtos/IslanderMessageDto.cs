using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Application.Islander.Dtos;

public class IslanderMessageDto
{
    public int IdEds { get; set; }
    public string User { get; set; } = string.Empty;
    public string Email { get; set; }= string.Empty;
    public string FirstName { get; set; }=string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string NameClaimToken { get; set; }
}
