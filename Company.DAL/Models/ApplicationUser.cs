﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FName { get; set; }
        [Required]
        public string LName { get; set; }
        [Required]
        public bool IsAgree { get; set; }

    }
}
