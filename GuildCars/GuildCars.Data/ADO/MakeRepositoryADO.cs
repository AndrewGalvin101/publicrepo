using GuildCars.Data.Interfaces;
using GuildCars.Models.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildCars.Data.ADO
{
    public class MakeRepositoryADO : IMakeRepository
    {
        public void Add(Makes make)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("AddMake", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter param = new SqlParameter("@MakeID", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(param);


                cmd.Parameters.AddWithValue("@Make", make.Make);
                cmd.Parameters.AddWithValue("@UserName", make.UserName);

                cn.Open();
                cmd.ExecuteNonQuery();

                make.MakeID = (int)param.Value;
            }
        }

        public IEnumerable<Makes> GetAll()
        {
            List<Makes> makes = new List<Makes>();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("MakesSelectAll", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Makes currentRow = new Makes();
                        currentRow.MakeID = (int)dr["MakeID"];
                        currentRow.Make = dr["Make"].ToString();
                        currentRow.CreateDate = (DateTime)dr["CreateDate"];
                        currentRow.UserName = dr["UserName"].ToString();

                        makes.Add(currentRow);
                    }

                }

                return makes;

            }
        }
    }
}
