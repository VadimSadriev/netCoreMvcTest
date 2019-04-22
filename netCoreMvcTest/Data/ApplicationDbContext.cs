using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace netCoreMvcTest.Data
{
    /// <summary>
    /// represent database conection
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        #region public proporties

        /// <summary>
        /// settings for the application
        /// </summary>
        public DbSet<SettingsDataModel> Settings { get; set; }

        #endregion

        #region constructor

        /// <summary>
        /// default constructr expection database options
        /// </summary>
        /// <param name="options">data base context options</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
