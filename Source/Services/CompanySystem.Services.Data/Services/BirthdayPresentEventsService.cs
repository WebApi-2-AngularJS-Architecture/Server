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
    using AutoMapper.QueryableExtensions;

    public class BirthdayPresentEventsService : IBirthdayPresentEventsService
    {
        private IRepository<BirthdayPresentEvent> birthdayPresentEvents;
        private IRepository<User> users;

        public BirthdayPresentEventsService(IRepository<BirthdayPresentEvent> birthdayPresentEvents, IRepository<User> users)
        {
            this.birthdayPresentEvents = birthdayPresentEvents;
            this.users = users;
        }

        public IQueryable<BirthdayPresentEvent> All()
        {
            return this.birthdayPresentEvents.All();
        }

        public async Task<int> CreateEvent(BirthdayPresentEventCreationDataTransferModel model)
        {
            if (this.CanCreateEvent(model))
            {
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

                this.birthdayPresentEvents.Add(birthdayPresentEvent);
                await this.birthdayPresentEvents.SaveChangesAsync();

                int id = birthdayPresentEvent.Id;
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

        public async Task<ICollection<BirthdayPresentEventStatistics>> GetStatistics(UserBriefDataTransferModel model)
        {
            var unactiveEvents = await this.GetAllVisibleUnactive(model);
            var allUsers = await this.users.All().Select(x => x.UserName).ToListAsync();

            return this.BuildStatistics(ref unactiveEvents,ref allUsers);
        }

        private ICollection<BirthdayPresentEventStatistics> BuildStatistics(ref ICollection<BirthdayPresentEventDataTransferModel> unactiveEvents, ref List<string> allUsers)
        {
            var statistics = new List<BirthdayPresentEventStatistics>();

            // Cycles through all events and builds the statistics object
            foreach (var unactiveEvent in unactiveEvents)
            {
                var item = new BirthdayPresentEventStatistics()
                {
                    EventId = unactiveEvent.EventId,
                    BirthdayDate = unactiveEvent.BirthdayDate,
                    BirthdayGuyUsername = unactiveEvent.BirthdayGuyUsername,
                    CreatorUsername = unactiveEvent.CreatorUsername
                };

                // Stores the present description and a list of all usernames that voted for this present
                // Key(Present) => Value(Usernames) 
                var votesStats = new Dictionary<string, List<string>>();

                foreach (var vote in unactiveEvent.Votes)
                {
                    if (!votesStats.ContainsKey(vote.BirthdayPresentDescription))
                    {
                        votesStats[vote.BirthdayPresentDescription] = new List<string>();
                    }

                    votesStats[vote.BirthdayPresentDescription].Add(vote.UserVoted);
                }

                var usersVoted = new List<string>();

                foreach (var pair in votesStats)
                {
                    usersVoted.AddRange(pair.Value);
                }

                // We add the birthday guy username to this list
                // Because we need to remove it from the list of users that havent voted using the Except() method
                // Which produces the set difference of two sequences
                usersVoted.Add(item.BirthdayGuyUsername);

                var allUsersNotVoted = allUsers.Except(usersVoted);

                // Attach users that didn't vote for this event
                item.UsersNotVoted = allUsersNotVoted;

                // Attach votes stats
                item.Votes = votesStats;

                // Attach the processed item to the full statistics
                statistics.Add(item);
            }

            return statistics;
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