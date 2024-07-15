using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Buku
    {
        public Guid id { get; set; }
        [Required(ErrorMessage = "Nama Buku harus diisi!")]
        [Display(Name = "Nama Buku")]
        public string NamaBuku { get; set; }
        public int Halaman { get; set; }
        [Required(ErrorMessage = "Sinopsis harus diisi!")]
        [StringLength(maximumLength: 1000, MinimumLength = 5)]
        public string Sinopsis { get; set; }
        public string Sampul { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        [Required(ErrorMessage = "Kategori harus diisi!")]
        public Guid Id_kategori { get; set; }
        public Guid Id_user { get; set; }
        public virtual KategoriBuku KategoriBuku { get; set; }
        public virtual Users Users { get; set; }
    }
}