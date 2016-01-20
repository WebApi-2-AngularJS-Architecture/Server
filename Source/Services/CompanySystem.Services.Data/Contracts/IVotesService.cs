namespace CompanySystem.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Contracts;
    using CompanySystem.Data.Models.Models;
    using Server.DataTransferModels.Votes;

    public interface IVotesService : IService
    {
        IQueryable<Vote> All();

        Task<int> Add(VoteCreationDataTransferModel model);

        Task<ICollection<VoteDetailsDataTransferModel>> GetAllVotesForEvent(int eventId);
    }
}