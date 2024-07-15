using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApp.Models.DataModel
{
    public class KategoriBukuDataModel
    {
        string connectionString = ConfigurationManager.ConnectionStrings["koneksi"].ConnectionString;
        public List<KategoriBuku> AllKategori()
        {
            List<KategoriBuku> data = new List<KategoriBuku>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("findAllKategoriBuku", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        KategoriBuku kategori = new KategoriBuku
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            NamaKategori = reader.GetString(reader.GetOrdinal("NamaKategori")),
                            JumlahBuku = reader.GetInt32(reader.GetOrdinal("JumlahBuku"))
                        };
                        data.Add(kategori);
                    }
                }
                return data;
            }

        }

        public bool InsertKategori(string namaKategori)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("CreateKategoriBuku", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Guid.NewGuid());
                    cmd.Parameters.AddWithValue("@NamaKategori", namaKategori);
                    var result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }

        public KategoriBuku GetKategoriById(Guid id)
        {
            KategoriBuku kb = new KategoriBuku();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("GetKategoriById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        kb.Id = reader.GetGuid(reader.GetOrdinal("Id"));
                        kb.NamaKategori = reader.GetString(reader.GetOrdinal("NamaKategori"));
                    }
                }
            }
            return kb;

        }
        public bool UpdateKategori(Guid id, string namaKategori)
        {
            bool status = false;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("UpdateKategoriById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@NamaKategori", namaKategori);
                    var result = cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            return status;
        }
        public bool DeleteKategori(Guid id)
        {
            bool status = false;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("DeleteKategoriBuku", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    var result = cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            return status;
        }

        public List<CategoryUsage> PenggunaanKategori()
        {
            List<CategoryUsage> cu = new List<CategoryUsage>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("GetPenggunaanKategori", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        cu.Add(
                            new CategoryUsage {
                                Kategori = reader.GetString(reader.GetOrdinal("Kategori")),
                                Total = reader.GetInt32(reader.GetOrdinal("Total"))
                            }
                        );
                       
                    }
                }
            }

            return cu;
        }
    }
}