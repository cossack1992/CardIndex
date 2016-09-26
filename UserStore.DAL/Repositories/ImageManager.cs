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
    class ImageManager : IImageManager
    {
        public ApplicationContext DataBase { get; set; }
        public ImageManager(ApplicationContext db)
        {
            DataBase = db;
        }
        public async Task<int> Count()
        {
            return await DataBase.Images.CountAsync();
        }

        public IQueryable<Image> GetAll()
        {
            return DataBase.Images;
        }

        public async Task<Image> Get(string name)
        {
            return await DataBase.Images.Where(x => x.Name == name).FirstOrDefaultAsync();
        }

        public async Task<Image> Get(int _id)
        {
            return await DataBase.Images.Where(x => x.Id == _id).FirstOrDefaultAsync();
        }

        public IQueryable<Image> Quary(Expression<Func<Image, bool>> newFunc)
        {
            return  DataBase.Images.Where(newFunc);
        }

        public async Task<int> SetNew(string name, string path)
        {
            DataBase.Images.Add(new Image { Name = name , Path = path});
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
        public async Task<Image> FindAsync(Image name)
        {
            return await DataBase.Images.FindAsync(name);
        }
    }
}
