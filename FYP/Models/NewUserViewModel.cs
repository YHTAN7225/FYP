using System.ComponentModel.DataAnnotations;

namespace FYP.Models
{
    public class NewUserViewModel
    {
        [Required]
        public string Email { get; set; }

        public string Admin { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
