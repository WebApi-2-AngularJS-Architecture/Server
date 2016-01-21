namespace CompanySystem.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Contracts;
    using CompanySystem.Data.Models.Models;
    using Server.DataTransferModels.BirthdayPresentEvent;
    using Server.DataTransferModels.Users;

    public interface IBirthdayPresentEventsService : IService
    {
        IQueryable<BirthdayPresentEvent> All();

        Task<int> CreateEvent(BirthdayPresentEventCreationDataTransferModel birthdayPresentEvent);

        Task<bool> CancelEvent(BirthdayPresentEventCancelationDataTransferModel model);

        Task<ICollection<BirthdayPresentEventStatistics>> GetStatistics(UserBriefDataTransferModel model);

        Task<ICollection<BirthdayPresentEventDataTransferModel>> GetAllVisibleActive(UserBriefDataTransferModel model);

        Task<ICollection<BirthdayPresentEventDataTransferModel>> GetAllVisibleUnactive(UserBriefDataTransferModel model);
    }
}