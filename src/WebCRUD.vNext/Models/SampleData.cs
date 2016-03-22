using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebCRUD.vNext.Models.Domain.Orders;

namespace WebCRUD.vNext.Models
{
    public static class SampleData
    {
        public static async void Initialize(IServiceProvider serviceProvider)
        {
            ApplicationDbContext context = serviceProvider.GetService<ApplicationDbContext>();
            context.Database.Migrate();

            if(!context.Roles.Any())
            {
                var role = new IdentityRole("admin");
                role.NormalizedName = "ADMIN";
                context.Roles.Add(role);
                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                var manager = serviceProvider.GetService<UserManager<ApplicationUser>>();
                var user = new ApplicationUser() { UserName = "admin" };
                await manager.CreateAsync(user, "admin");

                await manager.AddToRoleAsync(user, "admin");

                //context.SaveChanges();
            }

            if (!context.Customer.Any())
            {
                context.Customer.Add(new Customer("Customer 1", "123-456", "asd", "fgh", "jkl", "qwe"));
                context.Customer.Add(new Customer("Customer 2", "098-765", "poi", "uyt", "rew", "qas"));
                context.Customer.Add(new Customer("Customer 3", "987-123", "zxc", "vbn", "mkl", "poi"));

                //context.SaveChanges();
            }

            if (!context.Product.Any())
            {
                context.Product.AddRange(
                    new Product("Product 1", "P1", new decimal(9.99)),
                    new Product("Product 2", "P2", new decimal(10.5)),
                    new Product("Product 3", "P3", new decimal(40.4))
                );

                //context.SaveChanges();
            }

            context.SaveChanges();
        }

        /// <summary>
        /// Creates a store manager user who can manage the inventory.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
//        private static async Task CreateAdminUser(IServiceProvider serviceProvider)
//        {
//            var appEnv = serviceProvider.GetService<IApplicationEnvironment>();

//            var builder = new ConfigurationBuilder()
//                .SetBasePath(appEnv.ApplicationBasePath)
//                .AddJsonFile("config.json")
//                .AddEnvironmentVariables();
//            var configuration = builder.Build();

//            //const string adminRole = "Administrator";

//            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
//            // TODO: Identity SQL does not support roles yet
//            //var roleManager = serviceProvider.GetService<ApplicationRoleManager>();
//            //if (!await roleManager.RoleExistsAsync(adminRole))
//            //{
//            //    await roleManager.CreateAsync(new IdentityRole(adminRole));
//            //}

//            var user = await userManager.FindByNameAsync("admin");
//            if (user == null)
//            {
//                user = new ApplicationUser { UserName = "admin" };
//                await userManager.CreateAsync(user, "admin");
//                //await userManager.AddToRoleAsync(user, adminRole);
//                await userManager.AddClaimAsync(user, new Claim("ManageStore", "Allowed"));
//            }

//#if TESTING
//            var envPerfLab = configuration["PERF_LAB"];
//            if (envPerfLab == "true")
//            {
//                for (int i = 0; i < 100; ++i)
//                {
//                    var email = string.Format("User{0:D3}@example.com", i);
//                    var normalUser = await userManager.FindByEmailAsync(email);
//                    if (normalUser == null)
//                    {
//                        await userManager.CreateAsync(new ApplicationUser { UserName = email, Email = email }, "Password~!1");
//                    }
//                }
//            }
//#endif
//        }
    }
}
