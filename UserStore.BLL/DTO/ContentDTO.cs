using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStore.BLL.DTO
{
    public class ContentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Path { get; set; }
        public int VoteUp { get; set; }
        public int VoteDown { get; set; }
        public string Language { get; set; }
        public string Translator { get; set; }
        public string Check { get; set; }
        public string Year { get; set; }
        public IList<string> Genres { get; set; }
        public IList<string> Directors { get; set; }
        public IList<string> Writers { get; set; }
        public IList<ImageDTO> Images { get; set; }
        public ContentDTO()
        {
            Genres = new List<string>();
            Directors = new List<string>();
            Writers = new List<string>();
            Images = new List<ImageDTO>();
        }
    }
}
