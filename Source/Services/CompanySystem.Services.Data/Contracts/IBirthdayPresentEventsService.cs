namespace CompanySystem.Services.Data.Contracts
{
    using Common.Contracts;
    using CompanySystem.Data.Models.Models;
    using Server.DataTransferModels.BirthdayPresentEvent;
    using Server.DataTransferModels.Presents;
    using Server.DataTransferModels.Users;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IBirthdayPresentEventsService : IService
    {
        IQueryable<BirthdayPresentEvent> All();

        Task<int> Add(BirthdayPresentEventCreationDataTransferModel birthdayPresentEvent);

        Task<bool> CancelEvent(int eventId);

        Task<ICollection<ActiveEventDataTransferModel>> GetAllVisibleActive(UserBriefDataTransferModel model);

        Task<ICollection<BirthdayPresentEvent>> GetAllVisibleUnactive(UserBriefDataTransferModel model);

        Task<ICollection<PresentDataTransferModel>> GetAvailablePresents();
    }
}
