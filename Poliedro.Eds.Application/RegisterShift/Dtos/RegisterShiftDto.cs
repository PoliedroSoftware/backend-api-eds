namespace Poliedro.Eds.Application.RegisterShift.Dtos;

public class RegisterShiftDto
{
    public int IdEds { get; set; }
    public int IdBusiness { get; set; }
    public int IdIslander { get; set; }
    public DateOnly DateStartTime { get; set; }
    public TimeOnly StartTime { get; set; }
    public DateOnly DateEndTime { get; set; }
    public TimeOnly EndTime { get; set; }
}
