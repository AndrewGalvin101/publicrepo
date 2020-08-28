using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildCars.Models.Tables
{
    public class Makes
    {
        public int MakeID { get; set; }
        public string Make { get; set; }
        public DateTime CreateDate { get; set; }
        public string UserName { get; set; }
    }
}
