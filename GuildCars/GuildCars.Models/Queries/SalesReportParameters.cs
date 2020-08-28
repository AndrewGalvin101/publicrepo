using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildCars.Models.Queries
{
    public class SalesReportParameters
    {
        public string User { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; } 
    }
}
