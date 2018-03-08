﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Memberships.Models
{
    public class UserViewModel
    {
        [Display(Name ="User Id")]
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name ="Email")]
        public string Email { get; set; }

        [Display(Name ="First Name")]
        [StringLength(30, ErrorMessage="The {0} must be atleast {1} long"), MinLength(2)]
        public string FirstName { get; set; }

        [Display(Name ="Password")]
        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}