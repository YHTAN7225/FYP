using System.ComponentModel.DataAnnotations;

namespace FYP.Models
{
    public class NewUserViewModel
    {
        [Required]
        public string email { get; set; }

        public string admin { get; set; }

        [Required]
        public string password { get; set; }
    }
}
