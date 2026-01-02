using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace u24753328_HW02.Models
{
    public class Donations : Controller
    {
            public int DonationID { get; set; }
            public string DonorName { get; set; }
            public string PhoneNumber { get; set; }
            public decimal Amount { get; set; }
        }
    }
