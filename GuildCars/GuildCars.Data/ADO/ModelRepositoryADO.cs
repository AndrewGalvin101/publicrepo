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
    public class ModelRepositoryADO : IModelRepository
    {
        public void Add(VehicleModels model)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("AddModel", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter param = new SqlParameter("@ModelID", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(param);


                cmd.Parameters.AddWithValue("@Model", model.Model);
                cmd.Parameters.AddWithValue("@MakeID", model.MakeID);
                cmd.Parameters.AddWithValue("@UserName", model.UserName);

                cn.Open();
                cmd.ExecuteNonQuery();

                model.ModelID = (int)param.Value;
            }
        }

        public IEnumerable<VehicleModels> GetAll()
        {
            List<VehicleModels> models = new List<VehicleModels>();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("ModelsGetAll", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = cn;


                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        VehicleModels currentRow = new VehicleModels();
                        currentRow.ModelID = (int)dr["ModelID"];
                        currentRow.Model = dr["Model"].ToString();
                        currentRow.MakeID = (int)dr["MakeID"];
                        currentRow.CreateDate = (DateTime)dr["CreateDate"];
                        currentRow.UserName = dr["UserName"].ToString();

                        models.Add(currentRow);
                    }

                }

                return models;
            }
        }

        public IEnumerable<VehicleModels> GetSelected(int makeID)
        {
            List<VehicleModels> models = new List<VehicleModels>();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("ModelsGetSelected", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MakeID", makeID);
                cmd.Connection = cn;


                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        VehicleModels currentRow = new VehicleModels();
                        currentRow.ModelID = (int)dr["ModelID"];
                        currentRow.Model = dr["Model"].ToString();

                        models.Add(currentRow);
                    }

                }

                return models;

            }
        }
    }
}

