using Surveyer.HelperClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Surveyer.Models
{
    public class User
    {
        //test
        [ScaffoldColumn(false)]
        public string Id { get; set; }

        [Display(Name ="User name")]
        [Required]
        public string UserName { get; set; }

        [Display(Name = "Birth date")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        [Required]
        [Phone]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "User image")]
        public string ImageURL { get; set; }

        public User()
        {
            this.Id = Guid.NewGuid().ToString();
        }

    }
}