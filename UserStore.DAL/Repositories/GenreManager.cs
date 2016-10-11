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
    class GenreManager : IManager<Genre>
    {
        public ApplicationContext DataBase { get; set; }
        public GenreManager(ApplicationContext db)
        {
            DataBase = db;
        }
        public async Task<int> Count()
        {
            return await DataBase.Genres.CountAsync();
        }

        public IQueryable<Genre> GetAll()
        {
            return  DataBase.Genres;
        }

        public async Task<Genre> Get(string name)
        {
            return await DataBase.Genres.Where(x => x.Name == name).FirstOrDefaultAsync();
        }

        public async Task<Genre> Get(int _id)
        {
            return await DataBase.Genres.Where(x => x.Id == _id).FirstOrDefaultAsync();
        }

        public  IQueryable<Genre> Query(Expression<Func<Genre, bool>> newFunc)
        {
            Genre genre = DataBase.Genres.Include(x => x.Contents).Where(newFunc).FirstOrDefault();
            return DataBase.Genres.Include(x => x.Contents).Where(newFunc);
            
            
        }

        public async Task<int> SetNew(string name)
        {
            DataBase.Genres.Add(new Genre { Name = name });
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
        public async Task<Genre> FindAsync(Genre name)
        {
            return await DataBase.Genres.FindAsync(name);
        }
    }
}
