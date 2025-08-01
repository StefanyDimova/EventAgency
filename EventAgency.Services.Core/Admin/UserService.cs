using EventAgency.Services.Core.Admin.Interfaces;
using EventAgency.Web.ViewModels.Admin.UserManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Services.Core.Admin
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<bool> UserExistsByIdAsync(Guid userId)
        {
            IdentityUser? user = await this.userManager
                .FindByIdAsync(userId.ToString());

            return user != null;
        }

        public async Task<bool> AssignUserToRoleAsync(RoleSelectionInputModel inputModel)
        {
            IdentityUser? user = await this.userManager
                .FindByIdAsync(inputModel.UserId);

            if (user == null)
            {
                throw new ArgumentException("User does not exist!");
            }

            bool roleExists = await this.roleManager.RoleExistsAsync(inputModel.Role);
            if (!roleExists)
            {
                throw new ArgumentException("Selected role is not a valid role!");
            }

            try
            {
                await this.userManager.AddToRoleAsync(user, inputModel.Role);

                return true;
            }
            catch (Exception e)
            {
                throw new ArgumentException(
                    "Unexpected error occurred while adding the user to role! Please try again later!",
                    innerException: e);
            }
        }

        public async Task<IEnumerable<UserManagementIndexViewModel>> GetAllUsersAsync(string userId)
        {

            IEnumerable<UserManagementIndexViewModel> users = await this.userManager.Users
                .Where(u => u.Id.ToLower() != userId.ToLower())
                .Select(u => new UserManagementIndexViewModel()
                {
                    Id = u.Id,
                    Email = u.Email,
                    Roles = userManager.GetRolesAsync(u).GetAwaiter().GetResult()

                })
                .ToArrayAsync();

            return users;
        }

        public async Task<bool> RemoveUserRoleAsync(Guid userId, string roleName)
        {
            IdentityUser? user = await userManager
                .FindByIdAsync(userId.ToString());
            bool roleExists = await this.roleManager.RoleExistsAsync(roleName);

            if (user == null || !roleExists)
            {
                return false;
            }

            bool alreadyInRole = await this.userManager.IsInRoleAsync(user, roleName);
            if (alreadyInRole)
            {
                IdentityResult? result = await this.userManager
                    .RemoveFromRoleAsync(user, roleName);

                if (!result.Succeeded)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            IdentityUser? user = await userManager
                .FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return false;
            }

            IdentityResult? result = await this.userManager
                .DeleteAsync(user);
            if (!result.Succeeded)
            {
                return false;
            }

            return true;
        }
    }
}
