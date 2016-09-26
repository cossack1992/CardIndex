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
    class TransletorManager : IManager<Translator>
    {
        public ApplicationContext DataBase { get; set; }
        public TransletorManager(ApplicationContext db)
        {
            DataBase = db;
        }
        public async Task<int> Count()
        {
            return await DataBase.Transletors.CountAsync();
        }

        public IQueryable<Translator> GetAll()
        {
            return  DataBase.Transletors;
        }

        public async Task<Translator> Get(string name)
        {
            return await DataBase.Transletors.Where(x => x.Name == name).FirstOrDefaultAsync();
        }

        public async Task<Translator> Get(int _id)
        {
            return await DataBase.Transletors.Where(x => x.Id == _id).FirstOrDefaultAsync();
        }

        public IQueryable<Translator> Quary(Expression<Func<Translator, bool>> newFunc)
        {
            return  DataBase.Transletors.Where(newFunc);
        }

        public async Task<int> SetNew(string name)
        {
            DataBase.Transletors.Add(new Translator { Name = name});
            return await DataBase.SaveChangesAsync();
        }
        public async Task<Translator> FindAsync(Translator name)
        {
            return await DataBase.Transletors.FindAsync(name);
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
