using GuildCars.Data.Interfaces;
using GuildCars.Models;
using GuildCars.Models.Queries;
using GuildCars.Models.Tables;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildCars.Data.ADO
{
    public class VehicleRepositoryADO : IVehicleRepository
    {
        public void Add(Vehicle vehicle)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("AddVehicle", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter param = new SqlParameter("@ID", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(param);


                cmd.Parameters.AddWithValue("@MakeID", vehicle.MakeID);
                cmd.Parameters.AddWithValue("@ModelID", vehicle.ModelID);
                cmd.Parameters.AddWithValue("@TypeID", vehicle.TypeID);
                cmd.Parameters.AddWithValue("@BodyStyleID", vehicle.BodyStyleID);
                cmd.Parameters.AddWithValue("@Year", vehicle.Year);
                cmd.Parameters.AddWithValue("@TransmissionID", vehicle.TransmissionID);
                cmd.Parameters.AddWithValue("@ColorID", vehicle.ColorID);
                cmd.Parameters.AddWithValue("@InteriorID", vehicle.InteriorID);
                cmd.Parameters.AddWithValue("@Mileage", vehicle.Mileage);
                cmd.Parameters.AddWithValue("@VIN", vehicle.VIN);
                cmd.Parameters.AddWithValue("@MSRP", vehicle.MSRP);
                cmd.Parameters.AddWithValue("@SalePrice", vehicle.SalePrice);
                cmd.Parameters.AddWithValue("@Description", vehicle.Description);
                cmd.Parameters.AddWithValue("@ImageFileName", vehicle.ImageFileName);
                cmd.Parameters.AddWithValue("@Featured", false);

                cn.Open();

                cmd.ExecuteNonQuery();

                vehicle.ID = (int)param.Value;
            }
        }

        public void Update(Vehicle vehicle)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("UpdateVehicle", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", vehicle.ID);
                cmd.Parameters.AddWithValue("@MakeID", vehicle.MakeID);
                cmd.Parameters.AddWithValue("@ModelID", vehicle.ModelID);
                cmd.Parameters.AddWithValue("@TypeID", vehicle.TypeID);
                cmd.Parameters.AddWithValue("@BodyStyleID", vehicle.BodyStyleID);
                cmd.Parameters.AddWithValue("@Year", vehicle.Year);
                cmd.Parameters.AddWithValue("@TransmissionID", vehicle.TransmissionID);
                cmd.Parameters.AddWithValue("@ColorID", vehicle.ColorID);
                cmd.Parameters.AddWithValue("@InteriorID", vehicle.InteriorID);
                cmd.Parameters.AddWithValue("@Mileage", vehicle.Mileage);
                cmd.Parameters.AddWithValue("@VIN", vehicle.VIN);
                cmd.Parameters.AddWithValue("@MSRP", vehicle.MSRP);
                cmd.Parameters.AddWithValue("@SalePrice", vehicle.SalePrice);
                cmd.Parameters.AddWithValue("@Description", vehicle.Description);
                cmd.Parameters.AddWithValue("@ImageFileName", vehicle.ImageFileName);
                cmd.Parameters.AddWithValue("@Featured", vehicle.Featured);

                cn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public void Purchase(PurchaseDetails purchase, int id)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("AddPurchase", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter param = new SqlParameter("@PurchaseId", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(param);

                cmd.Parameters.AddWithValue("@Name", purchase.Name);

                if (!string.IsNullOrEmpty(purchase.Email))
                {
                    cmd.Parameters.AddWithValue("@Email", purchase.Email);
                }
                else
                {
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(purchase.Phone))
                {
                    cmd.Parameters.AddWithValue("@Phone", purchase.Phone);
                }
                else
                {
                    cmd.Parameters.Add("@Phone", SqlDbType.VarChar).Value = DBNull.Value;
                }
                cmd.Parameters.AddWithValue("@Street1", purchase.Street1);

                if (!string.IsNullOrEmpty(purchase.Street2))
                {
                    cmd.Parameters.AddWithValue("@Street2", purchase.Street2);
                }
                else
                {
                    cmd.Parameters.Add("@Street2", SqlDbType.VarChar).Value = DBNull.Value;
                }
                cmd.Parameters.AddWithValue("@City", purchase.City);
                cmd.Parameters.AddWithValue("@State", purchase.State);
                cmd.Parameters.AddWithValue("@Zipcode", purchase.ZipCode);
                cmd.Parameters.AddWithValue("@PurchasePrice", purchase.PurchasePrice);
                cmd.Parameters.AddWithValue("@PurchaseType", purchase.PurchaseType);
                cmd.Parameters.AddWithValue("@UserID", purchase.UserID);
                cmd.Connection = cn;

                cn.Open();
                cmd.ExecuteNonQuery();

                purchase.PurchaseID = (int)param.Value;
                cn.Close();

                SqlCommand cmd2 = new SqlCommand("RecordPurchase", cn);
                cmd2.CommandType = System.Data.CommandType.StoredProcedure;

                cmd2.Parameters.AddWithValue("@ID", id);
                cmd2.Parameters.AddWithValue("@PurchaseID", purchase.PurchaseID);
                cmd2.Connection = cn;
                cn.Open();
                cmd2.ExecuteNonQuery();
            }
        }

        public VehicleDetailsItem GetDetails(int id)
        {
            VehicleDetailsItem vehicle = new VehicleDetailsItem();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetDetails", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Connection = cn;

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        vehicle.ID = (int)dr["ID"];
                        vehicle.Make = dr["Make"].ToString();
                        vehicle.Model = dr["Model"].ToString();
                        vehicle.Year = (int)dr["Year"];
                        vehicle.BodyStyle = dr["BodyStyle"].ToString();
                        vehicle.Transmission = dr["Transmission"].ToString();
                        vehicle.Color = dr["Color"].ToString();
                        vehicle.Interior = dr["Interior"].ToString();
                        vehicle.Mileage = (int)dr["Mileage"];
                        vehicle.VIN = dr["VIN#"].ToString();
                        vehicle.SalePrice = (decimal)dr["SalePrice"];
                        vehicle.MSRP = (decimal)dr["MSRP"];
                        vehicle.Description = dr["Description"].ToString();

                        if (dr["ImageFileName"] != DBNull.Value)
                            vehicle.ImageFileName = dr["ImageFileName"].ToString();

                        if (dr["PurchaseID"] != DBNull.Value)
                            vehicle.PurchaseID = (int)dr["PurchaseID"];

                    }
                }
            }

            return vehicle;
        }

        public Vehicle GetVehicle(int id)
        {
            Vehicle vehicle = new Vehicle();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetVehicle", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Connection = cn;

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        vehicle.ID = (int)dr["ID"];
                        vehicle.MakeID = (int)dr["MakeID"];
                        vehicle.ModelID = (int)dr["ModelID"];
                        vehicle.TypeID = (int)dr["TypeID"];
                        vehicle.Year = (int)dr["Year"];
                        vehicle.BodyStyleID = (int)dr["BodyStyleID"];
                        vehicle.TransmissionID = (int)dr["TransmissionID"];
                        vehicle.ColorID = (int)dr["ColorID"];
                        vehicle.InteriorID = (int)dr["InteriorID"];
                        vehicle.Mileage = (int)dr["Mileage"];
                        vehicle.VIN = dr["VIN#"].ToString();
                        vehicle.SalePrice = (decimal)dr["SalePrice"];
                        vehicle.MSRP = (decimal)dr["MSRP"];
                        vehicle.Description = dr["Description"].ToString();

                        if (dr["ImageFileName"] != DBNull.Value)
                            vehicle.ImageFileName = dr["ImageFileName"].ToString();

                        vehicle.Featured = (bool)dr["Featured"];

                        if (dr["PurchaseID"] != DBNull.Value)
                            vehicle.PurchaseID = (int)dr["PurchaseID"];
                    }
                }
            }

            return vehicle;
        }

        public IEnumerable<FeaturedVehicle> GetFeatured()
        {
            List<FeaturedVehicle> featured = new List<FeaturedVehicle>();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetFeatured", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var vehicle = new FeaturedVehicle();
                        vehicle.ID = (int)dr["ID"];
                        vehicle.Make = dr["Make"].ToString();
                        vehicle.Model = dr["Model"].ToString();
                        vehicle.Year = (int)dr["Year"];  
                        vehicle.SalePrice = (decimal)dr["SalePrice"];
                        vehicle.ImageFileName = dr["ImageFileName"].ToString();

                        featured.Add(vehicle);

                    }

                }

            }

            return featured;
        }

        public List<SalesSpecial> GetSpecials()
        {

            List<SalesSpecial> specials = new List<SalesSpecial>();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetSpecials", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var special = new SalesSpecial();
                        special.SpecialID = (int)dr["SpecialID"];
                        special.SpecialTitle = dr["SpecialTitle"].ToString();
                        special.SpecialDescription = dr["SpecialDescription"].ToString();

                        specials.Add(special);

                    }

                }

            }

            return specials;
        }

        public void DeleteSpecial(int id)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("DeleteSpecial", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpecialID", id);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void AddSpecial(SalesSpecial special)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("AddSpecial", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter param = new SqlParameter("@SpecialID", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(param);

                cmd.Parameters.AddWithValue("@SpecialTitle", special.SpecialTitle);
                cmd.Parameters.AddWithValue("@SpecialDescription", special.SpecialDescription);
                cn.Open();
                cmd.ExecuteNonQuery();
                special.SpecialID = (int)param.Value;
            }
        }

        public IEnumerable<VehicleDetailsItem> Search(InventorySearchParameters parameters)
        {
            List<VehicleDetailsItem> results = new List<VehicleDetailsItem>();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                string query = "SELECT TOP 20 ID,Make,Model,[Year],BodyStyle,Transmission,Color,Interior,Mileage,VIN#,SalePrice,MSRP,ImageFileName,[Description]" +
                    " FROM Inventory i INNER JOIN Makes ma ON i.MakeID = ma.MakeID" +
                    " INNER JOIN Models mo ON i.ModelID = mo.ModelID" +
                    " INNER JOIN BodyStyles b ON i.BodyStyleID = b.BodyStyleID" +
                    " INNER JOIN Transmissions t ON i.TransmissionID = t.TransmissionID" +
                    " INNER JOIN Colors c ON i.ColorID = c.ColorID"+
                    " INNER JOIN Interiors ir ON i.InteriorID = ir.InteriorID" +
                    " WHERE i.PurchaseID IS NULL ";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;

                if(!string.IsNullOrEmpty(parameters.QuickSearch))
                {
                    query += $"AND (Make LIKE '%' + @QuickSearch + '%' OR Model LIKE '%' + @QuickSearch + '%' OR [Year] LIKE @QuickSearch + '%') ";
                    cmd.Parameters.AddWithValue("@QuickSearch", parameters.QuickSearch);
                }

                if (parameters.MinPrice.HasValue)
                {
                    query += $"AND SalePrice >= @MinPrice ";
                    cmd.Parameters.AddWithValue("@MinPrice", parameters.MinPrice.Value);
                }

                if (parameters.MaxPrice.HasValue)
                {
                    query += $"AND SalePrice <= @MaxPrice ";
                    cmd.Parameters.AddWithValue("@MaxPrice", parameters.MaxPrice.Value);
                }

                if (parameters.MinYear.HasValue)
                {
                    query += $"AND [YEAR] >= @MinYear ";
                    cmd.Parameters.AddWithValue("@MinYear", parameters.MinYear.Value);
                }

                if (parameters.MaxYear.HasValue)
                {
                    query += $"AND [Year] <= @MaxYear ";
                    cmd.Parameters.AddWithValue("@MaxYear", parameters.MaxYear.Value);
                }
                if (parameters.NewOrUsed.HasValue)
                {
                    query += $"AND TypeID = @NewOrUsed ";
                    cmd.Parameters.AddWithValue("@NewOrUsed", parameters.NewOrUsed.Value);
                } 
                query += "ORDER BY MSRP DESC";

                cmd.CommandText = query;
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        VehicleDetailsItem vehicle = new VehicleDetailsItem();

                        vehicle.ID = (int)dr["ID"];
                        vehicle.Make = dr["Make"].ToString();
                        vehicle.Model = dr["Model"].ToString();
                        vehicle.Year = (int)dr["Year"];
                        vehicle.BodyStyle = dr["BodyStyle"].ToString();
                        vehicle.Transmission = dr["Transmission"].ToString();
                        vehicle.Color = dr["Color"].ToString();
                        vehicle.Interior = dr["Interior"].ToString();
                        vehicle.Mileage = (int)dr["Mileage"];
                        vehicle.VIN = dr["VIN#"].ToString();
                        vehicle.SalePrice = (decimal)dr["SalePrice"];
                        vehicle.MSRP = (decimal)dr["MSRP"];
                        vehicle.Description = dr["Description"].ToString();

                        if (dr["ImageFileName"] != DBNull.Value)
                            vehicle.ImageFileName = dr["ImageFileName"].ToString();

                        results.Add(vehicle);

                    }
                }
            }

            return results;
        }
    }
}
