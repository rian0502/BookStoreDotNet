using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class KategoriBuku
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Nama Kategori Harus diisi !")]
        public string NamaKategori { get; set; }

        public int JumlahBuku { get; set; }
    }
}