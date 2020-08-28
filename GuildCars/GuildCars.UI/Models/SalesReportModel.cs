using GuildCars.Models.Queries;
using GuildCars.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GuildCars.UI.Models
{
    public class SalesReportModel
    {
        public IEnumerable<GetUserDetails> Users { get; set; }
        public IEnumerable<SalesReport> SalesReport { get; set; }
    }
}