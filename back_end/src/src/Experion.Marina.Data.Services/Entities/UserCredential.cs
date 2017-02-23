using Experion.Marina.Data.Contracts.Entities;
using System.ComponentModel.DataAnnotations;

namespace Experion.Marina.Data.Services.Entities
{
    public class UserCredential:IUserCredential, IEntity<long>
    {
        //class used for creating the login table 
        public long Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
