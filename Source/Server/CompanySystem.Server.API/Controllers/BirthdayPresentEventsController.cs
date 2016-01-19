namespace CompanySystem.Server.API.Controllers
{
    using DataTransferModels.BirthdayPresentEvent;
    using DataTransferModels.Users;
    using DataTransferModels.Votes;
    using Services.Data.Contracts;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;

    //[Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Events")]
    public class BirthdayPresentEventsController : ApiController
    {
        private IBirthdayPresentEventsService birthdayPresentEvents;

        public BirthdayPresentEventsController(IBirthdayPresentEventsService birthdayPresentEvents)
        {
            this.birthdayPresentEvents = birthdayPresentEvents;
        }

        [HttpGet]
        [Route("Active")]
        public async Task<IHttpActionResult> GetAllActiveEvents([FromUri]UserBriefDataTransferModel model)
        {
            var activeEvents = await this.birthdayPresentEvents.GetAllVisibleActive(model);

            return this.Ok(activeEvents);
        }

        [HttpGet]
        [Route("Presents")]
        public async Task<IHttpActionResult> GetAvailablePresents()
        {
            var presents = await this.birthdayPresentEvents.GetAvailablePresents();

            return this.Ok(presents);
        }

        [HttpGet]
        [Route("Unactive")]
        public async Task<IHttpActionResult> GetAllUnactiveEvents([FromUri]UserBriefDataTransferModel model)
        {
            var unactiveEvents = await this.birthdayPresentEvents.GetAllVisibleUnactive(model);

            // TODO: Use automapper for cleaner code
            var responseObjects = unactiveEvents.Select(x => new ActiveEventDataTransferModel()
            {
                BirthdayDate = x.BirthdayDate,
                BirthdayGuyUsername = x.BirthdayGuy.UserName,
                CreatorUsername = x.Creator.UserName,
                IsActive = x.IsActive,
                Votes = x.Votes.Select(v => new VotesDetailedDataTransferModel()
                {
                    BirthdayPresentDescription = v.Present.Description,
                    UserVoted = v.UserVoted.UserName,
                    EventId = v.BirthdayPresentEventId
                }).ToList()
            }).ToList();

            return this.Ok(responseObjects);
        }

        [HttpPost]
        [Route("Cancel/{id}")]
        public async Task<IHttpActionResult> CancelEvent(int id)
        {
            var isCanceled = await this.birthdayPresentEvents.CancelEvent(id);

            return this.Ok(isCanceled);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> CreateEvent([FromUri]BirthdayPresentEventCreationDataTransferModel model)
        {
            var eventId = await this.birthdayPresentEvents.Add(model);

            if(eventId == -1)
            {
                return this.BadRequest("Event insertion failed.");
            }
            else if(eventId == -2)
            {
                return this.BadRequest("Event creation failed. An active event from the same type already exists or you must wait for a year to pass untill next creation.");
            }

            return this.Ok(eventId);
        }
    }
}
