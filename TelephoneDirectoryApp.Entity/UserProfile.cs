using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TelephoneDirectoryApp.Entity
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Username")]
        public string Username { get; set; }

        [Required]
        [DisplayName("Password")]
        public string Password { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        public DateTime CreateDt { get; set; }
        public string CreateBy { get; set; }
        public DateTime UpdateDt { get; set; }
        public string UpdateBy { get; set; }
    }
}
