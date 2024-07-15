using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApp.Models.DataModel
{
    public class ProductDataModel
    {
        string connectionString = ConfigurationManager.ConnectionStrings["koneksi"].ConnectionString;
        public List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Id, Name, Price FROM Products";
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Product product = new Product
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("Id")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        Price = reader.GetDecimal(reader.GetOrdinal("Price"))
                    };
                    products.Add(product);

                }
                reader.Close();
            }
            return products;
        }
        public void InsertProduct(string name, decimal price)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Products (Id, Name, Price) VALUES (@Id, @Name, @Price)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", Guid.NewGuid());
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Price", price);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public Product getProductById(Guid id)
        {
            Product product = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Id, Name, Price FROM Products WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    product = new Product
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("Id")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        Price = reader.GetDecimal(reader.GetOrdinal("Price"))
                    };
                }
                reader.Close();
            }
            return product;
        }
        public void UpdateProduct(Guid id, string name, decimal price)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Products SET Name = @Name, Price = @Price WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Price", price);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void DeleteProduct(Guid id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE Products WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}