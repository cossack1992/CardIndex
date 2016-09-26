using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStore.BLL.Interfaces
{
    public interface IServiceCreator
    {
        /// <summary>
        /// Create new Service
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        IUserService CreateService(string connection);
    }
}
