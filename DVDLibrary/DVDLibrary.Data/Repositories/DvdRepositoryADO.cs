using DVDLibrary.Data.Interfaces;
using DVDLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVDLibrary.Data.Repositories
{
    public class DvdRepositoryADO : IDvdRepository
    {
        public void Add(Dvd dvd)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("DvdAdd", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter param = new SqlParameter("@DvdID", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(param);

                cmd.Parameters.AddWithValue("@Title", dvd.Title);
                cmd.Parameters.AddWithValue("@ReleaseYear", dvd.ReleaseYear);
                cmd.Parameters.AddWithValue("@Director", dvd.Director);
                cmd.Parameters.AddWithValue("@Rating", dvd.Rating);
                cmd.Parameters.AddWithValue("@Notes", dvd.Notes);

                cn.Open();

                cmd.ExecuteNonQuery();

                dvd.DvdID = (int)param.Value;
            }
        }

        public void Delete(int dvdID)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("DvdDelete", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DvdID", dvdID);

                cn.Open();

                cmd.ExecuteNonQuery();
            }

        }

        public void Edit(Dvd dvd)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("DvdUpdate", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DvdID", dvd.DvdID);
                cmd.Parameters.AddWithValue("@Title", dvd.Title);
                cmd.Parameters.AddWithValue("@ReleaseYear", dvd.ReleaseYear);
                cmd.Parameters.AddWithValue("@Director", dvd.Director);
                cmd.Parameters.AddWithValue("@Rating", dvd.Rating);
                cmd.Parameters.AddWithValue("@Notes", dvd.Notes);

                cn.Open();

                cmd.ExecuteNonQuery();

            }
        }

        public IEnumerable<Dvd> GetAll()
        {
            List<Dvd> Dvds = new List<Dvd>();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetAll", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Dvd dvd = new Dvd();

                        dvd.DvdID = (int)dr["DvdId"];
                        dvd.Title = dr["Title"].ToString();
                        dvd.ReleaseYear = (int)dr["ReleaseYear"];
                        dvd.Director = dr["Director"].ToString();
                        dvd.Rating = dr["Rating"].ToString();

                        if (dr["Notes"] != DBNull.Value)
                            dvd.Notes = dr["Notes"].ToString();

                        Dvds.Add(dvd);

                    }

                }
            }

            return Dvds;
        }

        public Dvd GetByID(int DvdID)
        { 
            Dvd dvd = null;

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("SearchByID", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DvdId", DvdID);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        dvd = new Dvd();
                        dvd.DvdID = (int)dr["DvdId"];
                        dvd.Title = dr["Title"].ToString();
                        dvd.ReleaseYear = (int)dr["ReleaseYear"];
                        dvd.Director = dr["Director"].ToString();
                        dvd.Rating = dr["Rating"].ToString();

                        if (dr["Notes"] != DBNull.Value)
                            dvd.Notes = dr["Notes"].ToString();

                    }

                }

            }

            return dvd;
        }

        public IEnumerable<Dvd> SearchByCategory(string Category, string SearchTerm)
        {
            string command = "Search" + Category;

            List<Dvd> Dvds = new List<Dvd>();
 
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand(command, cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SearchTerm", SearchTerm);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Dvd dvd = new Dvd();

                        dvd.DvdID = (int)dr["DvdId"];
                        dvd.Title = dr["Title"].ToString();
                        dvd.ReleaseYear = (int)dr["ReleaseYear"];
                        dvd.Director = dr["Director"].ToString();
                        dvd.Rating = dr["Rating"].ToString();

                        if (dr["Notes"] != DBNull.Value)
                            dvd.Notes = dr["Notes"].ToString();

                        Dvds.Add(dvd);

                    }

                }
            }

            return Dvds;

        }
    }
}
