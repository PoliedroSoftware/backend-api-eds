namespace Poliedro.Eds.Application.Tank.Commands.CreateTank;

public record CreateTankRequestDto(
    string Number,
    int Compartment,
    double Ability,
    double Stock);


