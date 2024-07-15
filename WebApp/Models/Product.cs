using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name must be less than 100 characters")]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }
    }
}