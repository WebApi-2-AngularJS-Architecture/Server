namespace CompanySystem.Services.Data.Services
{
    using Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CompanySystem.Data.Models.Models;
    using Server.DataTransferModels.Votes;
    using CompanySystem.Data.Contracts;
    using Common.Constants;
    using System.Data.Entity;
    public class VotesService : IVotesService
    {
        private IRepository<Vote> votes;
        private IRepository<Present> presents;
        private IRepository<BirthdayPresentEvent> events;
        private IRepository<User> users;

        public VotesService(IRepository<Vote> votes, 
            IRepository<Present> presents, 
            IRepository<BirthdayPresentEvent> events,
            IRepository<User> users)
        {
            this.votes = votes;
            this.presents = presents;
            this.events = events;
            this.users = users;
        }

        public IQueryable<Vote> All()
        {
            return this.votes.All();
        }

        public async Task<int> Add(VotesDataTransferModel model)
        {
            var present = this.presents.GetById(model.PresentId);
            var birthdayPresentEvent = this.events.GetById(model.BirthdayPresentEventId);
            var userVoted = await this.users.All().SingleOrDefaultAsync(x=> x.UserName == model.UserVotedUsername);

            var canVote = await this.CanCreateVote(present, birthdayPresentEvent, userVoted);

            if (!canVote)
            {
                return ServicesConstants.DbModelCreationFailed;
            }

            var vote = new Vote()
            {
                Present = present,
                UserVoted = userVoted,
                BirthdayPresentEvent = birthdayPresentEvent
            };

            this.votes.Add(vote);
            await this.votes.SaveChangesAsync();

            return ServicesConstants.VoteCreationSuccessful;
        }

        public async Task<ICollection<Vote>> GetAllVotesForEvent(int eventId)
        {
            var votes = await this.votes.All().Where(x => x.BirthdayPresentEventId == eventId).ToListAsync();

            return votes;
        }

        private async Task<bool> CanCreateVote(Present present, BirthdayPresentEvent birthdayPresentEvent, User userVoted)
        {
            // Cannot vote with invalid data
            if (present == null || birthdayPresentEvent == null || userVoted == null)
            {
                return false;
            }

            // Cannot vote for unactive event
            if(!birthdayPresentEvent.IsActive)
            {
                return false;
            }

            // Cannot vote on one event twice
            var userAlreadyVoted = await this.votes.All()
               .AnyAsync(
                   x =>
                       x.BirthdayPresentEventId == birthdayPresentEvent.Id &&
                       x.UserVotedId.Equals(userVoted.Id));

            if (userAlreadyVoted)
            {
                return false;
            }

            return true;
        }
    }
}
