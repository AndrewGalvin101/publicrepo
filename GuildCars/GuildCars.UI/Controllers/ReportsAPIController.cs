using GuildCars.Data.Factories;
using GuildCars.Models.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GuildCars.UI.Controllers
{
    public class ReportsAPIController : ApiController
    {
        [Route("api/reports/sales")]
        [AcceptVerbs("GET")]
        public IHttpActionResult Search(string user, string fromDate, string toDate)
        {
            var repo = ReportsRepositoryFactory.GetRepository();

            try
            {
                var paramaters = new SalesReportParameters()
                {
                    User = user,
                    FromDate = fromDate,
                    ToDate = toDate
                };

                var results = repo.Search(paramaters);

                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
