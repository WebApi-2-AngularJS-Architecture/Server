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
        [Route("Unactive")]
        public async Task<IHttpActionResult> GetAllUnactiveEvents([FromUri]UserBriefDataTransferModel model)
        {
            var unactiveEvents = await this.birthdayPresentEvents.GetAllVisibleUnactive(model);

            return this.Ok(unactiveEvents);
        }

        [HttpGet]
        [Route("Statistics")]
        public async Task<IHttpActionResult> GetStatistics([FromUri]UserBriefDataTransferModel model)
        {
            var statistics = await this.birthdayPresentEvents.GetStatistics(model);

            return this.Ok(statistics);
        }

        [HttpPost]
        [Route("Cancel")]
        public async Task<IHttpActionResult> CancelEvent([FromBody] BirthdayPresentEventCancelationDataTransferModel model)
        {
            var isCanceled = await this.birthdayPresentEvents.CancelEvent(model);

            if(!isCanceled)
            {
                return this.BadRequest("Event cannnot be cancelled.");
            }

            return this.Ok("Event successfully cancelled.");
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> CreateEvent([FromBody]BirthdayPresentEventCreationDataTransferModel model)
        {
            var eventId = await this.birthdayPresentEvents.CreateEvent(model);

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
