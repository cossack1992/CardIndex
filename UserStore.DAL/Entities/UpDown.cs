using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStore.DAL.Entities
{
    public class UpDown
    {
        public int Id { get; set; }
        public int Vote { get; set; }
        public IList<Vote> Votes { get; set; }
        public UpDown()
        {
            Votes = new List<Vote>();
        }
    }
}
