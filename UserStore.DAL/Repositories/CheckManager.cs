using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.DAL.Entities;
using UserStore.DAL.Interfaces;
using UserStore.DAL.EF;

namespace UserStore.DAL.Repositories
{
    class CheckManager : ICheckManager
    {
        public ApplicationContext DataBase { get; set; }
        public CheckManager(ApplicationContext db)
        {
            DataBase = db;
        }
        public Task<Check> GetCheck(int _id)
        {
            
            Task<Check> newTask = Task<Check>.Factory.StartNew(() =>
            {
                Check result = DataBase.Checks.Where(x=>x.Id==_id).FirstOrDefault();
                return result;
            }
            );
            return newTask;
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
