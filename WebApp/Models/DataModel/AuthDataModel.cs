using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApp.Models.DataModel
{
    public class AuthDataModel
    {
        string connectionString = ConfigurationManager.ConnectionStrings["koneksi"].ConnectionString;
        public void ChangePassword(Guid id, string password, string salt)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand("ChangePassword", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@Salt", salt);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void RegisterUser(string name, string email, string password, string salt)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand("RegisterUsers", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Guid.NewGuid());
                    cmd.Parameters.AddWithValue("@Nama", name);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@PasswordHash", password);
                    cmd.Parameters.AddWithValue("@Salt", salt);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public Users LoginUsers(string email)
        {
            Users user = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand("GetUsers", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", email);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new Users(
                                id: reader.GetGuid(reader.GetOrdinal("Id")),
                                name: reader.GetString(reader.GetOrdinal("Name")),
                                email: reader.GetString(reader.GetOrdinal("Email")),
                                password: reader.GetString(reader.GetOrdinal("Password")),
                                salt: reader.GetString(reader.GetOrdinal("Salt"))
                            );
                        }
                    }


                }
            }
            return user;
        }

        public List<Users> AllUser()
        {
            List<Users> users = new List<Users>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand("AllUser", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new Users(
                                id: reader.GetGuid(reader.GetOrdinal("id")),
                                name: reader.GetString(reader.GetOrdinal("Name")),
                                email: reader.GetString(reader.GetOrdinal("Email")),
                                password: reader.GetString(reader.GetOrdinal("Password")),
                                salt: reader.GetString(reader.GetOrdinal("Salt"))
                            ));
                        }
                    }
                }
            }
            return users;
        }
    }
}