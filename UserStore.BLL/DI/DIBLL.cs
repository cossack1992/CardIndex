using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using UserStore.DAL.Interfaces;
using UserStore.DAL.Repositories;

namespace UserStore.BLL.DI
{
    public class DIBLL : Ninject.Modules.NinjectModule
    {
        string con;
        public DIBLL(string con)
        {
            this.con = con;
        }
        public override void Load()
        {
            Kernel.Bind<IContentUnitOfWork>().To<ContentUnitOfWork>().WithConstructorArgument("connectionString", con);
        }
    }
}
