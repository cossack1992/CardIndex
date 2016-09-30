using UserStore.BLL.DTO;
using UserStore.BLL.Infrastructure;
using UserStore.DAL.Entities;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using UserStore.BLL.Interfaces;
using UserStore.DAL.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Data.Entity;
using System.IO;

namespace UserStore.BLL.Services
{
    public class UserService : IUserService
    {
        
        IUserUnitOfWork DataBase { get; set; }
        public UserService(IUserUnitOfWork uow)
        {
            DataBase = uow;
        }
        public  async Task<OperationDetails> CreateUser (UserDTO userDTO)
        {
            if(userDTO != null)
            {

            
            try
            {

                ApplicationUser user = await DataBase.UserManager.FindByEmailAsync(userDTO.Email);
                if (user == null)
                {
                    user = new ApplicationUser { Email = userDTO.Email, UserName = userDTO.Email };
                    var result = await DataBase.UserManager.CreateAsync(user, userDTO.Password);
                    if (result.Errors.Count() > 0) return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                    await DataBase.UserManager.AddToRoleAsync(user.Id, userDTO.Role);
                    ClientProfile clientProfile = new ClientProfile { Id = user.Id, Address = userDTO.Address, Name = userDTO.Name };
                    DataBase.ClientManager.CreateProfile(clientProfile);
                    await DataBase.SaveAsync();
                    return new OperationDetails(true, "registration succedeed ", "");
                }
                    else
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw new DataAccessException();
                }
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
        public async Task<ClaimsIdentity> Autenticate(UserDTO userDTO)
        {
            if (userDTO != null)
            {


                ClaimsIdentity claim = null;
                try
                {
                    ApplicationUser user = await DataBase.UserManager.FindAsync(userDTO.Email, userDTO.Password);
                    if (user != null)
                    {
                        claim = await DataBase.UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    }
                    return claim;
                }
                catch (Exception)
                {
                    throw new DataAccessException();
                }
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
        public async Task SetInitialUserData(UserDTO adminDTO, List<string> roles)
        {
            try
            {
                foreach (string roleName in roles)
                {
                    var role = await DataBase.RoleManager.FindByNameAsync(roleName);
                    if (role == null)
                    {
                        role = new ApplicationRole { Name = roleName };
                        await DataBase.RoleManager.CreateAsync(role);
                    }
                }
                await CreateUser(adminDTO);
            }
            catch (Exception)
            {
                throw new DataAccessException();
            }
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
