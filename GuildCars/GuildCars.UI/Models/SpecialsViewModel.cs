using GuildCars.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuildCars.UI.Models
{
    public class SpecialsViewModel
    {
       public List<SalesSpecial> specials { get; set; }
       public SalesSpecial newSpecial { get; set; }
    }
}