using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.OAuth;
using ShoeKingAPI.Context;
using ShoeKingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace ShoeKingAPI
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            await Task.Run(() => {
                context.Validated();
            });
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var contextDb = new ContextDB();
            var userStore = new UserStore<User>(contextDb);
            var manager = new UserManager<User>(userStore);
            var user = await manager.FindAsync(context.UserName, context.Password);
            if (user != null)
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim("Username", user.UserName));
                identity.AddClaim(new Claim("Email", user.Email));
                var role = await manager.GetRolesAsync(user.Id);
                identity.AddClaim(new Claim("Role", role.ElementAt(0)));
                identity.AddClaim(new Claim("FirstName", user.FirstName));
                identity.AddClaim(new Claim("LastName", user.LastName));
                identity.AddClaim(new Claim("LoggedOn", DateTime.Now.ToString()));
                context.Validated(identity);
            }
            else
                return;
        }

        //public override Task TokenEndpointResponse(OAuthTokenEndpointResponseContext context)
        //{
        //    var accessToken = context.AccessToken;
        //    return Task.FromResult<object>(null);
        //}
    }
}