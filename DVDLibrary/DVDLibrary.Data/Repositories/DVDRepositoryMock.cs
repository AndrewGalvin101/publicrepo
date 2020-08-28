using DVDLibrary.Data.Interfaces;
using DVDLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.UI.WebControls;

namespace DVDLibrary.Data.Repositories
{
    public class DVDRepositoryMock : IDvdRepository
    {

        private static List<Dvd> Dvds = new List<Dvd>
        {
             new Dvd()
                {
                    DvdID = 1,
                    Title = "The Big Sleep",
                    ReleaseYear = 1946,
                    Director = "John Huston",
                    Rating = "PG-13",
                    Notes =  "Bogey is the first Phillip Marlowe on film."
                },
                new Dvd() {
                    DvdID = 2,
                    Title = "The Long Goodbye",
                    ReleaseYear = 1974,
                    Director = "Robert Altman",
                    Rating = "R",
                    Notes = "Gould plays Marlowe at his ghoulish best."
                 },
                new Dvd()
                {
                    DvdID = 3,
                    Title = "Testing the Mock Repository",
                    ReleaseYear = 2020,
                    Director = "Andrew Galvin",
                    Rating = "G",
                    Notes = "Not much of a plot."
                }
        };

        public void Add(Dvd dvd)
        {
            List<Dvd> dvds = GetAll().ToList();
            int dvdID = dvds[dvds.Count() - 1].DvdID + 1;
            dvd.DvdID = dvdID;
            dvds.Add(dvd);

            HttpRuntime.Cache.Insert("Library", dvds, null);
            
        }

        public void Delete(int DvdID)
        {
            List<Dvd> dvds = GetAll().ToList();
            foreach (var dvd in dvds)
            {
                if (dvd.DvdID == DvdID)
                {
                    dvds.Remove(dvd);
                    break;
                }
            }

            HttpRuntime.Cache.Insert("Library", dvds, null);
        }

        public void Edit(Dvd dvd)
        {
            List<Dvd> dvds = GetAll().ToList();
            foreach (var _dvd in dvds)
            {
                if (_dvd.DvdID == dvd.DvdID)
                {
                    _dvd.Title = dvd.Title;
                    _dvd.ReleaseYear = dvd.ReleaseYear;
                    _dvd.Director = dvd.Director;
                    _dvd.Rating = dvd.Rating;
                    _dvd.Notes = dvd.Notes;
                    break;
                }
            }
            HttpRuntime.Cache.Insert("Library", dvds, null);
        }

        public IEnumerable<Dvd> GetAll()
        {

            if (HttpRuntime.Cache["Library"] == null)
            {

                HttpRuntime.Cache.Insert("Library", Dvds, null,
                    DateTime.Now.AddHours(8), Cache.NoSlidingExpiration);
            }
            else
            {
                Dvds = (List<Dvd>)HttpRuntime.Cache["Library"];
            }

            return Dvds;
        }

       
        public Dvd GetByID(int DvdID)
        {
            List<Dvd> dvds = GetAll().ToList();
            foreach (var dvd in dvds)
            {
                if (dvd.DvdID == DvdID)
                    return dvd;
            }
            throw new Exception("Dvd not found!");
        }

        public IEnumerable<Dvd> SearchByCategory(string Category, string SearchTerm)
        {

            List<Dvd> dvds = GetAll().ToList();
            List<Dvd> _dvds = new List<Dvd>();

            if (Category == "Title")
            {
                foreach (var dvd in dvds)
                {
                    if (dvd.Title.Contains(SearchTerm))
                        _dvds.Add(dvd);
                }
                return _dvds;
            }
            else if (Category == "ReleaseYear")
            {
                foreach (var dvd in dvds)
                {
                    if (dvd.ReleaseYear == int.Parse(SearchTerm))
                        _dvds.Add(dvd);
                }
                return _dvds;
            }
            else if (Category == "Director")
            {
                foreach (var dvd in dvds)
                {
                    if (dvd.Director.Contains(SearchTerm))
                        _dvds.Add(dvd);
                }
                return _dvds;
            }
            else
            {
                foreach (var dvd in dvds)
                {
                    if (dvd.Rating == SearchTerm)
                        _dvds.Add(dvd);
                }
                return _dvds;
            }
        }
    }
}
