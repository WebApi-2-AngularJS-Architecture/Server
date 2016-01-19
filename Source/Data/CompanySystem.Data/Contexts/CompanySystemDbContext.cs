namespace CompanySystem.Data.Contexts
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models.Models;
    using Contracts;
    using System.Data.Entity;
    using System;

    public class CompanySystemDbContext : IdentityDbContext<User>, ICompanySystemDbContext
    {
        public CompanySystemDbContext()
            : base("CompanySystem", throwIfV1Schema: false)
        {
        }

        public virtual IDbSet<Present> Presents { get; set; }

        public virtual IDbSet<Vote> Votes { get; set; }

        public virtual IDbSet<BirthdayPresentEvent> BirthdayPresentEvent { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BirthdayPresentEvent>()
                .HasRequired(c => c.BirthdayGuy)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BirthdayPresentEvent>()
               .HasRequired(c => c.Creator)
               .WithMany()
               .WillCascadeOnDelete(false);
        }

        public static CompanySystemDbContext Create()
        {
            return new CompanySystemDbContext();
        }
    }
}