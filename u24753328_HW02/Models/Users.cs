using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace u24753328_HW02.Models
{
    public class Users : Controller
    {
            public int UserID { get; set; }
            public string FullName { get; set; }
            public string PhoneNumber { get; set; } 
        }
    }