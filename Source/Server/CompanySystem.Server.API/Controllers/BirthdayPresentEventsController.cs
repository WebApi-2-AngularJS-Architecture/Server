namespace CompanySystem.Server.API.Controllers
{
    using Common.Constants;
    using DataTransferModels.BirthdayPresentEvent;
    using DataTransferModels.Users;
    using Services.Common.Constants;
    using Services.Data.Contracts;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;

    [Authorize]
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
                return this.BadRequest(ServerConstants.CancelEventErrorMessage);
            }

            return this.Ok(ServerConstants.CancelEventSuccessMessage);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> CreateEvent([FromBody]BirthdayPresentEventCreationDataTransferModel model)
        {
            var eventId = await this.birthdayPresentEvents.CreateEvent(model);

            if(eventId == ServicesConstants.DbModelInsertionFailed)
            {
                return this.BadRequest(ServerConstants.EventInsertionErrorMessage);
            }
            else if(eventId == ServicesConstants.DbModelCreationFailed)
            {
                return this.BadRequest(ServerConstants.EventCreationErrorMessage);
            }

            return this.Ok(eventId);
        }
    }
}
