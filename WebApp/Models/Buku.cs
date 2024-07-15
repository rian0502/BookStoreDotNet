using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Buku
    {
        public Guid id { get; set; }
        public string NamaBuku { get; set; }
        public int Halaman { get; set; }
        public string Sinopsis { get; set; }
        public string Sampul { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public Guid Id_kategori { get; set; }
        public Guid Id_user { get; set; }
        public virtual KategoriBuku KategoriBuku { get; set; }
        public virtual Users Users { get; set; }
    }
}