using GuildCars.Data.Factories;
using GuildCars.Models.Identity;
using GuildCars.Models.Tables;
using GuildCars.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static GuildCars.UI.App_Start.IdentityConfig;

namespace GuildCars.UI.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private readonly UserManager<User> userMgr = new UserManager<User>(new UserStore<User>(new GuildCarsDBContext()));

        public AdminController()
        {
        }

        public AdminController(ApplicationSignInManager signInManager)
        {
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }


        // GET: /Admin/Index
        [Authorize(Roles = "admin, sales")]
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new AdminIndexModel
            {
                HasPassword = HasPassword(),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        // GET: Admin
        [Authorize(Roles = "admin")]
        public ActionResult Vehicles()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult Add()
        {
            var MakesRepo = MakeRepositoryFactory.GetRepository();
            var model = new AddVehicleModel();
            model.Makes = new SelectList(MakesRepo.GetAll(), "MakeID", "Make");
            var ModelsRepo = ModelRepositoryFactory.GetRepository();
            model.Models = new SelectList(ModelsRepo.GetSelected(1), "ModelID", "Model");

            return View(model);
        }
        [HttpGet]
        public JsonResult GetModels(int MakeID)
        {
            var ModelsRepo = ModelRepositoryFactory.GetRepository();
            var models = new SelectList(ModelsRepo.GetSelected(MakeID), "ModelId", "Model");
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Add(AddVehicleModel model)
        {
            if (ModelState.IsValid)
            {
                var repo = VehicleRepositoryFactory.GetRepository();

                try
                {
                    if (model.ImageUpload != null && model.ImageUpload.ContentLength > 0)
                    {
                        var savepath = Server.MapPath("~/Images");

                        string fileName = Path.GetFileNameWithoutExtension(model.ImageUpload.FileName);
                        string extension = Path.GetExtension(model.ImageUpload.FileName);

                        var filePath = Path.Combine(savepath, fileName + extension);

                        int counter = 1;
                        while (System.IO.File.Exists(filePath))
                        {
                            filePath = Path.Combine(savepath, fileName + counter.ToString() + extension);
                            counter++;
                        }

                        model.ImageUpload.SaveAs(filePath);
                        model.Vehicle.ImageFileName = Path.GetFileName(filePath);
                    }

                    repo.Add(model.Vehicle);
                    return RedirectToAction("Edit", new { id = model.Vehicle.ID });
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            else
            {
                var MakesRepo = MakeRepositoryFactory.GetRepository();
                model.Makes = new SelectList(MakesRepo.GetAll(), "MakeID", "Make");
                var ModelsRepo = ModelRepositoryFactory.GetRepository();
                model.Models = new SelectList(ModelsRepo.GetSelected(model.Vehicle.MakeID), "ModelID", "Model");

                return View(model);
            }
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id)
        {
            var model = new EditVehicleModel();
            var repo = VehicleRepositoryFactory.GetRepository();
            model.Vehicle = repo.GetVehicle(id);
            var MakesRepo = MakeRepositoryFactory.GetRepository();
            model.Makes = new SelectList(MakesRepo.GetAll(), "MakeID", "Make");
            var ModelsRepo = ModelRepositoryFactory.GetRepository();
            model.Models = new SelectList(ModelsRepo.GetSelected(model.Vehicle.MakeID), "ModelID", "Model");
            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Edit(EditVehicleModel model)
        {
            if (ModelState.IsValid)
            {
                var repo = VehicleRepositoryFactory.GetRepository();

                try
                {
                    var oldDetails = repo.GetVehicle(model.Vehicle.ID);

                    if (model.ImageUpload != null && model.ImageUpload.ContentLength > 0)
                    {
                        var savepath = Server.MapPath("~/Images");

                        string fileName = Path.GetFileNameWithoutExtension(model.ImageUpload.FileName);
                        string extension = Path.GetExtension(model.ImageUpload.FileName);

                        var filePath = Path.Combine(savepath, fileName + extension);

                        int counter = 1;
                        while (System.IO.File.Exists(filePath))
                        {
                            filePath = Path.Combine(savepath, fileName + counter.ToString() + extension);
                            counter++;
                        }

                        model.ImageUpload.SaveAs(filePath);
                        model.Vehicle.ImageFileName = Path.GetFileName(filePath);

                        // delete old file
                        var oldPath = Path.Combine(savepath, oldDetails.ImageFileName);
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }
                    else
                    {
                        // they did not replace the old file, so keep the old file name
                        model.Vehicle.ImageFileName = oldDetails.ImageFileName;
                    }

                    repo.Update(model.Vehicle);

                    return RedirectToAction("Edit", new { id = model.Vehicle.ID });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                var repo = VehicleRepositoryFactory.GetRepository();
                model.Vehicle = repo.GetVehicle(model.Vehicle.ID);
                var MakesRepo = MakeRepositoryFactory.GetRepository();
                model.Makes = new SelectList(MakesRepo.GetAll(), "MakeID", "Make");
                var ModelsRepo = ModelRepositoryFactory.GetRepository();
                model.Models = new SelectList(ModelsRepo.GetSelected(model.Vehicle.MakeID), "ModelID", "Model");
                return View(model);
            }
        }

        [Authorize(Roles = "admin")]
        public ActionResult Users()
        {
            var repo = UserRepositoryFactory.GetRepository();
            var model = repo.GetAll();
            return View(model);
        }

        // GET: /Account/Register
        [Authorize(Roles = "admin")]
        public ActionResult AddUser()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> AddUser(AddUserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    FullName = $"{model.FirstName} {model.LastName}"
                };
                var UserMgr = new UserManager<User>(new UserStore<User>(new GuildCarsDBContext()));
                IdentityResult result = await UserMgr.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    UserMgr.AddToRole(user.Id, model.Role);
                    return RedirectToAction("EditUser", "Admin", new { id = user.Id });
                }
                AddErrors(result);
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public ActionResult EditUser(string id)
        {
            var repo = UserRepositoryFactory.GetRepository();
            var model = new EditUserModel();
            model.details = repo.GetByID(id);
            model.user = new User();
            model.user.Id = id;
            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult EditUser(EditUserModel model)
        {
            var repo = UserRepositoryFactory.GetRepository();
            model.user.FirstName = model.details.FirstName;
            model.user.LastName = model.details.LastName;
            model.user.FullName = $"{model.details.FirstName} {model.details.LastName}";
            model.user.Email = model.details.Email;
            model.user.UserName = model.user.Email;
            repo.EditUser(model.user, model.details);
            return View("EditUser", model);
        }

        [Authorize(Roles = "admin, sales")]
        public ActionResult ChangePassword()
        {
            return View();
        }

        // POST: /Admin/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var UserMgr = new UserManager<User>(new UserStore<User>(new GuildCarsDBContext()));
            var result = await UserMgr.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserMgr.FindByIdAsync(User.Identity.GetUserId());
                //if (user != null)
                //{
                //    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                //}
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Makes()
        {
            var repo = MakeRepositoryFactory.GetRepository();
            var model = new MakesViewModel();
            model.makes = repo.GetAll();
            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult AddMake(MakesViewModel model)
        {
            model.newMake.UserName = this.User.Identity.Name;
            var repo = MakeRepositoryFactory.GetRepository();
            try
            {
                repo.Add(model.newMake);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            model = new MakesViewModel();
            model.makes = repo.GetAll();
            return View("Makes", model);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Models()
        {
            var repo = ModelRepositoryFactory.GetRepository();
            var model = new ModelsViewModel();
            model.models = repo.GetAll();
            var MakesRepo = MakeRepositoryFactory.GetRepository();
            model.makes = new SelectList(MakesRepo.GetAll(), "MakeID", "Make");
            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult AddModel(ModelsViewModel model)
        {
            model.newModel.UserName = this.User.Identity.Name;
            var repo = ModelRepositoryFactory.GetRepository();
            try
            {
                repo.Add(model.newModel);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            repo = ModelRepositoryFactory.GetRepository();
            model.models = repo.GetAll();
            var MakesRepo = MakeRepositoryFactory.GetRepository();
            model.makes = new SelectList(MakesRepo.GetAll(), "MakeID", "Make");
            return View("Models", model);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Specials()
        {
            var repo = VehicleRepositoryFactory.GetRepository();
            Session.Abandon();
            var model = new SpecialsViewModel();
            model.specials = repo.GetSpecials();
            return View(model);
        }

        //delete Special 
        [Authorize(Roles = "admin")]
        public ActionResult Special(int id)
        {
            var repo = VehicleRepositoryFactory.GetRepository();
            repo.DeleteSpecial(id);
            Session.Abandon();
            var model = new SpecialsViewModel();
            model.specials = repo.GetSpecials();
            model.newSpecial = null;
            return View("Specials", model);
        }

        //add Special 
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Specials(SpecialsViewModel model)
        {
            var repo = VehicleRepositoryFactory.GetRepository();
            repo.AddSpecial(model.newSpecial);
            model = new SpecialsViewModel();
            model.specials = repo.GetSpecials();
            model.newSpecial = null;
            return View(model);
        }

        #region Helpers

        private bool HasPassword()
        {
            var UserMgr = new UserManager<User>(new UserStore<User>(new GuildCarsDBContext()));
            var user = UserMgr.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}