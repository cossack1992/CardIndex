using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.BLL.Interfaces;
using UserStore.DAL.Repositories;
using Ninject;

namespace UserStore.BLL.Services
{
    public class ServiceCreator : IServiceCreator
    {      
        public IUserService CreateService(string connection)
        {           
            return new UserService(new UserUnitOfWork(connection));
        }
    }
}
