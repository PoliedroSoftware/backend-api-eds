namespace Poliedro.Eds.Application.Court.Commands.CreateCourt
{
    public record CourtTypeOfCollectionCommand(
        int IdTypeOfCollection,
        double Amount,
        string Description,
        string TypeOfCollectionName
        );
}