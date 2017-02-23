namespace Experion.Marina.Data.Contracts.Entities
{
    public interface IUserValid
    {
        long Id { get; set; }
        bool isValid { get; set; }
    }
}
