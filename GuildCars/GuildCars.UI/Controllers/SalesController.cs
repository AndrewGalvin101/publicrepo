using GuildCars.Data.Factories;
using GuildCars.Models;
using GuildCars.UI.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GuildCars.UI.Controllers
{
    public class SalesController : Controller
    {
        public SalesController()
        {

        }

        // GET: Sales
        [Authorize(Roles = "admin,sales")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "admin,sales")]
        public ActionResult Purchase(int id)
        {
            var repo = VehicleRepositoryFactory.GetRepository();
            var model = new PurchaseView();
            model.vehicle = repo.GetDetails(id);

            return View(model);
        }

        [Authorize(Roles = "admin,sales")]
        [HttpPost]
        public ActionResult Purchase(PurchaseView model, int id)
        {
            if (ModelState.IsValid)
            {
                var repo = VehicleRepositoryFactory.GetRepository();

                try
                {
                    model.purchaser.UserID = User.Identity.GetUserId();
                        //AuthorizeUtilities.GetUserId(this);
                    repo.Purchase(model.purchaser, id);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return View("Index"); 
            }
            else
            {
                var repo = VehicleRepositoryFactory.GetRepository();
                model.vehicle = repo.GetDetails(id);
                return View(model);
            }
        }
    }
}