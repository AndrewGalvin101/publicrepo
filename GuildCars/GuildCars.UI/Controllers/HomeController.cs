using GuildCars.Data.Factories;
using GuildCars.Models.Identity;
using GuildCars.Models.Tables;
using GuildCars.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace GuildCars.UI.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var repo = VehicleRepositoryFactory.GetRepository();
            var indexModel = new IndexModel();
            indexModel.Featured = repo.GetFeatured();
            indexModel.Specials = repo.GetSpecials();

            return View(indexModel);
            
        }

        // GET: Specials
        public ActionResult Specials()
        {
            var model = VehicleRepositoryFactory.GetRepository().GetSpecials();
            return View(model);
        }

        public ActionResult Contact(string id)
        {
            var model = new ContactUsModel();
            model.insert = id;
            return View(model);
        }

        [HttpPost]
        public ActionResult Contact(ContactUsModel model, string id)
        {
            if (ModelState.IsValid)
            {
                //TODO: send an email to the admin with the user's contact info and message

                //use model.VIN to display a success message in textarea of Contact Us page
                model.insert = "YOUR MESSAGE HAS BEEN SUBMITTED. THANK YOU.";
            }
            else
            {
                //send the model back to the user
                //use model.VIN to reinsert user's message in textarea of Contact US page
                model.insert = model.contact.Message;
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            var model = new LoginViewModel();

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userManager = HttpContext.GetOwinContext().GetUserManager<UserManager<AppUser>>();
            var authManager = HttpContext.GetOwinContext().Authentication;

            // attempt to load the user with this password
            AppUser user = userManager.Find(model.UserName, model.Password);

            // user will be null if the password or user name is bad
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password");

                return View(model);
            }
            else
            {
                // successful login, set up their cookies and send them on their way
                var identity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                authManager.SignIn(new AuthenticationProperties { IsPersistent = model.RememberMe }, identity);

                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index");
            }
        }
    }
}