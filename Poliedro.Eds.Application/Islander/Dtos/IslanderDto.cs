namespace Poliedro.Eds.Application.Islander.Dtos;

public class IslanderDto
{
    public int IdIslander { get; set; }
    public string Name { get; set; }
    public int IdEds { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? NameClaimToken { get; set; }
}

