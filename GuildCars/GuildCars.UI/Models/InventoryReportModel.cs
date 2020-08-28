using GuildCars.Models.Queries;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace GuildCars.UI.Models
{
    public class InventoryReportModel
    {
        public IEnumerable<InventoryReport> New { get; set; }
        public IEnumerable<InventoryReport> Used { get; set; }
    }
}