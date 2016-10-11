using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UserStore.DAL.Interfaces
{
    public interface IManager<T> : IDisposable
    {
        /// <summary>
        /// Get all values from DataBase
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAll();
        /// <summary>
        /// Get value with id
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        Task<T> Get(int _id);
        /// <summary>
        /// Get value with name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<T> Get(string name);
        /// <summary>
        /// Save to DataBase new value with name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<int> SetNew(string name);
        /// <summary>
        /// Get count of values
        /// </summary>
        /// <returns></returns>
        Task<int> Count();
        /// <summary>
        /// Get all values for expression
        /// </summary>
        /// <param name="newFunc"></param>
        /// <returns></returns>
        IQueryable<T> Query(Expression<Func<T, bool>> newFunc);
        /// <summary>
        /// Check on exists value in DataBase
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<T> FindAsync(T name);
    }
}
