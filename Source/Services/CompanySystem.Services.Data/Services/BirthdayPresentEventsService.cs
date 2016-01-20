namespace CompanySystem.Services.Data.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CompanySystem.Data.Models.Models;
    using CompanySystem.Services.Data.Contracts;
    using CompanySystem.Server.DataTransferModels.BirthdayPresentEvent;
    using CompanySystem.Data.Contracts;
    using Common.Constants;
    using System.Data.Entity;
    using System.Collections.Generic;
    using Server.DataTransferModels.Users;
    using Server.DataTransferModels.Votes;
    using Server.DataTransferModels.Presents;
    using AutoMapper.QueryableExtensions;
    public class BirthdayPresentEventsService : IBirthdayPresentEventsService
    {
        private IRepository<BirthdayPresentEvent> birthdayPresentEvents;
        private IRepository<User> users;
        private IRepository<Present> presents;

        public BirthdayPresentEventsService(IRepository<BirthdayPresentEvent> birthdayPresentEvents,
            IRepository<User> users,
            IRepository<Present> presents)
        {
            this.birthdayPresentEvents = birthdayPresentEvents;
            this.users = users;
            this.presents = presents;
        }

        public IQueryable<BirthdayPresentEvent> All()
        {
            return this.birthdayPresentEvents.All();
        }

        public async Task<int> CreateEvent(BirthdayPresentEventCreationDataTransferModel model)
        {
            if (this.CanCreateEvent(model))
            {
                // Create model
                var creator = await this.users.All().SingleOrDefaultAsync(x => x.UserName.Equals(model.CreatorUsername));
                var birthdayGuy = await this.users.All().SingleOrDefaultAsync(x => x.UserName.Equals(model.BirthdayGuyUsername));
                var birthdayDate = DateTime.Parse(model.BirthdayDate);

                if (creator == null || birthdayGuy == null || birthdayDate == null)
                {
                    return ServicesConstants.DbModelCreationFailed;
                }

                var birthdayPresentEvent = new BirthdayPresentEvent()
                {
                    Creator = creator,
                    BirthdayGuy = birthdayGuy,
                    BirthdayDate = birthdayDate,
                    IsActive = true
                };

                // Insert model
                this.birthdayPresentEvents.Add(birthdayPresentEvent);
                await this.birthdayPresentEvents.SaveChangesAsync();

                int id = birthdayPresentEvent.Id;

                // Return result
                return id != 0 ? id : ServicesConstants.DbModelInsertionFailed;
            }
            else
            {
                return ServicesConstants.DbModelCreationFailed;
            }

        }

        public async Task<bool> CancelEvent(BirthdayPresentEventCancelationDataTransferModel model)
        {
            var eventForCancelation = await this.birthdayPresentEvents.All().SingleOrDefaultAsync(x => x.Id == model.EventId && x.IsActive);

            if (this.CanCancelEvent(eventForCancelation, model))
            {
                eventForCancelation.IsActive = false;
                // Dark magic
                eventForCancelation.BirthdayGuy = eventForCancelation.BirthdayGuy;

                await this.birthdayPresentEvents.SaveChangesAsync();

                return ServicesConstants.EventCancellationSuccessful;
            }

            return ServicesConstants.EventCancellationFailed;
        }

        public async Task<ICollection<BirthdayPresentEventDataTransferModel>> GetAllVisibleActive(UserBriefDataTransferModel model)
        {
            var activeEvents = await this.birthdayPresentEvents.All()
                .Where(x => !x.BirthdayGuy.UserName.Equals(model.UserName) && x.IsActive)
                .ProjectTo<BirthdayPresentEventDataTransferModel>()
                .ToListAsync();

            return activeEvents;
        }

        public async Task<ICollection<BirthdayPresentEventDataTransferModel>> GetAllVisibleUnactive(UserBriefDataTransferModel model)
        {
            var unactiveEvents = await this.birthdayPresentEvents.All()
               .Where(x => !x.BirthdayGuy.UserName.Equals(model.UserName) && !x.IsActive)
               .ProjectTo<BirthdayPresentEventDataTransferModel>()
               .ToListAsync();

            return unactiveEvents;
        }

        private bool CanCreateEvent(BirthdayPresentEventCreationDataTransferModel model)
        {
            // User cannot create event for his own benefits
            if (model.CreatorUsername.Equals(model.BirthdayGuyUsername))
            {
                return false;
            }

            // Check if there is an event that is already active for the targeted person
            var isActive = this.birthdayPresentEvents.All().Any(x => x.BirthdayGuy.UserName.Equals(model.BirthdayGuyUsername) && x.IsActive);

            if (isActive)
            {
                return false;
            }

            // Check if there was an event from the same type created this year
            var isDoneThisYear = this.birthdayPresentEvents.All()
                .Any(
                    x =>
                        x.BirthdayGuy.UserName.Equals(model.BirthdayGuyUsername) &&
                        x.BirthdayDate.Year.Equals(DateTime.Now.Year));

            if (isDoneThisYear)
            {
                return false;
            }

            // If none of the above is true, then an event may be created.
            return true;
        }

        private bool CanCancelEvent(BirthdayPresentEvent eventForCancelation, BirthdayPresentEventCancelationDataTransferModel model)
        {
            return eventForCancelation != null && eventForCancelation.Creator.UserName.Equals(model.RequestUsername);
        }
    }
}
