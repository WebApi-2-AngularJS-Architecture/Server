namespace CompanySystem.Services.Data.Services
{
    using Contracts;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CompanySystem.Data.Models.Models;
    using CompanySystem.Data.Contracts;
    using Server.DataTransferModels.Presents;
    using System.Data.Entity;

    public class PresentsService : IPresentsService
    {
        private IRepository<Present> presents;

        public PresentsService(IRepository<Present> presents)
        {
            this.presents = presents;
        }

        public IQueryable<Present> All()
        {
            return this.presents.All();
        }

        public async Task<ICollection<PresentDataTransferModel>> GetAvailablePresents()
        {
            var presents = await this.presents.All()
                .Select(x => new PresentDataTransferModel()
                {
                    Description = x.Description,
                    Id = x.Id,
                    PriceInEuro = x.PriceInEuro
                })
                .ToListAsync();

            return presents;
        }
    }
}