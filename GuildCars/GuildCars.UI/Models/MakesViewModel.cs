using GuildCars.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuildCars.UI.Models
{
    public class MakesViewModel
    {
        public IEnumerable<Makes> makes { get; set; }
        public Makes newMake { get; set; }
    }
}