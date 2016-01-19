namespace CompanySystem.Data.Migrations
{
    using Contexts;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models.Models;

    public sealed class Configuration : DbMigrationsConfiguration<CompanySystemDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(CompanySystemDbContext context)
        {
            if (context.Presents.Count() == 0)
            {
                var presents = new[] 
                {
                    new Present() { Description = "Board Game", PriceInEuro = 50 },
                    new Present() { Description = "Trip to a foreign country", PriceInEuro = 550 },
                    new Present() { Description = "Puzzle", PriceInEuro = 80 },
                    new Present() { Description = "Flowers", PriceInEuro = 39 },
                    new Present() { Description = "Parachute flight with instructor", PriceInEuro = 150 },
                    new Present() { Description = "Parfume", PriceInEuro = 90 },
                };

                context.Presents.AddOrUpdate(presents);
                context.SaveChanges();
            }

        }
    }
}