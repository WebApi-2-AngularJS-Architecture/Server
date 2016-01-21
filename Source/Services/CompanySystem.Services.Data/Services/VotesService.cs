namespace CompanySystem.Services.Data.Services
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using CompanySystem.Data.Models.Models;
    using CompanySystem.Data.Contracts;
    using Contracts;
    using Common.Constants;
    using Server.DataTransferModels.Votes;
    using AutoMapper.QueryableExtensions;

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

        public async Task<int> Add(VoteCreationDataTransferModel model)
        {
            var present = this.presents.GetById(model.PresentId);
            var birthdayPresentEvent = this.events.GetById(model.BirthdayPresentEventId);
            var userVoted = await this.users.All().SingleOrDefaultAsync(x=> x.UserName == model.UserVotedUsername);

            var canVote = await this.CanVote(present, birthdayPresentEvent, userVoted);

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

        public async Task<ICollection<VoteDetailsDataTransferModel>> GetAllVotesForEvent(int eventId)
        {
            var votes = await this.votes.All()
                .Where(x => x.BirthdayPresentEventId == eventId)
                .ProjectTo<VoteDetailsDataTransferModel>()
                .ToListAsync();

            return votes;
        }

        private async Task<bool> CanVote(Present present, BirthdayPresentEvent birthdayPresentEvent, User userVoted)
        {
            if (!this.VotingDataIsValid(present,birthdayPresentEvent, userVoted))
            {
                return false;
            }

            if(!birthdayPresentEvent.IsActive)
            {
                return false;
            }

            if (birthdayPresentEvent.BirthdayGuy.UserName == userVoted.UserName)
            {
                return false;
            }

            if(await this.UserAlreadyVoted(birthdayPresentEvent, userVoted))
            {
                return false;
            }
           
            return true;
        }

        private async Task<bool> UserAlreadyVoted(BirthdayPresentEvent birthdayPresentEvent, User userVoted)
        {
            var userAlreadyVoted = await this.votes.All()
                .AnyAsync(
                   x =>
                       x.BirthdayPresentEventId == birthdayPresentEvent.Id &&
                       x.UserVotedId.Equals(userVoted.Id));

            if (userAlreadyVoted)
            {
                return true;
            }

            return false;
        }

        private bool VotingDataIsValid(Present present, BirthdayPresentEvent birthdayPresentEvent, User userVoted)
        {
            if (present == null || birthdayPresentEvent == null || userVoted == null)
            {
                return false;
            }

            return true;
        }
    }
}