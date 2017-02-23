using Experion.Marina.Dto;

namespace Experion.Marina.Business.Services.Contracts
{
    public interface IUserService
    {
       UserValidDto IsValidUser(UserDto user);
    }
}
