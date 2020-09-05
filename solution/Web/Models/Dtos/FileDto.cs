using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace WebUI.Models.Dtos
{
    public class FileDto
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile FormFile { get; set; }

        [Display(Name = "Desciption")]
        [StringLength(100, MinimumLength = 0)]
        public string Desciption { get; set; }
    }
}
