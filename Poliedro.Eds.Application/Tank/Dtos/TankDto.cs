namespace Poliedro.Eds.Application.Tank.Dtos;

public record TankDto(
   int IdTank,
   string Number,
   int Compartment,
   double Ability,
   double Stock);