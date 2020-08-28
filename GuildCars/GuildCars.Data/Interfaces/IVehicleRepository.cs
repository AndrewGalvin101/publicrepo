using GuildCars.Models;
using GuildCars.Models.Queries;
using GuildCars.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildCars.Data.Interfaces
{
    public interface IVehicleRepository
    {
        IEnumerable<FeaturedVehicle> GetFeatured();
        List<SalesSpecial> GetSpecials();
        void DeleteSpecial(int id);
        void AddSpecial(SalesSpecial special);
        IEnumerable<VehicleDetailsItem> Search(InventorySearchParameters parameters);
        VehicleDetailsItem GetDetails(int id);
        void Purchase(PurchaseDetails purchase, int id);
        void Add(Vehicle vehicle);
        Vehicle GetVehicle(int id);
        void Update(Vehicle vehicle);
    }
}
