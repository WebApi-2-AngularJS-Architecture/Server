namespace CompanySystem.Services.Data.Services
{
    using Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CompanySystem.Data.Models.Models;
    using CompanySystem.Data.Contracts;

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
    }
}
