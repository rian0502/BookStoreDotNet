using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApp.Models.DataModel
{
    public class BukuDataModel
    {
        string connectionString = ConfigurationManager.ConnectionStrings["koneksi"].ConnectionString;

        public bool InsertBuku(Buku buku)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("CreateBuku", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NamaBuku", buku.NamaBuku);
                    cmd.Parameters.AddWithValue("@Halaman", buku.Halaman);
                    cmd.Parameters.AddWithValue("@Sinopsis", buku.Sinopsis);
                    cmd.Parameters.AddWithValue("@Sampul", buku.Sampul);
                    cmd.Parameters.AddWithValue("@Id_kategori", buku.Id_kategori);
                    cmd.Parameters.AddWithValue("@Id_user", buku.Id_user);
                    var result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }
        public Buku getBukuById(Guid id)
        {
            Buku buku = new Buku();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("GetBukuById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            buku.id = reader.GetGuid(reader.GetOrdinal("Id"));
                            buku.NamaBuku = reader.GetString(reader.GetOrdinal("NamaBuku"));
                            buku.Halaman = reader.GetInt32(reader.GetOrdinal("Halaman"));
                            buku.Sinopsis = reader.GetString(reader.GetOrdinal("Sinopsis"));
                            buku.Sampul = reader.GetString(reader.GetOrdinal("Sampul"));
                            buku.Id_kategori = reader.GetGuid(reader.GetOrdinal("Id_kategori"));
                            buku.Id_user = reader.GetGuid(reader.GetOrdinal("Id_user"));
                        }
                    }
                }
            }
            return buku;
        }

        public List<Buku> FindAll(Guid id)
        {
            List<Buku> data = new List<Buku>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("GetBukuWithKategori", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_user", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Buku buku = new Buku
                            {
                                id = reader.GetGuid(reader.GetOrdinal("Id")),
                                NamaBuku = reader.GetString(reader.GetOrdinal("NamaBuku")),
                                Halaman = reader.GetInt32(reader.GetOrdinal("Halaman")),
                                Sinopsis = reader.GetString(reader.GetOrdinal("Sinopsis")),
                                Sampul = reader.GetString(reader.GetOrdinal("Sampul")),
                                created_at = (DateTime)reader["created_at"],
                                updated_at = (DateTime)reader["updated_at"],
                                Id_kategori = reader.GetGuid(reader.GetOrdinal("Id_kategori")),
                                Id_user = reader.GetGuid(reader.GetOrdinal("Id_user")),

                            };
                            buku.KategoriBuku = new KategoriBuku
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("ID_KATEGORI")),
                                NamaKategori = reader["NamaKategori"].ToString()
                            };
                            data.Add(buku);
                        }
                    }
                }
                return data;
            }

        }
        public bool UpdateBuku(Buku buku)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("UpdateBukuById", conn))
                {
                    Buku test = buku;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", buku.id);
                    cmd.Parameters.AddWithValue("@NamaBuku", buku.NamaBuku);
                    cmd.Parameters.AddWithValue("@Halaman", buku.Halaman);
                    cmd.Parameters.AddWithValue("@Sinopsis", buku.Sinopsis);
                    cmd.Parameters.AddWithValue("@Sampul", buku.Sampul);
                    cmd.Parameters.AddWithValue("@Id_kategori", buku.Id_kategori);
                    cmd.Parameters.AddWithValue("@Id_user", buku.Id_user);
                    cmd.Parameters.AddWithValue("@updated_at", DateTime.Now);
                    var result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }
        public bool DeleteBuku(Guid id)
        {
            bool status = false;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("DeleteBukuById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    var result = cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            return status;
        }
    }
}