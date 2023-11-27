using Core.Domains.Auth;
using Data.DbContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Data.Context
{
    public static class Seeder
    {
        public static void SeedData(this IApplicationBuilder applicationBuilder)
        {
            SeedIdentity(applicationBuilder);
        }

        //===============================================================================
        private static void SeedIdentity(IApplicationBuilder applicationBuilder)
        {
            using var scope = applicationBuilder.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

            List<string> users = new() { "Admin" };
            users.ForEach(username =>
            {
                if (userManager.FindByNameAsync(username).Result == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = username,
                        Email = $"{username}@Admin.com",
                        FullName = $"Admin",
                        PhoneNumber = "0123456789",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                    };

                    var result = userManager.CreateAsync(user, "Admin111@@@").Result;
                    if (result.Succeeded)
                    {
                        var claims = new List<Claim>
                        {

                        };
                        userManager.AddClaimsAsync(user, claims).Wait();

                    }
                }
            });


        }


    }

}
