using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using UserStore.BLL.Interfaces;
using UserStore.BLL.Services;

namespace UserStore.WEB.DI
{
    public class DIWEB : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IContentService>().To<ContentService>();
        }
    }
}