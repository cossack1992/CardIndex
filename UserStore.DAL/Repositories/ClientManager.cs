using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.DAL.EF;
using UserStore.DAL.Entities;
using UserStore.DAL.Interfaces;

namespace UserStore.DAL.Repositories
{
    class ClientManager : IClientManager
    {
        public ApplicationContext DataBase { get; set; }
        public ClientManager(ApplicationContext db)
        {
            DataBase = db;
        }
        public void CreateProfile(ClientProfile item)
        {
            DataBase.ClientProfiles.Add(item);
            DataBase.SaveChanges();
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
