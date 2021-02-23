using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Models
{
    public class FileDataModel
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
