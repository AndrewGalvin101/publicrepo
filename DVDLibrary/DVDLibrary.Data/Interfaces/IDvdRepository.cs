using DVDLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVDLibrary.Data.Interfaces
{
    public interface IDvdRepository
    {
        IEnumerable<Dvd> GetAll();
        Dvd GetByID(int DvdID);
        IEnumerable <Dvd> SearchByCategory(string Category, string SearchTerm);
        void Add(Dvd dvd);
        void Edit(Dvd dvd);
        void Delete(int DvdID);
    }
}
