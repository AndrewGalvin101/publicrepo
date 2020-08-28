using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildCars.Models.Identity
{
    public class GuildCarsDBContext : IdentityDbContext<AppUser>
    {
        public GuildCarsDBContext() : base("GuildCars")
        {

        }
    }
}
