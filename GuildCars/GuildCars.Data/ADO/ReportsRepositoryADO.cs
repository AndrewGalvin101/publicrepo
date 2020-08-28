using GuildCars.Data.Interfaces;
using GuildCars.Models.Queries;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GuildCars.Data.ADO
{
    class ReportsRepositoryADO : IReportsRepository
    {
        public IEnumerable<InventoryReport> GetNew()
        {
            List<InventoryReport> newInventory = new List<InventoryReport>();
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("NewInventoryReport", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = cn;

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        InventoryReport row = new InventoryReport();

                        row.Year = (int)dr["Year"];
                        row.Make = dr["Make"].ToString();
                        row.Model = dr["Model"].ToString();
                        row.Count = (int)dr["Count"];
                        row.StockValue = (decimal)dr["StockValue"];

                        newInventory.Add(row);
                    }
                }
            }

            return newInventory;
        }

        public IEnumerable<InventoryReport> GetUsed()
        {
            List<InventoryReport> usedInventory = new List<InventoryReport>();
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("UsedInventoryReport", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = cn;

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        InventoryReport row = new InventoryReport();

                        row.Year = (int)dr["Year"];
                        row.Make = dr["Make"].ToString();
                        row.Model = dr["Model"].ToString();
                        row.Count = (int)dr["Count"];
                        row.StockValue = (decimal)dr["StockValue"];

                        usedInventory.Add(row);
                    }
                }
            }

            return usedInventory;
        }

        public IEnumerable<SalesReport> Search(SalesReportParameters parameters)
        {
            List<SalesReport> salesReport = new List<SalesReport>();
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                string query = "SELECT FullName, SUM(PurchasePrice) As TotalSales, COUNT(PurchaseID) AS TotalVehicles " +
                               "FROM AspNetUsers u INNER JOIN Purchases p ON u.Id = p.UserID WHERE 1=1 ";

                SqlCommand cmd = new SqlCommand();

                if (!string.IsNullOrEmpty(parameters.User))
                {
                    query += "AND FullName = @FullName ";
                    cmd.Parameters.AddWithValue("@FullName", parameters.User);
                }
                
                if (!string.IsNullOrEmpty(parameters.FromDate))
                {
                    query += "AND CreateDate >= @FromDate ";
                    var date = DateTime.Parse(parameters.FromDate).ToShortDateString();
                    cmd.Parameters.AddWithValue("@FromDate", date);
                }


                if (!string.IsNullOrEmpty(parameters.ToDate))
                {
                    query += "AND CreateDate <= @ToDate ";
                    var date = DateTime.Parse(parameters.ToDate).ToShortDateString();
                    cmd.Parameters.AddWithValue("@ToDate", date);
                }

                query += "GROUP BY FullName";

                cmd.Connection = cn;

                cmd.CommandText = query;
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        SalesReport row = new SalesReport();

                        row.User = dr["FullName"].ToString();
                        row.TotalSales = (decimal)dr["TotalSales"];
                        row.TotalVehicles = (int)dr["TotalVehicles"];

                        salesReport.Add(row);
                    }
                }
            }

        return salesReport;
        }
    }
}
