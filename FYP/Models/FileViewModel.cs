using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models
{
    public class FileViewModel
    {
        public string FileName { get; set; }

        public string LastModifiedDate { get; set; }

        public string FileSize { get; set; }

        public IFormFile File { get; set; }
    }
}
