using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.DAL.Entities;

namespace UserStore.DAL.Interfaces
{
    public interface IClientManager : IDisposable
    {
        /// <summary>
        /// Create and save to DataBase new Client Profile
        /// </summary>
        /// <param name="item"></param>
        void CreateProfile(ClientProfile item);
        
    }
}
