using DVDLibrary.Data.Factories;
using DVDLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace DVDLibrary.UI.Controllers
{
    public class HomeAPIController : ApiController
    {
        // GET: DvdAPI

        [System.Web.Http.Route("api/home/dvds")]
        [System.Web.Http.AcceptVerbs("GET")]
        public IHttpActionResult LoadDvds()
        {
            var repo = DvdRepositoryFactory.GetRepository();

            try
            {
                var result = repo.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [System.Web.Http.Route("api/home/dvd/{DvdID}")]
        [System.Web.Http.AcceptVerbs("GET")]
        public IHttpActionResult DvdDetails(int dvdID)
        {
            var repo = DvdRepositoryFactory.GetRepository();

            try
            {
              
                var result = repo.GetByID(dvdID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [System.Web.Http.Route("api/home/dvd")]
        [System.Web.Http.AcceptVerbs("POST")]
        public IHttpActionResult Add(Dvd dvd)
        {
            var repo = DvdRepositoryFactory.GetRepository();

            try
            {
                repo.Add(dvd);
                System.Web.Http.Results.OkResult okResult = Ok();
                return okResult;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [System.Web.Http.Route("api/home/dvd/{DvdID}")]
        [System.Web.Http.AcceptVerbs("PUT")]
        public IHttpActionResult Update(Dvd dvd)
        {
            var repo = DvdRepositoryFactory.GetRepository();

            try
            {
                repo.Edit(dvd);
                return Ok();
                //System.Web.Http.Results.OkResult okResult = Ok();
                //return okResult;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [System.Web.Http.Route("api/home/dvd/{DvdID}")]
        [System.Web.Http.AcceptVerbs("DELETE")]
        public IHttpActionResult Remove(int dvdID)
        {
            var repo = DvdRepositoryFactory.GetRepository();

            try
            {
                repo.Delete(dvdID);
                return Ok();
                //System.Web.Http.Results.OkResult okResult = Ok();
                //return okResult;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [System.Web.Http.Route("api/home/dvds/{Category}/{SearchTerm}")]
        [System.Web.Http.AcceptVerbs("GET")]
        public IHttpActionResult SearchByCategory(string Category, string SearchTerm)
        {
            var repo = DvdRepositoryFactory.GetRepository();

            try
            {
                var result = repo.SearchByCategory(Category, SearchTerm);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}