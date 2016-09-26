using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserStore.DAL.Entities
{
    public abstract class AbstractContent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
       // [Required]
        public string Name { get; set; }
        //[Required]
        public string Path { get; set; }
        public int VoteUp { get; set; }
        public int VoteDown { get; set; }
        //[Required]
        //[Key, ForeignKey("Images")]
        //public int? imageId { get; set; }
        //[Required]
        public virtual IList<Image> Images { get; set; }

        //[ForeignKey("Genres")]
        //public int? genreId { get; set; }
        
        public virtual IList<Genre> Genres { get; set; }
        //[ForeignKey("Directors")]
        //public int? directorId { get; set; }
       
        public virtual IList<Director> Directors { get; set; }
        //[ForeignKey("Writers")]
        //public int? scenaristId { get; set; }
        
        public virtual IList<Scenarist> Writers { get; set; }

        public virtual string Year { get; set; }
        //[ForeignKey("Languages")]
        //public int? languageId { get; set; }
        //[Required]
        public virtual Language Language { get; set; }
        //[ForeignKey("Translators")]
        //public int? translatorId { get; set; }
        //[Required]
        public virtual Translator Transletor { get; set; }
        //[ForeignKey("Checks")]
        //public int? checkId { get; set; }
        //[Required]
        public virtual Check Check { get; set; } 

    }
}
