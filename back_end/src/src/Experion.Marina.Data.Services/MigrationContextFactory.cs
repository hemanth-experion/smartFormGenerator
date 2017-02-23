using System.Data.Entity.Infrastructure;

namespace Experion.Marina.Data.Services
{
    public class MigrationContextFactory : IDbContextFactory<MarinaContext>
    {
        public MarinaContext Create()
        {
            return new MarinaContext(@"Data Source=DESKTOP-2BRPU4D\SQLEXPRESS;Initial Catalog=SmartFormDb;User Id=sa;Password=exp@123;MultipleActiveResultSets=true");
        }
    }
}