using GuildCars.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GuildCars.UI.Models
{
    public class ModelsViewModel
    {
        public IEnumerable<VehicleModels> models { get; set; }
        public VehicleModels newModel { get; set; }
        public IEnumerable<SelectListItem> makes { get; set; }
    }
}