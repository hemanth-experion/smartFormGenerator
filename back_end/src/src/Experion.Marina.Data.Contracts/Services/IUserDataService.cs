using Experion.Marina.Data.Contracts.Entities;

namespace Experion.Marina.Data.Contracts.Services
{
    public interface IUserDataService
    {
        IUserValid IsValidEntity(IUserCredential userCredential);
    }
}
