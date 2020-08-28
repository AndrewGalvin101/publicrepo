using GuildCars.Models.Queries;
using GuildCars.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuildCars.UI.Models
{
    public class IndexModel
    {
        public IEnumerable<FeaturedVehicle> Featured { get; set; }
        public List<SalesSpecial> Specials { get; set; }
    }
}