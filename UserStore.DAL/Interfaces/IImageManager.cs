using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserStore.DAL.Entities;

namespace UserStore.DAL.Interfaces
{
    public interface IImageManager : IDisposable
    {
        IQueryable<Image> GetAll();
        Task<Image> Get(int _id);
        Task<Image> Get(string name);
        Task<int> SetNew(string name, string path);
        Task<int> Count();

        IQueryable<Image> Quary(Expression<Func<Image, bool>> newFunc);
        Task<Image> FindAsync(Image name);
    }
}
