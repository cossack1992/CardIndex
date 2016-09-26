using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace UserStore.WEB.Models
{
    public class UpdateContentModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name is required.")]
        public string Name { get; set; }
        public string operation { get; set; }

        [Required(ErrorMessage = "The Path is required.")]
        public HttpPostedFileBase Path { get; set; }
        [Required(ErrorMessage = "The Landuage is required.")]
        public string Language { get; set; }
        [Required(ErrorMessage = "The Traslator is required.")]
        public string Transletor { get; set; }
        public string Check { get; set; }
        [Required(ErrorMessage = "The Year is required.")]
        [DisplayFormat(DataFormatString = "{0:yyyy}")]
        public string Year { get; set; }
        [Required(ErrorMessage = "The Genres are required.")]
        public string Genres { get; set; }
        [Required(ErrorMessage = "The Image is required.")]
        public HttpPostedFileBase Image { get; set; }
        [Required(ErrorMessage = "The Directors are required.")]
        public string Directors { get; set; }
        [Required(ErrorMessage = "The Writers are required.")]
        public string Writers { get; set; }
    }
}