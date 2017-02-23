using Autofac;
using Experion.Marina.Data.Contracts.Entities;
using Experion.Marina.Data.Contracts.Services;
using Experion.Marina.Data.Services.Entities;
using System.Linq;
using System;

namespace Experion.Marina.Data.Services.Services
{
    public class UserDataService : DataService<MarinaContext>, IUserDataService
    {
        public UserDataService(IComponentContext iocContext, IRepositoryContext context)
            : base(iocContext, context)
        {
        }
        private IRepository<UserCredential, long> UserRepository => DataContext.GetRepository<UserCredential, long>();


        public IUserValid IsValidEntity(IUserCredential userCredential)
        {
            var param = new Specification<UserCredential>(x => ( x.UserName == userCredential.UserName && x.Password == userCredential.Password));
            var entity = UserRepository.GetBySpecification(param).FirstOrDefault();
            if (entity == null)
            {
                return new UserValid
                {
                    Id = 0,
                    isValid = false
                };
            }
            else
                return new UserValid
                {
                   
                    Id = entity.Id,
                    isValid = true
                };
        }

    }
}
