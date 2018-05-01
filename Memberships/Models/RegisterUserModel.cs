using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Memberships.Models
{
    public class RegisterUserModel
    {
        [Required]
        [EmailAddress]
        [Display(Name ="Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(30, ErrorMessage ="The {0} must be atleast {1} char long.", MinimumLength =2)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]        
        public string Password { get; set; }

        [Required]
        public bool AcceptUSerAgreement { get; set; }

    }
}