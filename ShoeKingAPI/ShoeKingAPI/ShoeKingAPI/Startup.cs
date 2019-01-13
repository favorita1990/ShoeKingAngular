using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using ShoeKingAPI.Context;
using ShoeKingAPI.Models;

[assembly: OwinStartup(typeof(ShoeKingAPI.Startup))]
namespace ShoeKingAPI
{
    public class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);

            var OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/token"),
                Provider = new ApplicationOAuthProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(60),
                AllowInsecureHttp = true
            };

            app.UseOAuthAuthorizationServer(OAuthOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);

        }

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();

            //GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        private void createRolesandUsers()
        {
            ContextDB context = new ContextDB();
            var userManager = new UserManager<User>(new UserStore<User>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            const string role = "Founder";
            const string email = "founder1990@abv.bg";
            const string pass = "12";
            if (!roleManager.RoleExists(role))
            {
                roleManager.Create(new IdentityRole(role));
            }

            var PasswordHash = new PasswordHasher();
            if (!context.Users.Any(u => u.Email == email))
            {
                var user = new User
                {
                    Email = email,
                    UserName = email,
                    FirstName = "Iliyan",
                    LastName = "Kodzehamnov",
                    Gender = "0",
                    PasswordHash = PasswordHash.HashPassword(pass),
                    CreationDate = DateTime.Now
                };

                userManager.Create(user);
                userManager.AddToRole(user.Id, role);
            }

            var cart = context.Carts.GroupBy(g => g.CartId).ToList();

            foreach (var items in cart)
            {
                var date = items.FirstOrDefault().DateCreated;
                if (date.Day != DateTime.Now.Day)
                {
                    context.Carts.RemoveRange(items);
                }
            }


            if (!context.OrderStatuses.Any(f => f.Name == "waiting"))
            {
                context.OrderStatuses.Add(new OrderStatus() { Name = "waiting" });
            }
            if (!context.OrderStatuses.Any(f => f.Name == "accepted"))
            {
                context.OrderStatuses.Add(new OrderStatus() { Name = "accepted" });
            }
            if (!context.OrderStatuses.Any(f => f.Name == "canceled"))
            {
                context.OrderStatuses.Add(new OrderStatus() { Name = "canceled" });
            }
            if (!context.OrderStatuses.Any(f => f.Name == "resend"))
            {
                context.OrderStatuses.Add(new OrderStatus() { Name = "resend" });
            }

            if (!context.About.Any())
            {
                var firstAbout = new About()
                {
                    FirstImage = "about1.jpg",
                    SecondImage = "about2.jpg",
                    FirstTextHeader = "Partnerships",
                    SecondTextHeader = "Partnerships",
                    FirstText = "Kodzhemanov has had a partnership with UNICEF since 2005.Kodzhemanov store worldwide donate a percentage of the sales for special collections made specifically for UNICEF to go toward the United Nations Children's Fund. The annual Kodzhemanov Campaign to Benefit UNICEF supports education, healthcare, protection and clean water programs for orphans and children affected by HIV/AIDS in sub-Saharan Africa. For the campaign in 2009, Michael Roberts promoted a children's book, \"Snowman in Africa\" with proceeds going to UNICEF. In five years, Kodzhemanov donated over $7 million to UNICEF. Kodzhemanov is the largest corporate donor to UNICEF's \"Schools for Africa\" that was established in 2004 by UNICEF, the Nelson Mandela Foundation, and the Hamburg Society. Its goal is to increase access to basic schooling for all, with a special emphasis on children orphaned by HIV/AIDS and children living in extreme poverty.",
                    SecondText = "With beginnings at the end of the 21th century, Shoe King\"s site became one of the world’s most successful manufacturers of high - end leather goods."
                };

                context.About.Add(firstAbout);
            }

            if (!context.HomePage.Any())
            {
                var homePage = new HomePage()
                {
                    TextHeader = "Welcome to paradise",
                    Text = "This is the best site you ever see.",
                    ImageUrl = "banner.jpg"
                };

                context.HomePage.Add(homePage);
            }

            context.SaveChanges();
        }
    }
}