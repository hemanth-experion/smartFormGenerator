using Autofac;
using Experion.Marina.Business.Services.Contracts;
using Experion.Marina.Core;
using Experion.Marina.Data.Contracts.Entities;
using Experion.Marina.Data.Contracts.Services;
using Experion.Marina.Dto;

namespace Experion.Marina.Business.Services
{
    public class UserService :BusinessService, IUserService
    {
        private readonly IComponentContext _iocContext;

        public UserService(IComponentContext iocContext)
        {
            _iocContext = iocContext;
        }

        public UserValidDto IsValidUser(UserDto user)
        {
            var userDataService = _iocContext.Resolve<IUserDataService>();
            var userCredential = _iocContext.Resolve<IUserCredential>();
            userCredential.UserName = user.UserName;
            userCredential.Password = user.Password;
            var userValidity = userDataService.IsValidEntity(userCredential);

            return new UserValidDto
            {
                Id = userValidity.Id,
                IsValid = userValidity.isValid
            };
        }
    }
}
