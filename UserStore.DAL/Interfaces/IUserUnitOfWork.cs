using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.DAL.Entities;
using UserStore.DAL.Identity;


namespace UserStore.DAL.Interfaces
{
    public interface IUserUnitOfWork : IDisposable
    {
        ApplicationUserManager  UserManager { get; }
        IClientManager ClientManager { get; }
        ApplicationRoleManager RoleManager { get; }
        
        Task SaveAsync();
    }
}
