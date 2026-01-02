using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace u24753328_HW02.Models
{
    public class Pets : Controller
    {
            public int PetID { get; set; }
        [Required(ErrorMessage = "A pet name is required.")]
        [StringLength(50)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please select the pet's Type.")]
        public string Type { get; set; }
        [Required(ErrorMessage = "Please select the pet's Breed.")]
        public string Breed { get; set; }
        [Required(ErrorMessage = "Please select the pet's Location.")]
        public string Location { get; set; }
        [Required(ErrorMessage = "The pet's age is required.")]
        [Range(0, 30, ErrorMessage = "Age must be between 0 and 30.")]
        public int Age { get; set; }
        [Required(ErrorMessage = "The pet's weight is required.")]
        [Range(0.1, 100.0, ErrorMessage = "Weight must be greater than zero.")]
        public decimal Weight { get; set; }
        [Required(ErrorMessage = "Please select the pet's gender.")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "A pet image is required.")]
        public string ImagePath { get; set; }
        [Required(ErrorMessage = "A pet story is required.")]
        [DataType(DataType.MultilineText)]
        public string Story { get; set; }
            public string Status { get; set; }
            public string PostedBy { get; set; }
            
            public string PhoneNumber { get; set; }
        }
    }