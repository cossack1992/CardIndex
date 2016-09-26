using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using UserStore.BLL.DTO;
using UserStore.BLL.Infrastructure;

namespace UserStore.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        /// <summary>
        /// Create and save new User
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        Task<OperationDetails> CreateUser(UserDTO userDTO);
        /// <summary>
        /// Autenticate user
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        Task<ClaimsIdentity> Autenticate(UserDTO userDTO);
        Task SetInitialUserData(UserDTO adminDTO, List<string> roles);
    }
}
