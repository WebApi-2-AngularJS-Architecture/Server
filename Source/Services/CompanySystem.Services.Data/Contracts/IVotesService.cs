namespace CompanySystem.Services.Data.Contracts
{
    using Common.Contracts;
    using CompanySystem.Data.Models.Models;
    using Server.DataTransferModels.Votes;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IVotesService : IService
    {
        IQueryable<Vote> All();

        Task<int> Add(VotesDataTransferModel model);

        Task<ICollection<Vote>> GetAllVotesForEvent(int eventId);
    }
}
