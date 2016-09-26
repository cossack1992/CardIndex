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
    class LanguageManager : IManager<Language>
    {
        public ApplicationContext DataBase { get; set; }
        public LanguageManager(ApplicationContext db)
        {
            DataBase = db;
        }
        public async Task<int> Count()
        {
            return await DataBase.Languages.CountAsync();
        }

        public IQueryable<Language> GetAll()
        {
            return DataBase.Languages;
        }

        public async Task<Language> Get(string name)
        {
            return await DataBase.Languages.Where(x => x.Name == name).FirstOrDefaultAsync();
        }

        public async Task<Language> Get(int _id)
        {
            return await DataBase.Languages.Where(x => x.Id == _id).FirstOrDefaultAsync();
        }

        public IQueryable<Language> Quary(Expression<Func<Language, bool>> newFunc)
        {
            return  DataBase.Languages.Where(newFunc);
            
        }

        public async Task<int> SetNew(string name)
        {
            DataBase.Languages.Add(new Language { Name = name });
            return await DataBase.SaveChangesAsync();
        }
        public async Task<Language> FindAsync(Language name)
        {
            return await DataBase.Languages.FindAsync(name);
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
