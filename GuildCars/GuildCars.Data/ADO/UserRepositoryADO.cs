using GuildCars.Data.Interfaces;
using GuildCars.Models.Tables;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GuildCars.Data.ADO
{
    public class UserRepositoryADO : IUserRepository
    {
        public EditUserDetails GetByID(string id)
        {
            EditUserDetails details = new EditUserDetails();
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetUserByID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        details.FirstName = dr["FirstName"].ToString();
                        details.LastName = dr["LastName"].ToString();
                        details.Email = dr["Email"].ToString();
                        details.Role = dr["Role"].ToString();
                    }
                }
            }

            return details;
        }

        public IEnumerable<GetUserDetails> GetAll()
        {
            List<GetUserDetails> users = new List<GetUserDetails>();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetUsers", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var user = new GetUserDetails();

                        user.Id = dr["Id"].ToString();
                        user.LastName = dr["LastName"].ToString();
                        user.FirstName = dr["FirstName"].ToString();
                        user.FullName = dr["FullName"].ToString();
                        user.Email = dr["Email"].ToString();
                        user.Role = dr["Role"].ToString();

                        users.Add(user);
                    }
                }
            }
            return users;
        }

        public void EditUser(User user, EditUserDetails details)
        {

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("EditUser", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", user.Id);
                cmd.Parameters.AddWithValue("@FullName", user.FullName);
                cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                cmd.Parameters.AddWithValue("@LastName", user.LastName);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@UserName", user.UserName);

                cn.Open();
                cmd.ExecuteNonQuery();

                cn.Close();

                SqlCommand cmd2 = new SqlCommand("GetRoleID", cn);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@Role", details.Role);

                cn.Open();

                using (SqlDataReader dr2 = cmd2.ExecuteReader())
                {
                    if (dr2.Read())
                    {
                        details.RoleID = dr2["Id"].ToString();
                    }
                }

                cn.Close();

                SqlCommand cmd3 = new SqlCommand("UpdateUserRole", cn);
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.Parameters.AddWithValue("@UserID", user.Id);
                cmd3.Parameters.AddWithValue("@RoleID", details.RoleID);

                cn.Open();
                cmd3.ExecuteNonQuery();
            }
        }
    }
}
