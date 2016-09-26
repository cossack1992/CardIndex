using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UserStore.WEB.Models;

namespace UserStore.WEB.Helpers
{
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;

    //.............................
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this AjaxHelper html,
            AjaxOptions ajaxOptions,
            PageModel pageModel, Func<int, string> pageUrl )
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= pageModel.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();
                // если текущая страница, то выделяем ее,
                // например, добавляя класс
                if (i == pageModel.PageNumber)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                tag.AddCssClass("btn btn-default");
                result.Append(tag.ToString());
            }
            return MvcHtmlString.Create(result.ToString());
            
        }
    }
}