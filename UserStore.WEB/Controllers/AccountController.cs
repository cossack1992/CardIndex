using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using UserStore.WEB.Models;
using UserStore.BLL.DTO;
using System.Security.Claims;
using UserStore.BLL.Interfaces;
using UserStore.BLL.Infrastructure;


namespace UserStore.WEB.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }


        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        public ActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError()]
        public async Task<ActionResult> Login(LoginModel model)
        {
            await SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                UserDTO userDTO = new UserDTO { Email = model.Email.ToString().ToLower(), Password = model.Password.ToString().ToLower() };
                ClaimsIdentity claim = await UserService.Autenticate(userDTO);
                if(claim == null)
                {
                    ModelState.AddModelError("", "Password is not valid");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = false }, claim);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }
        [HandleError()]
        public ActionResult LogOut()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError()]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            
            await SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO
                {
                    Email = model.Email.ToString().ToLower(),
                    Password = model.Password.ToString().ToLower(),
                    Address = model.Address,
                    Name = model.Name,
                    Role = "user"
                };
                OperationDetails operationDetails = await UserService.CreateUser(userDto);
                if (operationDetails.Succedeed)
                    return Redirect("/Home/Index");
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(model);
        }
        private async Task SetInitialDataAsync()
        {
            
            await UserService.SetInitialUserData(new UserDTO
            {
                Email = "cozachello@gmail.com",
                UserName = "cozachello@gmail.com",
                Password = "123456789",
                Name = "Ya Ya Ya",
                Address = "des'",
                Role = "admin",
            }, new List<string> { "user", "admin" });
            
        }
    }
}