public interface IProductCompartimentGetByCompartmentId
{
    Task<int?> GetProductIdByCompartmentIdAsync(int idCompartiment);
}
