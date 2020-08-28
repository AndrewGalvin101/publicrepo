using GuildCars.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildCars.Data.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<GetUserDetails> GetAll();
        EditUserDetails GetByID(string id);
        void EditUser(User user, EditUserDetails details);
    }
}
