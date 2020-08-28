using GuildCars.Data.Factories;
using GuildCars.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GuildCars.UI.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports
        [Authorize(Roles ="admin")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult Sales()
        {

            var userRepo = UserRepositoryFactory.GetRepository();
            var model = new SalesReportModel();
            model.Users = userRepo.GetAll();
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Inventory()
        {
            var repo = ReportsRepositoryFactory.GetRepository();
            var model = new InventoryReportModel();
            model.New = repo.GetNew();
            model.Used = repo.GetUsed();

            return View(model);
        }
    }
}