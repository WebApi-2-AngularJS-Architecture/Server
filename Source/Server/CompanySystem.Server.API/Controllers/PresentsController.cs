namespace CompanySystem.Server.API.Controllers
{
    using CompanySystem.Services.Data.Contracts;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;

    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Presents")]
    public class PresentsController : ApiController
    {
        private IPresentsService presents;

        public PresentsController(IPresentsService presents)
        {
            this.presents = presents;
        }

        [HttpGet]
        [Route("All")]
        public async Task<IHttpActionResult> GetAvailablePresents()
        {
            var presents = await this.presents.GetAvailablePresents();

            return this.Ok(presents);
        }
    }
}