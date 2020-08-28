using GuildCars.Data;
using GuildCars.Data.ADO;
using GuildCars.Data.Factories;
using GuildCars.Models;
using GuildCars.Models.Tables;
using GuildCars.UI.Controllers;
using GuildCars.UI.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildCars.Tests.IntegrationTests
{
    [TestFixture]
    public class ADOTests
    {
        [SetUp]
        public void Init()
        {

            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["GuildCars"].ConnectionString))
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "DBReset";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Connection = cn;
                cn.Open();

                cmd.ExecuteNonQuery();

            }
        }

        [Test]
        public void CanLoadFeatured()
        {
            var repo = new VehicleRepositoryADO();
            var featured = repo.GetFeatured().ToList();

            Assert.AreEqual(5, featured.Count);
            Assert.AreEqual("Audi", featured[0].Make);
            Assert.AreEqual("F150", featured[1].Model);
        }

        [Test]
        public void CanLoadSpecials()
        {
            var repo = new VehicleRepositoryADO();
            var specials = repo.GetSpecials().ToList();

            Assert.AreEqual(3, specials.Count);
            Assert.AreEqual("GuildCars Huge Sale!!!", specials[0].SpecialTitle);
        }

        [Test]
        public void CanLoadMakes()
        {
            var repo = new MakeRepositoryADO();
            var makes = repo.GetAll().ToList();

            Assert.AreEqual(3, makes.Count);
            Assert.AreEqual(makes[2].Make, "Ford");
        }

        [Test]
        public void CanLoadSelectedModels()
        {
            var repo = new ModelRepositoryADO();
            int makeID = 1;
            var models = repo.GetSelected(makeID).ToList();

            Assert.AreEqual(3, models.Count);
            Assert.AreEqual("A8", models[2].Model);

            makeID = 3;
            models = repo.GetSelected(makeID).ToList();

            Assert.AreEqual(1, models.Count);
            Assert.AreEqual("F150", models[0].Model);
        }

        [Test]
        public void CanGetDetails()
        {
            var repo = new VehicleRepositoryADO();
            int id = 5;
            var vehicle = repo.GetDetails(id);

            Assert.AreEqual("Audi", vehicle.Make);
            Assert.AreEqual("A8", vehicle.Model);
            Assert.AreEqual(2020, vehicle.Year);
            Assert.AreEqual("Car", vehicle.BodyStyle);
            Assert.AreEqual("Yellow", vehicle.Color);
            Assert.AreEqual(10, vehicle.Mileage);
        }

        [Test]
        public void CanAddPurchase()
        {
            var purchase = new PurchaseDetails();
            purchase.Name = "Andrew Galvin";
            purchase.Email = "ag@asp.net";
            purchase.Street1 = "20 Stable Court";
            purchase.City = "Wilmington";
            purchase.State = "DE";
            purchase.ZipCode = "19803";
            purchase.PurchasePrice = 17995;
            purchase.PurchaseType = "Cash";
            purchase.UserID = "00000000-0000-0000-0000-000000000000";

            var repo = new VehicleRepositoryADO();
            int id = 3;
            repo.Purchase(purchase, id);

            Assert.AreEqual(2, purchase.PurchaseID);

            var vehicle = repo.GetDetails(id);

            Assert.AreEqual(2, vehicle.PurchaseID);
        }

        [Test]
        public void CanAddVehicle()
        {
            var vehicle = new Vehicle();
            vehicle.MakeID = 1;
            vehicle.ModelID = 1;
            vehicle.TypeID = 1;
            vehicle.Year = 2020;
            vehicle.TransmissionID = 1;
            vehicle.ColorID = 1;
            vehicle.InteriorID = 1;
            vehicle.BodyStyleID = 1;
            vehicle.Mileage = 50;
            vehicle.VIN = "A12B";
            vehicle.MSRP = 20000;
            vehicle.SalePrice = 18000;
            vehicle.Description = "This real-deal is a complete steal!";
            vehicle.ImageFileName = "placeholder.png";
       //     vehicle.Featured = false;

            var repo = new VehicleRepositoryADO();
            repo.Add(vehicle);

            Assert.AreEqual(8, vehicle.ID);
        }

        [Test]
        public void CanAddMake()
        {
            var repo = new MakeRepositoryADO();
            Makes newMake = new Makes()
            {
                Make = "Chevy",
                UserName = "admin@guildcars.com"
            };

            repo.Add(newMake);

            var makes = repo.GetAll();

            Assert.AreEqual(makes.Count(), 4);
        }

        [Test]
        public void CanAddModel()
        {
            var repo = new ModelRepositoryADO();
            VehicleModels newModel = new VehicleModels()
            {
                Model = "Citation",
                MakeID = 3,
                UserName = "admin@guildcars.com"
            };
            repo.Add(newModel);
            var models = repo.GetAll();
            Assert.AreEqual(models.Count(), 6);
        }

        [Test]
        public void CanAddAndDeleteSpecials()
        {
            var repo = new VehicleRepositoryADO();
            SalesSpecial newSpecial = new SalesSpecial()
            {
                SpecialTitle = "A New Special!",
                SpecialDescription = "This is only a test."
            };
            repo.AddSpecial(newSpecial);
            var specials = repo.GetSpecials();
            Assert.AreEqual(specials.Count(), 4);
            repo.DeleteSpecial(4);
            specials = repo.GetSpecials();
            Assert.AreEqual(specials.Count(), 3);
        }

        [Test]
        public async Task CanAddUserAsync()
        {
            var newUser = new AddUserModel()
            {
                FirstName = "Tasty",
                LastName = "Cake",
                Email = "tasty@cake.net",
                Role = "sales",
                Password = "tasty!",
                ConfirmPassword = "tasty!"
            };
            AdminController admin = new AdminController();
            await admin.AddUser(newUser);
            var repo = new UserRepositoryADO();
            var users = repo.GetAll();
            Assert.AreEqual(users.Count(), 3);
        }
    }
}
