using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace UserStore.WEB.Models
{
    public class PageModel
    {
    
        public int PageNumber { get; set; } // номер текущей страницы
        public int PageSize { get; set; } // кол-во объектов на странице
        public int TotalItems { get; set; } // всего объектов
        public int TotalPages  // всего страниц
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }
    }
    public class IndexViewModel
    {
        public IEnumerable<ContentModelOutPut> Contents { get; set; }
        public PageModel PageInfo { get; set; }
        public List<string> Genres { get; set; }
        public string Filter { get; set; }
        public string Value { get; set; }
        public string Types { get; set; }
    }
}