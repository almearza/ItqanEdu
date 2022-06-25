using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace QuranEducation.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string LangCode { get; set; }
        public string FullName { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Tutorial> Tutorials { get; set; }
        public DbSet<Inbox> Inbox { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<InstructorProfile> InstructorProfiles { get; set; }
        public DbSet<Assigment> Assigments { get; set; }
        public DbSet<AssigmentAttachment> AssigmentAttachment { get; set; }
        
        public DbSet<StudentProfile> StudentProfiles { get; set; }
        public DbSet<StudentTutorial> StudentTutorials { get; set; }
        public DbSet<AssSolution> AssSolutions { get; set; }
        public DbSet<SolutionAttachment> SolutionAttachments { get; set; }
        public DbSet<InstEval> InstEval { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public ApplicationDbContext()
            : base("QuranEdu", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}