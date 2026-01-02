using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace u24753328_HW02.Models
{
    public class Adoption_records
    {
      
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdoptionID { get; set; }
        [Required]
        public int PetID { get; set; }
        [Required]
        [StringLength(50)]
        public string PetName { get; set; }
        [Required]
        [StringLength(100)]
        public string UserName { get; set; }
        [Required]
        public DateTime AdoptionDate { get; set; }
    }
    }