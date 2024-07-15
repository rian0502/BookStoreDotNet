using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Nama harus diisi!")]
        [StringLength(100, ErrorMessage = "Nama maksimal 100 karakter")]
        [Display(Name = "Nama Lengkap")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email harus diisi!")]
        [EmailAddress(ErrorMessage = "Format email tidak valid")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password harus diisi!")]
        [StringLength(100, ErrorMessage = "Password minimal {2} karakter dan maksimal {1} karakter", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Konfirmasi Password")]
        [Compare("Password", ErrorMessage = "Password dan konfirmasi password tidak cocok")]
        public string KonfirmasiPassword { get; set; }
    }
}