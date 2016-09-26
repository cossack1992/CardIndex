using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.DAL.Entities;
using UserStore.DAL.Interfaces;
using UserStore.DAL.EF;
using System.Linq.Expressions;
using System.Data.Entity;

namespace UserStore.DAL.Repositories
{
    class ScenaristManager : IManager<Scenarist>
    {
        public ApplicationContext DataBase { get; set; }
        public ScenaristManager(ApplicationContext db)
        {
            DataBase = db;
        }
        public async Task<int> Count()
        {
            return await DataBase.Writers.CountAsync();
        }

        public IQueryable<Scenarist> GetAll()
        {
            return  DataBase.Writers;
        }

        public async Task<Scenarist> Get(string name)
        {
            return await DataBase.Writers.Where(x => x.Name == name).FirstOrDefaultAsync();
        }

        public async Task<Scenarist> Get(int _id)
        {
            return await DataBase.Writers.Where(x => x.Id == _id).FirstOrDefaultAsync();
        }

        public IQueryable<Scenarist> Quary(Expression<Func<Scenarist, bool>> newFunc)
        {
            return  DataBase.Writers.Include(x => x.Contents).Where(newFunc);
            
        }

        public async Task<int> SetNew(string name)
        {
            DataBase.Writers.Add(new Scenarist { Name = name });
            return await DataBase.SaveChangesAsync();
        }
        public async Task<Scenarist> FindAsync(Scenarist name)
        {
            return await DataBase.Writers.FindAsync(name);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    DataBase.Dispose();
                }
                this.disposed = true;
            }
        }
    }
}
