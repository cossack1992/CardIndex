using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.DAL.Entities;

namespace UserStore.DAL.Interfaces
{
    public interface ICheckManager : IDisposable
    {
        /// <summary>
        /// Get check value for id
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        Task<Check> GetCheck(int _id);
    }
}
