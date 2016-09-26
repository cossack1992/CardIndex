using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace UserStore.WEB.Models
{
    public class ContentModelOutPut
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string Type { get; set; }
        public string Path { get; set; }
        public int VoteUp { get; set; }
        public int VoteDown { get; set; }
        public string Language { get; set; }
        
        public string Transletor { get; set; }
        public string Check { get; set; }
        
        public string Year { get; set; }
        
        public IList<string> Genres { get; set; }
        
        public IList<string> Directors { get; set; }
        
        public IList<string> Writers { get; set; }
        
        public IList<ImageModel> Images { get; set; }
        
        public ContentModelOutPut()
        {
            Id = 0;
            Genres = new List<string>();
            Path = "";
            Directors = new List<string>();
            Writers = new List<string>();
            Images = new List<ImageModel>();
            Name = "";
            Language = "";
            Transletor = "";
            Year = DateTime.MinValue.ToString() ;
        }
    }
}