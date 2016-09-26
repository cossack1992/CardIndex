using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.DAL.Entities;

namespace UserStore.DAL.Entities
{
    public class Vote
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public AbstractContent Contents { get; set; }
        public UpDown UpDown {get;set;}
        
    }
}
