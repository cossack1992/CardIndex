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
    
    class DirectorManager : IManager<Director>
    {

        public ApplicationContext DataBase { get; set; }
        public DirectorManager(ApplicationContext db)
        {
            DataBase = db;
        }
        public async Task<int> Count()
        {
            return await DataBase.Directors.CountAsync();
        }

        public IQueryable<Director> GetAll()
        {
            return  DataBase.Directors;
        }

        public async Task<Director> Get(string name)
        {
            return await DataBase.Directors.Where(x => x.Name == name).FirstOrDefaultAsync();
        }

        public async Task<Director> Get(int _id)
        {
            return await DataBase.Directors.Where(x => x.Id == _id).FirstOrDefaultAsync();
        }

        

        public async Task<int> SetNew(string name)
        {
            DataBase.Directors.Add(new Director { Name = name });
            return await DataBase.SaveChangesAsync();
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

        public IQueryable<Director> Query(Expression<Func<Director, bool>> newFunc)
        {
            var loc = DataBase.Directors.Include(x => x.Contents).Where(newFunc);
            return loc;
            
        }
        public async Task<Director> FindAsync(Director name)
        {
            return await DataBase.Directors.FindAsync(name);
        }
    }
}
