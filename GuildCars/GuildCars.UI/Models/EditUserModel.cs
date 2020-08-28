using GuildCars.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuildCars.UI.Models
{
    public class EditUserModel
    {
        public User user { get; set; }
        public EditUserDetails details { get; set; }
    }
}