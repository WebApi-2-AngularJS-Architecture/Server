namespace CompanySystem.Server.API.Controllers
{
    using Common.Constants;
    using DataTransferModels.Votes;
    using Services.Common.Constants;
    using Services.Data.Contracts;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;

    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Votes")]
    public class VotesController : ApiController
    {
        private IVotesService votes;

        public VotesController(IVotesService votes)
        {
            this.votes = votes;
        }

        [HttpGet]
        [Route("All/{eventId}")]
        public async Task<IHttpActionResult> GetAllVotesForEvent(int eventId)
        {
            var votes = await this.votes.GetAllVotesForEvent(eventId);

            if(votes.Count > 0)
            {
                return this.Ok(votes);
            }

            return this.Ok(ServerConstants.NoVotesMessage);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> VoteForEvent([FromBody]VoteCreationDataTransferModel model)
        {
            var result = await this.votes.Add(model);

            if(result == ServicesConstants.VoteCreationSuccessful)
            {
                return this.Ok(ServerConstants.VoteSuccessfulMessage);
            }

            return this.BadRequest(ServerConstants.VoteFailedMessage);
        }
    }
}