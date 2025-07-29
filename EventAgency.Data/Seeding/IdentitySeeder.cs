using EventAgency.Data.Seeding.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


using static EventAgency.GCommon.ApplicationConstants;

namespace EventAgency.Data.Seeding
{
    public class IdentitySeeder : IIdentitySeeder
    {
        private readonly string[] DefaultRoles
            = { adminRoleName, userRoleName };

        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IUserStore<IdentityUser> userStore;
        private readonly IUserEmailStore<IdentityUser> emailStore;
        private readonly IConfiguration configuration;

        public IdentitySeeder(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, IUserStore<IdentityUser> userStore, IConfiguration configuration)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.userStore = userStore;
            this.configuration = configuration;

            this.emailStore = GetEmailStore();

        }
        public async Task SeedIdentityAsync() 
        {
            await this.SeedRolesAsync();
            await this.SeedUsersAsync();
        }

        private async Task SeedRolesAsync()
        {
           
            foreach (string defaultRole in DefaultRoles)
            {
                bool roleExists = await this.roleManager.RoleExistsAsync(defaultRole);

                if (!roleExists)
                {
                    IdentityRole newRole = new IdentityRole(defaultRole);
                    IdentityResult result 
                        = await roleManager.CreateAsync(newRole);

                    if (!result.Succeeded)
                    {
                        throw new Exception($"There was an exception while seeding the {defaultRole} role!");
                    }

                }
            }
        }

        private async Task SeedUsersAsync()
        {
            string? testUserEmail = this.configuration["UserSeed:TestUser:Email"];
            string? testUserPassword = this.configuration["UserSeed:TestUser:Password"];

            string? testAdminEmail = this.configuration["UserSeed:TestAdmin:Email"];
            string? testAdminPassword = this.configuration["UserSeed:TestAdmin:Password"];


            if(testUserEmail == null || testUserPassword == null
                || testAdminEmail == null || testAdminPassword == null)
            {
                throw new Exception($"There was an exception while obtaining the {nameof(testUserEmail)}, {nameof(testUserPassword)},{nameof(testAdminEmail)} and {nameof(testAdminPassword)} from the app configuration.");
            }
            IdentityUser testUser = new IdentityUser();

            IdentityUser? testUserSeeded = await this.userStore.FindByNameAsync(testUserEmail, CancellationToken.None);

            if (testUserSeeded == null)
            {
                await this.userStore.SetUserNameAsync(testUser, testUserEmail, CancellationToken.None);
                await this.emailStore.SetEmailAsync(testUser, testUserEmail, CancellationToken.None);

                IdentityResult result =  await this.userManager.CreateAsync(testUser, testUserPassword);
                if (!result.Succeeded)
                {
                    throw new Exception($"There was an exception while seeding the {testUserEmail} user!");
                }

                result = await userManager.AddToRoleAsync(testUser, userRoleName);

                if (!result.Succeeded)
                {
                    throw new Exception($"There was an exception while assigning the {userRoleName} role to the {testUserEmail} user!");
                }
            }


            IdentityUser adminUser = new IdentityUser();

            IdentityUser? testAdminSeeded = await this.userStore.FindByNameAsync(testAdminEmail, CancellationToken.None);
            if (testUserSeeded == null)
            {
                await this.userStore.SetUserNameAsync(adminUser, testAdminEmail, CancellationToken.None);
                await this.emailStore.SetEmailAsync(adminUser, testAdminEmail, CancellationToken.None);

                IdentityResult result = await this.userManager.CreateAsync(adminUser, testAdminPassword);

                if (!result.Succeeded)
                {
                    throw new Exception($"There was an exception while seeding the {testAdminEmail} admin!");
                }


                result = await userManager.AddToRoleAsync(adminUser, adminRoleName);


                if (!result.Succeeded)
                {
                    throw new Exception($"There was an exception while assigning the {adminRoleName} role to the {testAdminEmail} user!");
                }
            }


        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)userStore;
        }
    } 
}
