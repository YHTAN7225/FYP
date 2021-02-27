using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.File;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models
{
    public class LinkFileViewModel
    {
        [Required]
        public IFormFile Files { get; set; }
    }
}
