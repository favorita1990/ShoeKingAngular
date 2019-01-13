using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ShoeKingAPI.Context;
using ShoeKingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ShoeKingAPI.Controllers
{
    public class AccountController : ApiController
    {
        [Route("api/User/Register")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IdentityResult> Register(AccountModel model)
        {
            var context = new ContextDB();
            var userStore = new UserStore<User>(context);
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var manager = new UserManager<User>(userStore);

            var firstName =
              $"{model.FirstName.Trim().Substring(0, 1).ToUpper()}{model.FirstName.Trim().Substring(1)}";

            var lastName = $"{model.LastName.Trim().Substring(0, 1).ToUpper()}{model.LastName.Trim().Substring(1)}";

            var user = new User()
            {
                UserName = model.Email.Trim(),
                Email = model.Email.Trim(),
                FirstName = firstName,
                LastName = lastName,
                CreationDate = DateTime.Now,
                Gender = model.Gender
            };

            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 2
            };

            IdentityResult result = await manager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {

                if (!roleManager.RoleExists("User"))
                {
                    roleManager.Create(new IdentityRole { Name = "User" });
                }

                manager.AddToRole(user.Id, "User");
            }

            return result;
        }

        [HttpGet]
        [Route("api/GetUserClaims")]
        public AccountModel GetUserClaims()
         {
            var identityClaims = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;

            if (claims.Count() > 0)
            {
                var model = new AccountModel()
                {
                    UserName = identityClaims.FindFirst("UserName").Value,
                    Email = identityClaims.FindFirst("Email").Value,
                    Role = identityClaims.FindFirst("Role").Value,
                    FirstName = identityClaims.FindFirst("FirstName").Value,
                    LastName = identityClaims.FindFirst("LastName").Value,
                    LoggedOn = identityClaims.FindFirst("LoggedOn").Value
                };
                return model;
            }

            return null;
        }

        [HttpPost]
        [Route("api/CheckUserPassword")]
        public bool CheckUserPassword(string password)
        {
            var identityClaims = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;

            if (claims.Count() > 0)
            {
                var contextDb = new ContextDB();
                var userStore = new UserStore<User>(contextDb);
                var manager = new UserManager<User>(userStore);
                var email = identityClaims.FindFirst("Email").Value;

                var user = manager.Find(email, password);

                if (user != null)
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        [HttpPost]
        [Route("api/ChangePassword")]
        public IHttpActionResult ChangePassword(string oldPassword, string newPassword)
        {
            var identityClaims = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;

            if (claims.Count() > 0)
            {
                var contextDb = new ContextDB();
                var userStore = new UserStore<User>(contextDb);
                var manager = new UserManager<User>(userStore);
                var email = identityClaims.FindFirst("Email").Value;
                var userId = contextDb.Users.FirstOrDefault(f => f.Email == email)?.Id;
                if(userId != null)
                {
                    manager.PasswordValidator = new PasswordValidator
                    {
                        RequiredLength = 2
                    };

                    var result = manager.ChangePassword(userId, oldPassword, newPassword);
                    if (result.Succeeded)
                    {
                        return Ok("Everything passed well!");
                    }

                    return Content(HttpStatusCode.BadRequest, "Something's wrong!");
                }
            }

            return Content(HttpStatusCode.BadRequest, "Something's wrong!");
        }
    }
}