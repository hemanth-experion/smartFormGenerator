using Experion.Marina.Data.Services.Entities;
using System.Data.Entity;
using Z.EntityFramework.Plus;

namespace Experion.Marina.Data.Services
{
    public class MarinaContext : DbContext
    {
        public MarinaContext(string connectionString) 
            : base(connectionString)
        {

        }

        #region DbSet Definitions
        public DbSet<AuditEntry> AuditEntries { get; set; }
        public DbSet<AuditEntryProperty> AuditEntryProperties { get; set; }
        public DbSet<UserCredential> UserCredentials { get; set; }
        public DbSet<TemplateDetails> TemplateDetail { get; set; }
        public DbSet<Template> Template { get; set; }
        

        #endregion

        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        /// before the model has been locked down and used to initialize the context.  The default
        /// implementation of this method does nothing, but it can be overridden in a derived class
        /// such that the model can be further configured before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        /// <remarks>
        /// Typically, this method is called only once when the first instance of a derived context
        /// is created.  The model for that context is then cached and is for all further instances of
        /// the context in the app domain.  This caching can be disabled by setting the ModelCaching
        /// property on the given ModelBuidler, but note that this can seriously degrade performance.
        /// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        /// classes directly.
        /// </remarks>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            ConfigureAuditSettings();
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Configure the audit settings here.
        /// You can exempt audit entities or non-auditable fields
        /// </summary>
        private void ConfigureAuditSettings()
        {
            AuditManager.DefaultConfiguration.Exclude(x => true); // Exclude ALL
            AuditManager.DefaultConfiguration.Include<IAuditable>(); // Include Only Entities that extends IAuditable
            AuditManager.DefaultConfiguration.AutoSavePreAction = (context, audit) =>
                (context as MarinaContext).AuditEntries.AddRange(audit.Entries);
        }

        /// <summary>
        /// Disposes the context. The underlying <see cref="T:System.Data.Entity.Core.Objects.ObjectContext" /> is also disposed if it was created
        /// is by this context or ownership was passed to this context when this context was created.
        /// The connection to the database (<see cref="T:System.Data.Common.DbConnection" /> object) is also disposed if it was created
        /// is by this context or ownership was passed to this context when this context was created.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
