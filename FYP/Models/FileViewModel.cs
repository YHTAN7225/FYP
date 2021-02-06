﻿using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.File;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models
{
    public class FileViewModel
    {
        [Required]
        public List<IFormFile> Files { get; set; }


    }
}
